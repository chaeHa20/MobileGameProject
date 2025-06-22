using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public class BaseGameSettings : ScriptableObject
    {
        [Serializable]
        public class App
        {
            public string privacyUrl = "";
            public string facebookUrl = "";
            public string helpMailAddress = "";
            public string screenShotFileName = "Temp_ScreenShotName.png";
            public string galleryFileName = "ScreenShotName.png";
            public string icloudPlayedTime = "";
            public string iosRestoreAllData = "";
            public string cloudFileName;
        }

        [SerializeField] App m_app = new App();

        public App app { get { return m_app; } }
    }
}