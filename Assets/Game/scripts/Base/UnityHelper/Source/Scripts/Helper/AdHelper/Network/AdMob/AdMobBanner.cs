using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if _ADMOB_
using GoogleMobileAds.Api;
#endif

namespace UnityHelper
{
    public class AdMobBanner : AdBanner
    {
#if _ADMOB_
        private BannerView m_bannerView = null;

        public override bool request()
        {
            if (Logx.isActive)
                Logx.trace("AdMobBanner request {0}", name);

            if (!base.request())
                return false;

            if (m_bannerView != null)
            {
                m_bannerView.Destroy();
                m_bannerView = null;
            }

            m_bannerView = new BannerView(id, new AdSize(AdSize.FullWidth, 50), AdPosition.Bottom);

            m_bannerView.OnBannerAdLoaded += OnBannerAdLoaded;
            m_bannerView.OnBannerAdLoadFailed += OnBannerAdLoadFailed;
            //m_bannerView.OnAdOpening += HandleAdOpened;
            //m_bannerView.OnAdClosed += HandleAdClosed;
            //m_bannerView.OnAdLeavingApplication += HandleAdLeftApplication;

            m_bannerView.LoadAd(new AdRequest());//.Builder().Build());
            return true;
        }

        public override bool show(Action<eAdResult> callback)
        {
            if (Logx.isActive)
                Logx.trace("AdMobBanner show {0}", name);

            if (null == m_bannerView)
                return false;

            base.show(callback);

            m_bannerView.Show();

            if (null != callback)
                callback(eAdResult.Closed);

            return true;
        }

        public override void hide()
        {
            if (null == m_bannerView)
                return;

            m_bannerView.Hide();
        }

        public override bool isLoaded()
        {
            if (null == m_bannerView)
                return false;

            return true;
        }

        public void OnBannerAdLoaded()
        {
            if (Logx.isActive)
                Logx.trace("AdMobBanner OnBannerAdLoaded event received");
        }

        public void OnBannerAdLoadFailed(LoadAdError args)
        {
            if (Logx.isActive)
                Logx.trace("AdMobBanner OnBannerAdLoadFailed event received with message: " + args.GetMessage());

            clearRequest();
        }
#endif
    }
}
