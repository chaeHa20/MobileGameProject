using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if _ADMOB_
using GoogleMobileAds.Api;
#endif

namespace UnityHelper
{
    public class AdMobInterstitial : AdInterstitial
    {
#if _ADMOB_
        private InterstitialAd m_interstitial = null;

        public override void initialize(string appId, string formatId, bool isTestDevice)
        {
            base.initialize(appId, formatId, isTestDevice);

            request();

            if (Logx.isActive)
                Logx.trace("Admob logInitialize AdMobInterstitial success");
        }

        public override bool request()
        {
            if (Logx.isActive)
                Logx.trace("Admob logAdMobInterstitial request {0}", name);

            if (!base.request())
                return false;

            if (m_interstitial != null)
            {
                m_interstitial.Destroy();
                m_interstitial = null;
            }

            // create our request used to load the ad.
            var adRequest = new AdRequest();

            // send the request to load the ad.
            InterstitialAd.Load(id, adRequest, (ad, error) =>
            {
                // if error is not null, the load request failed.
                if (error != null)
                {
                    if (Logx.isActive)
                        Logx.error("Admob log interstitial ad failed to load an ad with error : " + error);
                    return;
                }

                if (null == ad)
                {
                    if (Logx.isActive)
                        Logx.error("Admob log interstitial ad failed to load an ad with error: ad is null");
                    return;
                }

                if (Logx.isActive)
                    Logx.trace("Admob logInterstitial ad loaded with response);// : " + ad.GetResponseInfo());

                m_interstitial = ad;

                RegisterEventHandlers(ad);
            });

            return true;
        }

        public override bool show(Action<eAdResult> callback)
        {
            if (Logx.isActive)
                Logx.trace("Admob logAdMobInterstitial show {0}", name);

            if (isLoaded())
            {
                base.show(callback);
                m_interstitial.Show();
                return true;
            }
            else
            {
                if (Logx.isActive)
                    Logx.trace("Admob logAdMobInterstitial Not ready show, {0}", name);

                return false;
            }
        }

        public override bool isLoaded()
        {
            if (null == m_interstitial)
                return false;

            return m_interstitial.CanShowAd();
        }

        private void RegisterEventHandlers(InterstitialAd interstitialAd)
        {
            // Raised when the ad is estimated to have earned money.
            interstitialAd.OnAdPaid += (AdValue adValue) =>
            {
                if (Logx.isActive)
                    Logx.trace("Admob logInterstitial ad paid {0} {1}.", adValue.Value, adValue.CurrencyCode);
            };
            // Raised when an impression is recorded for an ad.
            interstitialAd.OnAdImpressionRecorded += () =>
            {
                if (Logx.isActive)
                    Logx.trace("Admob logInterstitial ad recorded an impression.");
            };
            // Raised when a click is recorded for an ad.
            interstitialAd.OnAdClicked += () =>
        {
            if (Logx.isActive)
                    Logx.trace("Admob logInterstitial ad was clicked.");
            };
            // Raised when an ad opened full screen content.
            interstitialAd.OnAdFullScreenContentOpened += () =>
            {
                if (Logx.isActive)
                    Logx.trace("Admob logInterstitial ad full screen content opened.");
            };
            // Raised when the ad closed full screen content.
            interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            if (Logx.isActive)
                    Logx.trace("Admob logInterstitial ad full screen content closed.");

            if (null != m_callback)
                    m_callback(eAdResult.Closed);

            clearRequest();
                request();
            };
            // Raised when the ad failed to open full screen content.
            interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            if (Logx.isActive)
                    Logx.error("Admob log Interstitial ad failed to open full screen content with error : " + error);

            if (null != m_callback)
                    m_callback(eAdResult.Fail);

            clearRequest();
            request();
            };
        }
#endif
    }
}
