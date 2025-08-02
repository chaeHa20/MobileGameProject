using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;

namespace UnityHelper
{
    public class SocialCloud
    {
        public enum eResult { SUCCESS, INTERNET_REACHABLE, NOT_LOGIN, FAIL };

        protected string m_filename = null;
        protected Crypto m_crypto = new Crypto();

        public static SocialCloud create()
        {
#if UNITY_EDITOR
            return new EditorCloud();
#else
#if UNITY_ANDROID
            return new GoogleCloud();
#else
            return new SocialCloud();
#endif
#endif
        }

        public virtual void initialize(string filename, Crypto crypto)
        {
            m_filename = filename;
            m_crypto = crypto;
        }

        public virtual bool isLoggedIn()
        {
            return false;
        }

        /// <param name="callback">success, error</param>
        public virtual void login(bool isDownloadProfile, Action<bool, string> callback)
        {
            callback?.Invoke(true, null);
        }

        public virtual void save(Action<eResult> callback)
        {
            callback?.Invoke(eResult.FAIL);
        }

        public virtual void load(Action<eResult> callback)
        {
            callback?.Invoke(eResult.FAIL);
        }

        protected eResult isValid()
        {
            if (!SystemHelper.isInternetReachable())
            {
                return eResult.INTERNET_REACHABLE;
            }

            if (!isLoggedIn())
            {
                return eResult.NOT_LOGIN;
            }

            return eResult.SUCCESS;
        }

        protected TimeSpan getPlayedTime()
        {
            return new TimeSpan(DateTime.Now.Ticks);
        }

        public virtual void getSavedDataTime(Action<string> callback)
        {
            callback?.Invoke("");
        }

        protected string getNowSavedTimeString()
        {
            return DateTimeHelper.toString(DateTime.Now, "MMM dd, yyyy(H:mm:ss)");
        }
    }
}