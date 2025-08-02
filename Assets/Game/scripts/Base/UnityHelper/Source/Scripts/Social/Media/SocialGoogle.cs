using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


#if UNITY_ANDROID && _SOCIAL_
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
#endif

namespace UnityHelper
{
    public class SocialGoogle : SocialUnity
    {

#if UNITY_ANDROID && _SOCIAL_
        public override void initialize()
        {
            if (Logx.isActive)
                Logx.trace("initialize GameSocialGoogle");

            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
        }
        

        protected override string getProfileUrl()
        {
            string userImageUrl = PlayGamesPlatform.Instance.GetUserImageUrl();
            if (Logx.isActive)
                Logx.trace("google play games user image url {0}", userImageUrl);

            return userImageUrl;
        }

        public override void logout()
        {

        }
#endif
    }
}