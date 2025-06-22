using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public class AdAppLovinReward : AdReward
    {
#if _APPLOVIN_
        private int m_retryAttempt = 0;

        public override void initialize(string appId, string formatId, bool isTestDevice)
        {
            base.initialize(appId, formatId, isTestDevice);

            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

            if (Logx.isActive)
                Logx.trace("AppLovin initialize reward");

            request();
        }

        public override bool request()
        {
            if (Logx.isActive)
                Logx.trace("AppLovin reward request {0}", name);

            if (!base.request())
                return false;

            MaxSdk.LoadRewardedAd(id);

            return true;
        }

        public override bool show(Action<eAdResult> callback)
        {
            if (Logx.isActive)
                Logx.trace("AppLovin rewad show {0}", name);

            if (isLoaded())
            {
                base.show(callback);
                MaxSdk.ShowRewardedAd(id);
                return true;
            }
            else
            {
                if (Logx.isActive)
                    Logx.trace("AppLovin reward not ready show, {0}", name);

                return false;
            }
        }

        public override bool isLoaded()
        {
            return MaxSdk.IsRewardedAdReady(id);
        }

        private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {

            // Rewarded ad is ready to be shown. MaxSdk.IsRewardedAdReady(rewardedAdUnitId) will now return 'true'
            m_retryAttempt = 0;
        }

        private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            // Rewarded ad failed to load 
            // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds).

            if (Logx.isActive)
                Logx.error("AppLovin reward ad failed to open full screen content with error : " + errorInfo.Message);

            if (null != m_callback)
                m_callback(eAdResult.Fail);

            clearRequest();

            double retryDelay = Math.Pow(2, Math.Min(6, ++m_retryAttempt));
            Invoke("request", (float)retryDelay);
        }

        private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            // Rewarded ad failed to display. We recommend loading the next ad
            request();
        }

        private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
        }

        private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
        }

        private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Ad revenue paid. Use this callback to track user revenue.
        }

        private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Rewarded ad is hidden. Pre-load the next ad

            if (Logx.isActive)
                Logx.trace("AppLovin rewarded ad full screen content closed.");

            if (null != m_callback)
                m_callback(eAdResult.Closed);

            clearRequest();
            request();
        }

        private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
        {
            // Rewarded ad was displayed and user should receive the reward
        }
#endif
    }
}
