using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

namespace UnityHelper
{
    /// <summary>
    /// http://genieker.tistory.com/149 참고
    /// 안쓰인다. AndroidAppUpdateHelper를 쓰자
    /// </summary>
    public class AndroidVersionChecker
    {
        /// <param name="callback">need update, player store version</param>
        public void check(MonoBehaviour mono, int maxCheckCount, float timeOut, Action<bool, string> callback)
        {
            if (SystemHelper.isInternetReachable())
            {
                mono.StartCoroutine(coCheck(maxCheckCount, timeOut, callback));
            }
            else
            {
                callback(false, null);
            }
        }

        private bool isAlreadyCheck(int maxCheckCount, string playStoreVersion)
        {
            int checkCount = 0;
            if (PlayerPrefs.HasKey(playStoreVersion))
            {
                checkCount = PlayerPrefs.GetInt(playStoreVersion);
                if (checkCount >= maxCheckCount)
                    return true;
            }

            PlayerPrefs.SetInt(playStoreVersion, ++checkCount);
            return false;
        }

        /// <param name="callback">need update, player store version</param>
        IEnumerator coCheck(int maxCheckCount, float timeOut, Action<bool, string> callback)
        {
            string packageName = Application.identifier;
            string httpUrl = "https://play.google.com/store/apps/details?id=" + packageName + "&hl=en";

            if (Logx.isActive)
                Logx.trace("android client version check httpUrl {0}", httpUrl);

            using (UnityWebRequest www = UnityWebRequest.Get(httpUrl))
            {
                www.timeout = (int)timeOut;
                yield return www.SendWebRequest();

                if (string.IsNullOrEmpty(www.error))
                {
                    try
                    {
                        int htlgb_len = "htlgb".Length;
                        string[] parts = www.downloadHandler.text.Split(new string[] { "Current Version" }, StringSplitOptions.None);
                        // 100은 임시
                        string res1 = parts[1].Substring(parts[1].IndexOf("htlgb") + htlgb_len, 100);
                        string res2 = res1.Substring(res1.IndexOf("htlgb") + htlgb_len + 1);

                        int start_index = res2.IndexOf(">");
                        int end_index = res2.IndexOf("<");
                        string playStoreVersion = res2.Substring(start_index + 1, end_index - start_index - 1);

                        if (Logx.isActive)
                            Logx.trace("play store version {0}", playStoreVersion);

                        if (isAlreadyCheck(maxCheckCount, playStoreVersion))
                        {
                            callback(false, null);
                        }
                        else
                        {
                            string curVersion = Application.version;

                            if (Logx.isActive)
                                Logx.trace("client version curVersion {0}", curVersion);

                            if (playStoreVersion.Trim().Equals(curVersion))
                            {
                                callback(false, null);
                            }
                            else
                            {
                                callback(true, playStoreVersion);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        callback(false, e.ToString());
                    }
                }
                else
                {
                    if (Logx.isActive)
                        Logx.trace("client version check error : {0}", www.error);

                    callback(false, null);
                }
            }
        }
    }
}