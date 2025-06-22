using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if _ADMOB_
using GoogleMobileAds.Api;
#endif

namespace UnityHelper
{
    public class AdMobReward : AdReward
    {
#if _ADMOB_
        private RewardedAd m_reward = null;

        public override void initialize(string appId, string formatId, bool isTestDevice)
        {
            base.initialize(appId, formatId, isTestDevice);

            request();
        }

        public override bool request()
        {
            if (Logx.isActive)
                Logx.trace("Admob log AdMobReward request {0}", name);

            if (!base.request())
                return false;

            if (null != m_reward)
            {
                m_reward.Destroy();
                m_reward = null;
            }

            var adRequest = new AdRequest();
            RewardedAd.Load(id, adRequest, (ad, error) =>
            {
                // if error is not null, the load request failed.
                if (error != null)
                {
                    if (Logx.isActive)
                        Logx.error("Admob log Rewarded ad failed to load an ad with error : " + error);
                    return;
                }

                if (null == ad)
                {
                    if (Logx.isActive)
                        Logx.error("Admob log Rewarded ad failed to load an ad with error: ad is null");
                    return;
                }

                if (Logx.isActive)
                    Logx.trace("Admob log Rewarded ad loaded with response");// : " + ad.GetResponseInfo());

                m_reward = ad;

                RegisterEventHandlers(ad);
            });

            return true;
        }

        public override bool show(Action<eAdResult> callback)
        {
            if (Logx.isActive)
                Logx.trace("Admob log AdMobRewad show {0}", name);

            if (isLoaded())
            {
                base.show(callback);
                m_reward.Show((reward) =>
                {
                    if (Logx.isActive)
                        Logx.trace("Admob log Rewarded ad rewarded the user. Type: {0}, amount: {1}.", reward.Type, reward.Amount);
                });
                return true;
            }
            else
            {
                if (Logx.isActive)
                    Logx.trace("Admob log AdMobRewad Not ready show, {0}", name);

                return false;
            }
        }

        public override bool isLoaded()
        {
            if (null == m_reward)
                return false;

            return m_reward.CanShowAd();
        }


        private void RegisterEventHandlers(RewardedAd ad)
        {
            // Raised when the ad is estimated to have earned money.
            ad.OnAdPaid += (AdValue adValue) =>
        {
            if (Logx.isActive)
                    Logx.trace("Admob log Rewarded ad paid {0} {1}.", adValue.Value, adValue.CurrencyCode);

                var LTVParameters = new List<Firebase.Analytics.Parameter>();
                LTVParameters.Add(new Firebase.Analytics.Parameter("valuemicros", adValue.Value));
                LTVParameters.Add(new Firebase.Analytics.Parameter("currency", adValue.CurrencyCode));
                LTVParameters.Add(new Firebase.Analytics.Parameter("precision", (int)adValue.Precision));
                LTVParameters.Add(new Firebase.Analytics.Parameter("adunitid", id));
                LTVParameters.Add(new Firebase.Analytics.Parameter("network", m_reward.GetResponseInfo().GetMediationAdapterClassName()));
                Firebase.Analytics.FirebaseAnalytics.LogEvent("paid_ad_impression", LTVParameters.ToArray());
            };
            // Raised when an impression is recorded for an ad.
            ad.OnAdImpressionRecorded += () =>
        {
            if (Logx.isActive)
                    Logx.trace("Admob log Rewarded ad recorded an impression.");
            };
            // Raised when a click is recorded for an ad.
            ad.OnAdClicked += () =>
        {
            if (Logx.isActive)
                    Logx.trace("Admob log Rewarded ad was clicked.");
            };
            // Raised when an ad opened full screen content.
            ad.OnAdFullScreenContentOpened += () =>
        {
            if (Logx.isActive)
                    Logx.trace("Admob log Rewarded ad full screen content opened.");
            };
            // Raised when the ad closed full screen content.
            ad.OnAdFullScreenContentClosed += () =>
        {
            if (Logx.isActive)
                    Logx.trace("Admob log Rewarded ad full screen content closed.");

            if (null != m_callback)
                    m_callback(eAdResult.Closed);

            clearRequest();
                request();
            };
            // Raised when the ad failed to open full screen content.
            ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            if (Logx.isActive)
                    Logx.error("Admob log Rewarded ad failed to open full screen content with error : " + error);

            if (null != m_callback)
                    m_callback(eAdResult.Fail);

            clearRequest();
            request();
            };
        }
#endif
    }
}
