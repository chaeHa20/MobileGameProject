using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if _ADMOB_
using GoogleMobileAds.Api;
#elif _ONE_ADMAX_
using ONEAdMax;
#endif

namespace UnityHelper
{
    public class AdHelper : MonoSingleton<AdHelper>
    {
        private List<AdNetwork> m_networks = new List<AdNetwork>();
        private AdImplementor m_implementor = new AdImplementor();
        private AdShowReceiver m_showReceiver = new AdShowReceiver();

        private Action m_initCallback = null;
        private bool m_isInitialized = false;
        private BaseAdSettings m_settings = null;

        public int networkCount => m_networks.Count;
        public bool isShowing => m_showReceiver.isShowing;

        public virtual void initialize(BaseAdSettings settings, Action callback)
        {
            clearChilds();

            m_initCallback = callback;
            m_settings = settings;

#if _ADMOB_
            m_isInitialized = false;
            initMobileAds(settings, callback);
#elif _APPLOVIN_
            m_isInitialized = false;
            initMaxSdk();
#elif _ONE_ADMAX_
            m_isInitialized = false;
            initOneAdMaxSdk();
#else
            m_isInitialized = true;
#endif
        }

        private void clearChilds()
        {
            foreach (Transform child in transform)
                Destroy(child.gameObject);
        }

#if _ADMOB_
        private void initMobileAds(BaseAdSettings settings, Action callback)
        {
            MobileAds.Initialize((initializationStatus) =>
            {
                if (Logx.isActive)
                    Logx.trace("MobileAds initialize complete");

                MobileAds.SetRequestConfiguration(setTestDevice(settings.isTestDevice));

                m_isInitialized = true;
            });
        }
#elif _APPLOVIN_
        private void initMaxSdk()
        {
            MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) => {
                // AppLovin SDK is initialized, start loading ads

                m_isInitialized = true;
            };

            MaxSdk.InitializeSdk();
        }
#elif _ONE_ADMAX_
        private void initOneAdMaxSdk()
        {
            if (!m_isInitialized)
            {
                ONEAdMaxClient.SetLogEnable(true);
                ONEAdMaxClient.Initialize(() =>
                {
                    ONEAdMaxClient.GdprConsentAvailable(true);
                    ONEAdMaxClient.TagForChildDirectedTreatment(false);
                    m_isInitialized = true;
                });
            }
            else
            {
                ONEAdMaxClient.SetLogEnable(true);
                ONEAdMaxClient.GdprConsentAvailable(true);
                ONEAdMaxClient.TagForChildDirectedTreatment(false);
            }
        }
#endif

        private void initialize(List<BaseAdSettings.AdNetwork>.Enumerator e, bool isTestId, bool isTestDevice)
        {
            initNetworks(e, isTestId, isTestDevice);
            m_implementor.initialize();
        }

        private void initNetworks(List<BaseAdSettings.AdNetwork>.Enumerator e, bool isTestId, bool isTestDevice)
        {
            m_networks.Clear();

            eDevice deviceType = getDeviceType();

            while (e.MoveNext())
            {
                var network = e.Current;
                if (!network.isEnable)
                    continue;

                AdNetwork adNetwork = AdNetwork.create(network.type);
                if (null == adNetwork)
                    continue;

                var device = network.findDevice(deviceType);
                adNetwork.initialize(gameObject, network.type, device, isTestId, isTestDevice);

                m_networks.Add(adNetwork);
            }
        }

        public void show(eAdFormat adFormat, Action<AdResults> callback)
        {
            if (m_showReceiver.isShowing)
            {
                if (Logx.isActive)
                    Logx.trace("Ads show, isShowing, format {0}", adFormat);

                if (null != callback)
                    callback(new AdResults(adFormat));
                return;
            }

            StartCoroutine(coShow(adFormat, callback));
        }

        public void forceRequest(eAdFormat adFormat)
        {
            m_implementor.forceRequest(this, adFormat);
        }

        IEnumerator coShow(eAdFormat adFormat, Action<AdResults> callback)
        {
            if (Logx.isActive)
                Logx.trace("Ads coShow, format {0}", adFormat);

            m_showReceiver.beginShow(adFormat);
            m_implementor.show(this, adFormat, (result) =>
            {
                if (Logx.isActive)
                    Logx.trace("Ads coShow result {0}", result);

                m_showReceiver.setResult(result);
            });

            yield return new WaitUntil(() => m_showReceiver.isClosed);

            var adResults = m_showReceiver.endShow();
            callback(adResults);
        }

        public AdNetwork getNetwork(int networkIndex)
        {
            if (m_networks.Count <= networkIndex)
            {
                if (Logx.isActive)
                {
                    Logx.warn("Invalid network index {0}", networkIndex);
                }

                return null;
            }

            return m_networks[networkIndex];
        }

        public bool isLoaded(eAdFormat adFormat)
        {
            foreach (var net in m_networks)
            {
                if (net.isLoaded(adFormat))
                    return true;
            }

            return false;
        }

        private eDevice getDeviceType()
        {
#if UNITY_ANDROID
            return eDevice.Android;
#elif UNITY_IOS
            return eDevice.Ios;
#else
            return eDevice.Window;
#endif
        }

#if _ADMOB_
        protected RequestConfiguration setTestDevice(bool isTestDevice)
        {
            List<string> testDeviceIds = new List<string>();

            if (isTestDevice)
            {
                testDeviceIds.Add(AdRequest.TestDeviceSimulator);
                testDeviceIds.Add("BA78D827C5B2EF3E44776ECD50E962F8");
                testDeviceIds.Add("D5CC6D0AC97291A3FFEC0A5402BDAF0E");
                testDeviceIds.Add("fe11bee8c1376f30a82d8d7778685e03dc661078");
                testDeviceIds.Add("5c399f9746421af10620ff7ae16014e14a5a3f9d");
                testDeviceIds.Add("00008020-00011C413A62002E");
                testDeviceIds.Add("877137591c5d903ce07e9b29c802dc99");

                return new RequestConfiguration.Builder().SetTestDeviceIds(testDeviceIds).build();
            }
            else
            {
                testDeviceIds.Add(AdRequest.TestDeviceSimulator);
                return new RequestConfiguration.Builder().SetTestDeviceIds(testDeviceIds).build();
            }
        }
#endif



        protected override void OnApplicationQuit()
        {
#if _ONE_ADMAX_
            ONEAdMaxClient.Destroy();
#endif
            base.OnApplicationQuit();
        }


        void Update()
        {
            if (m_isInitialized)
            {
                m_isInitialized = false;

                initialize(m_settings.getAdEnumerator(), m_settings.isTestId, m_settings.isTestDevice);
                m_initCallback?.Invoke();
            }
        }
    }
}