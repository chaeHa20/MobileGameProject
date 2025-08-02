using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace UnityHelper
{
    public class BaseAdSettings : ScriptableObject
    {
        [Serializable]
        public class AdNetwork
        {
            public bool isEnable;
            public eAdNetwork type;
            public List<AdDevice> devices = new List<AdDevice>();

            public AdDevice findDevice(eDevice deviceType)
            {
                AdDevice device = (from d in devices
                                   where d.deviceType == deviceType
                                   select d).FirstOrDefault();

                if (null == device)
                {
                    if (Logx.isActive)
                        Logx.error("Failed find ad device {0}", deviceType);
                }

                return device;
            }
        }

        [Serializable]
        public class AdDevice
        {
            public eDevice deviceType;
            public string appId;
            public AdFormat production = new AdFormat();
            public AdFormat test = new AdFormat();
        }

        [Serializable]
        public class AdFormat
        {
            public string bannerId;
            public string interstitialId;
            public string rewardId;
            public string nativeId;
        }

        [Serializable]
        public class AdFormatId
        {
            public eAdFormat type;
            public string id;
        }

        [SerializeField] bool m_isTestId = true;
        [SerializeField] bool m_isTestDevice = true;
        [SerializeField] List<AdNetwork> m_ads = new List<AdNetwork>();

        public List<AdNetwork>.Enumerator getAdEnumerator()
        {
            return m_ads.GetEnumerator();
        }

        public AdNetwork findNetwork(eAdNetwork adNetworkType)
        {
            var adNetwork = (from n in m_ads
                             where n.type == adNetworkType
                             select n).FirstOrDefault();

            return adNetwork;
        }

        public bool isTestId { get { return m_isTestId; } }
        public bool isTestDevice { get { return m_isTestDevice; } }
    }
}