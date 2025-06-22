using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if _ADMOB_
using GoogleMobileAds.Api;
#endif

namespace UnityHelper
{
    public class AdMobNative : AdNative
    {
#if _ADMOB_
        /*
        private AdLoader m_adLoader = null;
        private UnifiedNativeAd m_nativeAd = null;
        private bool m_isUnifiedNativeAdLoaded = false;

        public UnifiedNativeAd nativeAd { get { return m_nativeAd; } }

        public override void initialize(string appId, string formatId)
        {
            base.initialize(appId, formatId);

            m_adLoader = new AdLoader.Builder(id).ForUnifiedNativeAd().Build();
            m_adLoader.OnUnifiedNativeAdLoaded += HandleUnifiedNativeAdLoaded;
            m_adLoader.OnAdFailedToLoad += HandleNativeAdFailedToLoad;

            request();
        }

        public override bool request()
        {
            if (Logx.isActive)
                Logx.trace("request {0}", name);

            if (!base.request())
                return false;

            m_adLoader.LoadAd(createAdRequest());
            return true;
        }

        private void HandleUnifiedNativeAdLoaded(object sender, UnifiedNativeAdEventArgs args)
        {
            if (Logx.isActive)
                Logx.trace("Unified Native ad loaded.");

            m_isUnifiedNativeAdLoaded = true;
            m_nativeAd = args.nativeAd;
        }

        private void HandleNativeAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            if (Logx.isActive)
                Logx.trace("Native ad failed to load: " + args.Message);

            clearRequest();
        }

        void Update()
        {
            if (m_isUnifiedNativeAdLoaded)
            {
                m_isUnifiedNativeAdLoaded = false;

                // 새로 갱신됐을 때 여기서 처리하면 된다.
            }
        }

        public override bool isLoaded()
        {
            return (null == m_nativeAd) ? false : true;
        }
        */
#endif
    }
}
