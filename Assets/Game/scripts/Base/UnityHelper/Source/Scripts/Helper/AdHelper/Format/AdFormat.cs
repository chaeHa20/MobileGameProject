using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if _ADMOB_
using GoogleMobileAds.Api;
#endif

namespace UnityHelper
{
    public class AdFormat : MonoBehaviour
    {
        private string m_id = null;
        private bool m_isRequesting = false;
        private bool m_isTestDevice = false;
        protected Action<eAdResult> m_callback = null;

        public string id { get { return m_id; } }

        public virtual void initialize(string appId, string formatId, bool isTestDevice)
        {
            m_id = formatId;
            m_isTestDevice = isTestDevice;

            if (Logx.isActive)
                Logx.trace("AdFormat initialize appId {0}, formatId {1}, isTestDevice {2}", appId, formatId, isTestDevice);
        }

        public virtual bool show(Action<eAdResult> callback)
        {
            if (Logx.isActive)
                Logx.assert(null != callback, "callback is null");

            m_callback = callback;
            return true;
        }

        public virtual void hide()
        {

        }

        public void forceRequest()
        {
            clearRequest();
            request();
        }

        public virtual bool request()
        {
            if (m_isRequesting)
            {
                if (Logx.isActive)
                    Logx.warn("Already requesting");

                return false;
            }

            m_isRequesting = true;
            return true;
        }

        protected void clearRequest()
        {
            if (Logx.isActive)
                Logx.trace("Admob log clearRequest");

            m_isRequesting = false;
        }

        public virtual bool isLoaded()
        {
            return false;
        }
    }
}