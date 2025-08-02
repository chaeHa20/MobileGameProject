using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    [Serializable]
    public class AdImplementorFormat
    {
        [SerializeField] int m_networkIndex = 0;

        public void forceRequest(AdHelper adHelper, eAdFormat formatType)
        {
            int networkIndex = m_networkIndex;
            for (int i = 0; i < adHelper.networkCount; ++i)
            {
                AdNetwork adNetwork = adHelper.getNetwork(networkIndex);
                adNetwork.forceRequest(formatType);
            }
        }

        public void show(AdHelper adHelper, eAdFormat formatType, Action<eAdResult> callback)
        {
            if (Logx.isActive)
                Logx.assert(null != adHelper, "adHelper is null");

            int networkIndex = m_networkIndex;
            bool isShow = false;
            for (int i = 0; i < adHelper.networkCount; ++i)
            {
                AdNetwork adNetwork = adHelper.getNetwork(networkIndex);
                if (null == adNetwork)
                    continue;

                isShow = adNetwork.show(formatType, (result) =>
                {
                    if (null != callback)
                        callback(result);
                });

                if (Logx.isActive)
                    Logx.trace("AdImplementorFormat show {0}", isShow);

                incNetworkIndex(adHelper.networkCount);

                if (isShow)
                    break;
            }

            if (!isShow)
            {
                if (null != callback)
                    callback(eAdResult.Exhausted);
            }
        }

        private void incNetworkIndex(int maxNetworkdCount)
        {
            ++m_networkIndex;
            if (m_networkIndex >= maxNetworkdCount)
                m_networkIndex = 0;
        }
    }
}