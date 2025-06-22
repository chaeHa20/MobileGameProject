using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public class LocalePlugin : MonoBehaviour
    {
        [SerializeField] List<eLanguage> m_validLanguages = new List<eLanguage>();

        private Action<eLanguage> m_callback = null;

        public void initialize(eLanguage language, Action<eLanguage> callback)
        {
            if (Logx.isActive)
                Logx.assert(null != callback, "callback is null");

            m_callback = callback;

            if (eLanguage.None == language)
            {
#if UNITY_EDITOR
                setGameLanguage(language);
#else
    #if UNITY_ANDROID
                setAndroidLanguage();
    #else
                setGameLanguage(language);
    #endif
#endif
            }
            else
            {
                setGameLanguage(language);
            }
        }

#if UNITY_ANDROID
        private void setAndroidLanguage()
    {
        //AndroidJavaClass unity_jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        //AndroidJavaObject unity_jo = unity_jc.GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaClass jc = new AndroidJavaClass("com.nexelon.localeplugin.LocalePluginActivity");
        AndroidJavaObject jo = jc.CallStatic<AndroidJavaObject>("instance");
        jo.Call("getCountry");
        jo.Call("getLanguage");
    }
#endif

        /// <summary>
        /// Android Native에서 호출
        /// </summary>
        /// <param name="str">language/displayLanguage/iosLanguage</param>
        public void setLanguage(string str)
        {
            string[] codes = str.Split('/');
            eLanguage code;
            if (null == codes || 3 != codes.Length)
            {
                code = LanguageHelper.getDeviceLanguage();
            }
            else
            {
                string defaultCode = codes[0];

                if (string.Equals("hi", defaultCode))
                {
                    code = eLanguage.Hi;
                }
                else if (string.Equals("ms", defaultCode))
                {
                    code = eLanguage.My;
                }
                else
                {
                    code = LanguageHelper.getDeviceLanguage();
                }
            }

            setGameLanguage(code);
        }

        /// <summary>
        /// Android Native에서 호출
        /// </summary>
        /// <param name="str">country/displayCountry/iosCountry</param>
        public void setCountry(string str)
        {
            string[] codes = str.Split('/');

            if (Logx.isActive)
            {
                Logx.trace("Locale country {0}", str);
                Logx.assert(3 == codes.Length, "Invalid locale country {0}", str);
            }
        }

        private void setGameLanguage(eLanguage language)
        {
            LanguageHelper.language = getValidLanguage(language);
            m_callback(LanguageHelper.language);
        }

        private eLanguage getValidLanguage(eLanguage language)
        {
            if (m_validLanguages.Exists(x => x == language))
                return language;

#if UNITY_EDITOR
            return eLanguage.Kr;
#else
            return eLanguage.En;
#endif
        }
    }
}