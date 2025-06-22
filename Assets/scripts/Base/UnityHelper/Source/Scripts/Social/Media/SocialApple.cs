using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_IOS && _SOCIAL_ 
using UnityEngine.SocialPlatforms.GameCenter;
#endif

namespace UnityHelper
{
    public class SocialApple : SocialUnity
    {
        public override void initialize()
        {
#if UNITY_IOS && _SOCIAL_
            if (Logx.isActive)
                Logx.trace("GameSocialApple config");

            GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
#endif
        }
    }
}