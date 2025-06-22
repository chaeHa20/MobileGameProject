using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace UnityHelper
{
    public class AndroidHelper
    {
        /// <summary>
        /// https://answers.unity.com/questions/537476/open-gallery-android.html
        /// .png, .jpg, .jpeg
        /// </summary>
        /// <returns></returns>
        public static List<string> getAllGalleryImagePaths(int limit = 0)
        {
            List<string> results = new List<string>();
            HashSet<string> allowedExtesions = new HashSet<string>() { ".png", ".jpg", ".jpeg" };

            try
            {
                AndroidJavaClass mediaClass = new AndroidJavaClass("android.provider.MediaStore$Images$Media");

                // Set the tags for the data we want about each image.  This should really be done by calling; 
                //string dataTag = mediaClass.GetStatic<string>("DATA");
                // but I couldn't get that to work...

                const string dataTag = "_data";

                string[] projection = new string[] { dataTag };
                AndroidJavaClass player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject currentActivity = player.GetStatic<AndroidJavaObject>("currentActivity");

                string[] urisToSearch = new string[] { "EXTERNAL_CONTENT_URI", "INTERNAL_CONTENT_URI" };
                foreach (string uriToSearch in urisToSearch)
                {
                    AndroidJavaObject externalUri = mediaClass.GetStatic<AndroidJavaObject>(uriToSearch);
                    AndroidJavaObject finder = currentActivity.Call<AndroidJavaObject>("managedQuery", externalUri, projection, null, null, null);
                    bool foundOne = finder.Call<bool>("moveToFirst");
                    while (foundOne)
                    {
                        int dataIndex = finder.Call<int>("getColumnIndex", dataTag);
                        string data = finder.Call<string>("getString", dataIndex);
                        if (allowedExtesions.Contains(Path.GetExtension(data).ToLower()))
                        {
                            string path = @"file:///" + data;
                            results.Add(path);

                            if (0 < limit)
                            {
                                if (results.Count >= limit)
                                    return results;
                            }
                        }

                        foundOne = finder.Call<bool>("moveToNext");
                    }
                }
            }
            catch (Exception e)
            {
                if (Logx.isActive)
                    Logx.exception(e);
            }

            return results;
        }

        /// <summary>
        /// https://docs.unity3d.com/ScriptReference/SystemInfo-operatingSystem.html
        /// https://answers.unity.com/questions/976506/how-to-get-andriod-api-level.html
        /// </summary>
        /// <returns></returns>
        public static int getApiLevel()
        {
            int index = SystemInfo.operatingSystem.IndexOf("-");
            if (0 > index)
                return 0;

            int apiLevel = int.Parse(SystemInfo.operatingSystem.Substring(index + 1, 3));
            return apiLevel;
        }

        /// <summary>
        /// 안드로이드 OS가 오레오 이상인지
        /// </summary>
        /// <returns></returns>
        public static bool isOreoOver()
        {
            int apiLevel = getApiLevel();
            return (26 <= apiLevel);
        }

        /// <summary>
        /// cross app에서 갖고 옴
        /// </summary>
        /// <returns></returns>
        public static bool isAppInstalled(string packageName)
        {
            if (Logx.isActive)
                Logx.assert(!string.IsNullOrEmpty(packageName), "packageName is null or empty");

            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject packageManager = currentActivity.Call<AndroidJavaObject>("getPackageManager");

            AndroidJavaObject launchIntent = null;

            try
            {
                launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", packageName);
            }
            catch (Exception ex)
            {
                Debug.Log("exception" + ex.Message);
            }

            return (launchIntent != null);
        }

        public static void shareScreenShot(MonoBehaviour mono, string title, string description, string filename, bool needCapture, Action callback)
        {
            if (Logx.isActive)
            {
                Logx.assert(null != mono, "mono is null");
                Logx.assert(!string.IsNullOrEmpty(title), "title is null or empty");
                Logx.assert(!string.IsNullOrEmpty(description), "description is null or empty");
                Logx.assert(!string.IsNullOrEmpty(filename), "filename is null or empty");
            }

            mono.StartCoroutine(coShareScreenShot(mono, title, description, filename, needCapture, callback));
        }

        private static IEnumerator coShareScreenShot(MonoBehaviour mono, string title, string description, string filename, bool needCapture, Action callback)
        {
            yield return new WaitForEndOfFrame();

            string fullPath = GraphicHelper.getScreenShotFullPath(filename);
            if (needCapture)
                yield return mono.StartCoroutine(GraphicHelper.coCaptureScreenShot(fullPath, null));

            if (File.Exists(fullPath) == true)
            {
                if (Logx.isActive)
                    Logx.trace("share exist file {0}", fullPath);

                try
                {
                    callAndroidFileProvider(fullPath, title, description);
                }
                catch (Exception e)
                {
                    if (Logx.isActive)
                        Logx.exception(e);
                }
            }
            else
            {
                if (Logx.isActive)
                    Logx.trace("Not exist screen shopt {0}", fullPath);
            }

            callback?.Invoke();

            yield return null;
        }

        private static void callAndroidFileProvider(string fullPath, string title, string description)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject unityContext = currentActivity.Call<AndroidJavaObject>("getApplicationContext");

            string packageName = unityContext.Call<string>("getPackageName");
            string authority = packageName + ".fileprovider";

            AndroidJavaObject fileObject = new AndroidJavaObject("java.io.File", fullPath);
            AndroidJavaClass fileProvider = new AndroidJavaClass("androidx.core.content.FileProvider");
            AndroidJavaObject uriObject = fileProvider.CallStatic<AndroidJavaObject>("getUriForFile", unityContext, authority, fileObject);

            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent", intentClass.GetStatic<string>("ACTION_VIEW"));
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), title);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), description);

            intentObject.Call<AndroidJavaObject>("setDataAndType", uriObject, "image/png");
            intentObject.Call<AndroidJavaObject>("addFlags", intentClass.GetStatic<int>("FLAG_ACTIVITY_NEW_TASK"));
            intentObject.Call<AndroidJavaObject>("addFlags", intentClass.GetStatic<int>("FLAG_GRANT_READ_URI_PERMISSION"));
            intentObject.Call<AndroidJavaObject>("addFlags", intentClass.GetStatic<int>("FLAG_GRANT_WRITE_URI_PERMISSION"));

            AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, title);
            currentActivity.Call("startActivity", chooser);
        }
    }
}