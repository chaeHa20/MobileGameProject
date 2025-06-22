using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public class SocialEditor : SocialUnity
    {
        public override bool isLoggedIn => true;

        public override void initialize()
        {
            base.initialize();

            setUser("Player", "Player", null);
        }

        public override void login(bool isDownloadProfile, Action<bool, string> callback)
        {
            if (Logx.isActive)
                Logx.trace("Social Authenticate");

            callback?.Invoke(true, "");
        }
    }
}