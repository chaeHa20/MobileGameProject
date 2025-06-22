using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

namespace UnityHelper
{
    public class AndroidBackButton : MonoSingleton<AndroidBackButton>
    {
#if UNITY_ANDROID
        /// <summary>
        /// 연속 백 버튼 클릭 방지
        /// </summary>
        private bool m_ready = true;
        private Coroutine m_coReady = null;
        private bool m_isPurchasing = false;

        public bool isPurchasing { get => m_isPurchasing; set => m_isPurchasing = value; }

        void Update()
        {
            if (m_ready)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    if (!isActiveTutorial())
                    {
                        resetReady();
                        back();
                    }
                }
            }
        }

        IEnumerator coReady()
        {
            yield return new WaitForSecondsRealtime(0.5f);

            m_ready = true;
            m_coReady = null;
        }

        private void resetReady()
        {
            m_ready = false;

            if (null != m_coReady)
                StopCoroutine(m_coReady);

            m_coReady = StartCoroutine(coReady());
        }

        private void back()
        {
            if (m_isPurchasing)
                return;

            UIScene.instance().back();
        }

        protected virtual bool isActiveTutorial()
        {
            return false;
        }
#endif
    }
}