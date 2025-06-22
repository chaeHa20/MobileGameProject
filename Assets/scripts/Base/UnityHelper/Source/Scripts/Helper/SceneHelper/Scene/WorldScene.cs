using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public class WorldScene : Scene
    {
        private static WorldScene s_instance = null;
        public static T instance<T>() where T : WorldScene
        {
            if (null == s_instance)
                return null;

            return s_instance as T;
        }

        public static WorldScene instance()
        {
            return s_instance;
        }

        protected override void Awake()
        {
            base.Awake();

            s_instance = this;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                PoolHelper.instance.Dispose();
                VibrationHelper.instance.Dispose();
                SoundHelper.instance.Dispose();
                CoroutineHelper.instance.Dispose();
                TimeHelper.Dispose();
                // ObjectMsgHelper.instance.Dispose();
            }
        }
    }
}