using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public class AdAppLovin : AdNetwork
    {
        protected override void initFormat(GameObject parent, BaseAdSettings.AdFormat format)
        {
            createFormat<AdAppLovinReward>(parent, eAdFormat.Reward, format.rewardId);
        }
    }
}
