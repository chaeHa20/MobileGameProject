using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public class UIScene : Scene
    {
        [SerializeField] UICanvasGroup m_uiCanvasGroup = new UICanvasGroup();
        [SerializeField] GameObject m_block = null;

        private static UIScene s_instance = null;
        public static T instance<T>() where T : UIScene
        {
            if (null == s_instance)
                return null;

            return s_instance as T;
        }

        public static UIScene instance()
        {
            return s_instance;
        }

        protected override void Awake()
        {
            base.Awake();

            s_instance = this;

            if (!UIHelper.isNullInstance())
                UIHelper.instance.initCanvas(m_uiCanvasGroup);
        }

        protected override void initialize()
        {
            base.initialize();

            if(null != UIHelper.instance)
                UIHelper.instance.initScene();

            setActiveBlock(false, false);
        }

        public void setActiveBlock(bool isBlock, bool isApplyAndroidBackButton = true)
        {
            if (null == m_block)
                return;

            m_block.SetActive(isBlock);
        }

        public virtual void back()
        {

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                UIHelper.instance.Dispose();
            }
        }
    }
}