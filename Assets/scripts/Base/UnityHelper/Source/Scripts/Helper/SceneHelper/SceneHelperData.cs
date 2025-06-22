using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public struct SceneLoadConfig
    {
        public bool isCollectGC;
        public bool isUnloadUnUsedAssets;

        public static SceneLoadConfig Default()
        {
            SceneLoadConfig config = new SceneLoadConfig
            {
                isCollectGC = true,
                isUnloadUnUsedAssets = true
            };

            return config;
        }
    }

    public class SceneLoadData
    {
        public SceneLoadConfig config = SceneLoadConfig.Default();
        public string sceneName = "";
        public string additiveSceneName = "";
        public int defaultState = 0;
        public object[] defaultArgs = null;
    }
}