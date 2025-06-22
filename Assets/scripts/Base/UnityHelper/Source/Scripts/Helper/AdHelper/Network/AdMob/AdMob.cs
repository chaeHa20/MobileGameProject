using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public class AdMob : AdNetwork
    {
        protected override void initFormat(GameObject parent, BaseAdSettings.AdFormat format)
        {
            createFormat<AdMobBanner>(parent, eAdFormat.Banner, format.bannerId);
            createFormat<AdMobInterstitial>(parent, eAdFormat.Interstitial, format.interstitialId);
            createFormat<AdMobReward>(parent, eAdFormat.Reward, format.rewardId);
            createFormat<AdMobNative>(parent, eAdFormat.Native, format.nativeId);
        }
    }
}