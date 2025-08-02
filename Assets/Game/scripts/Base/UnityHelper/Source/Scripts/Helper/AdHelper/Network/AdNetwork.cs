using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public abstract class AdNetwork
    {
        private string m_appId = null;
        private eAdNetwork m_type = eAdNetwork.AdMob;
        private bool m_isTestDevice = false;
        private Dictionary<eAdFormat, AdFormat> m_formats = new Dictionary<eAdFormat, AdFormat>();

        public eAdNetwork type { get { return m_type; } }

        public static AdNetwork create(eAdNetwork networkType)
        {
            switch (networkType)
            {
                case eAdNetwork.AdMob: return new AdMob();
                case eAdNetwork.UnityAds: return new AdUnityAds();
                case eAdNetwork.AppLovin: return new AdAppLovin();
            }

            if (Logx.isActive)
                Logx.error("Failed create AdNetwork, {0}", networkType);

            return null;
        }

        public void initialize(GameObject parent, eAdNetwork networkType, BaseAdSettings.AdDevice device, bool isTestId, bool isTestDevice)
        {
            if (Logx.isActive)
                Logx.assert(null != device, "device is null");

            m_appId = device.appId;
            m_type = networkType;
            m_isTestDevice = isTestDevice;

            var format = (isTestId) ? device.test : device.production;
            initFormat(parent, format);
        }

        protected abstract void initFormat(GameObject parent, BaseAdSettings.AdFormat format);

        protected void createFormat<T>(GameObject parent, eAdFormat formatType, string formatId) where T : AdFormat
        {
            if (string.IsNullOrEmpty(formatId))
                return;

            GameObject obj = new GameObject();
            obj.name = typeof(T).Name;
            obj.transform.parent = parent.transform;
            T t = obj.AddComponent<T>();
            t.initialize(m_appId, formatId, m_isTestDevice);

            m_formats.Add(formatType, t);
        }

        private AdFormat getFormat(eAdFormat formatType)
        {
            AdFormat format = null;
            if (m_formats.TryGetValue(formatType, out format))
                return format;

            return null;
        }

        public void forceRequest(eAdFormat formatType)
        {
            AdFormat adFormat = getFormat(formatType);
            if (null == adFormat)
                return;

            adFormat.forceRequest();
        }

        public bool show(eAdFormat formatType, Action<eAdResult> callback)
        {
            if (Logx.isActive)
                Logx.assert(null != callback, "callback is null");

            AdFormat adFormat = getFormat(formatType);
            if (null == adFormat)
                return false;

#if UNITY_EDITOR
            //callback(eAdResult.Rewarded);
            //callback(eAdResult.Closed);
            //return true;

            if (!adFormat.isLoaded())
            {
                if (Logx.isActive)
                    Logx.trace("AdNetwork not isLoaded {0}", formatType);

                adFormat.request();
                return false;
            }

            return adFormat.show(callback);
#else
            if (!adFormat.isLoaded())
            {
                if (Logx.isActive)
                    Logx.trace("AdNetwork not isLoaded {0}", formatType);

                adFormat.request();
                return false;
            }

            return adFormat.show(callback);
#endif
        }

        public bool isLoaded(eAdFormat formatType)
        {
            AdFormat adFormat = getFormat(formatType);
            if (null == adFormat)
                return false;

            return adFormat.isLoaded();
        }
    }
}