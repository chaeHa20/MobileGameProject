using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ANDROID && _ANDROID_APP_UPDATE_
using Google.Play.AppUpdate;
using Google.Play.Common;
using System;

namespace UnityHelper
{
    /// <summary>
    /// https://developer.android.com/guide/playcore/in-app-updates/unity?hl=ko
    /// </summary>
    public class AndroidAppUpdateHelper
    {
        /// <param name="callback">isUpdateAvailable</param>
        public void check(MonoBehaviour mono, Action<bool> callback)
        {
#if UNITY_EDITOR
            callback(false);
#else
            if (SystemHelper.isInternetReachable())
            {
                mono.StartCoroutine(coCheckForUpdate(callback));
            }
            else
            {
                callback(false);
            }
#endif
        }

        /// <param name="callback">isUpdateAvailable</param>
        IEnumerator coCheckForUpdate(Action<bool> callback)
        {
            var appUpdateManager = new AppUpdateManager();

            PlayAsyncOperation<AppUpdateInfo, AppUpdateErrorCode> appUpdateInfoOperation = appUpdateManager.GetAppUpdateInfo();

            // Wait until the asynchronous operation completes.
            yield return appUpdateInfoOperation;

            if (Logx.isActive)
                Logx.trace("AppUpdate Info Operation IsSuccessful {0}", appUpdateInfoOperation.IsSuccessful);

            if (appUpdateInfoOperation.IsSuccessful)
            {
                var appUpdateInfoResult = appUpdateInfoOperation.GetResult();
                if (null == appUpdateInfoResult)
                {
                    if (Logx.isActive)
                        Logx.trace("AppUpdate Info Result is null");

                    callback(false);
                }
                else
                {
                    if (Logx.isActive)
                        Logx.trace("AppUpdate Info Result {0}", appUpdateInfoResult.ToString());

                    if (isAlreadyCheck(appUpdateInfoResult.AvailableVersionCode))
                    {
                        callback(false);
                    }
                    else
                    {
                        // Check AppUpdateInfo's UpdateAvailability, UpdatePriority,
                        // IsUpdateTypeAllowed(), etc. and decide whether to ask the user
                        // to start an in-app update.

                        if (Logx.isActive)
                            Logx.trace("AppUpdate UpdateAvailable : {0}", appUpdateInfoResult.UpdateAvailability);

                        if (UpdateAvailability.UpdateAvailable == appUpdateInfoResult.UpdateAvailability)
                            callback(true);
                        else
                            callback(false);
                    }
                }
            }
            else
            {
                callback(false);
            }
        }

        private bool isAlreadyCheck(int versionCode)
        {
            string key = string.Format("AppUpdate {0}_VersionCode{1}", Application.identifier, versionCode);

            if (PlayerPrefs.HasKey(key))
            {
                if (Logx.isActive)
                    Logx.trace("AppUpdate isAlreadyChecked versionCode {0}", versionCode);
                return true;
            }

            PlayerPrefs.SetInt(key, 1);
            return false;
        }
    }
}
#endif