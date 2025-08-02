using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public class AdAppLovinInterstitial : AdInterstitial
    {
#if _APPLOVIN_
        private int m_retryAttempt = 0;

        public override void initialize(string appId, string formatId, bool isTestDevice)
        {
            base.initialize(appId, formatId, isTestDevice);

            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
            MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;

            if (Logx.isActive)
                Logx.trace("AppLovin initialize interstitial");

            request();
        }

        public override bool request()
        {
            if (Logx.isActive)
                Logx.trace("AppLovin interstitial request {0}", name);

            if (!base.request())
                return false;

            MaxSdk.LoadInterstitial(id);

            return true;
        }

        public override bool show(Action<eAdResult> callback)
        {
            if (Logx.isActive)
                Logx.trace("AppLovin interstitial show {0}", name);

            if (isLoaded())
            {
                base.show(callback);
                MaxSdk.ShowInterstitial(id);
                return true;
            }
            else
            {
                if (Logx.isActive)
                    Logx.trace("AppLovin interstitial not ready show, {0}", name);

                return false;
            }
        }

        public override bool isLoaded()
        {
            return MaxSdk.IsInterstitialReady(id);
        }

        private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            m_retryAttempt = 0;
        }

        private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            if (Logx.isActive)
                Logx.error("AppLovin interstitial ad failed to open full screen content with error : " + errorInfo.Message);

            if (null != m_callback)
                m_callback(eAdResult.Fail);

            clearRequest();

            double retryDelay = Math.Pow(2, Math.Min(6, ++m_retryAttempt));
            Invoke("request", (float)retryDelay);
        }

        private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {

        }

        private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {

        }

        private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            // Interstitial ad failed to display. AppLovin recommends that you load the next ad.

            clearRequest();
            request();
        }

        private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Interstitial ad is hidden. Pre-load the next ad.
            if (Logx.isActive)
                Logx.trace("Admob logInterstitial ad full screen content closed.");

            if (null != m_callback)
                m_callback(eAdResult.Closed);

            clearRequest();
            request();
        }
#endif
    }
}
