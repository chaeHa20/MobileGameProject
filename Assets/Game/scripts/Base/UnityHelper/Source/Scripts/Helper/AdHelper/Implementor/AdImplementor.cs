using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public class AdImplementor
    {
        private Dictionary<eAdFormat, AdImplementorFormat> m_formats = new Dictionary<eAdFormat, AdImplementorFormat>();

        public void initialize()
        {
            initFormats();
        }

        private void initFormats()
        {
            m_formats.Clear();
            
            SystemHelper.forEachEnum<eAdFormat>((type) =>
            {
                m_formats.Add(type, new AdImplementorFormat());
            });
        }

        public void forceRequest(AdHelper adHelper, eAdFormat adFormat)
        {
            if (!SystemHelper.isInternetReachable())
            {
                return;
            }

            if (m_formats.TryGetValue(adFormat, out AdImplementorFormat format))
            {
                format.forceRequest(adHelper, adFormat);
            }
        }

        public void show(AdHelper adHelper, eAdFormat adFormat, Action<eAdResult> callback)
        {
            if (Logx.isActive)
                Logx.assert(null != adHelper, "adHelper is null");

            if (!SystemHelper.isInternetReachable())
            {
                if (null != callback)
                    callback(eAdResult.InternetNotRechable);
                return;
            }

            if (m_formats.TryGetValue(adFormat, out AdImplementorFormat format))
            {
                format.show(adHelper, adFormat, callback);
            }
            else
            {
                if (null != callback)
                    callback(eAdResult.Exhausted);
            }
        }
    }
}