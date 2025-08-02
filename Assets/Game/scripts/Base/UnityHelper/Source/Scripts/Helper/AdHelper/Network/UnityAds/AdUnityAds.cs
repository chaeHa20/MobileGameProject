using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public class AdUnityAds : AdNetwork
    {
        protected override void initFormat(GameObject parent, BaseAdSettings.AdFormat format)
        {
            createFormat<AdUnityAdsInterstitial>(parent, eAdFormat.Interstitial, format.interstitialId);
            createFormat<AdUnityAdsReward>(parent, eAdFormat.Reward, format.rewardId);
        }
    }
}
