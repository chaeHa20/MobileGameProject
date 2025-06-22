using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace UnityHelper
{
    public class UIToastMsg : PoolObject
    {
        [SerializeField] TextSelector m_msg = new TextSelector();
        [SerializeField] float m_animationSpeed = 1.0f;
        [SerializeField] float m_middleDelay = 1.0f;

        private CanvasGroup m_canvasGroup = null;

        public static void create<T>(GameObject parent, string poolName, string msg, Action<T> callback) where T : UIToastMsg
        {
            PoolHelper.instance.pop<T>(poolName, (t) =>
            {
                if (null != t)
                {
                    t.setMsg(msg);

                    UIHelper.instance.setParent(parent, t.gameObject, SetParentOption.notFullAndReset());
                }

                callback?.Invoke(t);
            });
        }

        void Awake()
        {
            m_canvasGroup = GetComponent<CanvasGroup>();
            if (Logx.isActive)
                Logx.assert(null != m_canvasGroup, "m_canvasGroup is null");
        }

        private void setMsg(string msg)
        {
            m_msg.text = msg;
            StartCoroutine(coMsg());
        }

        IEnumerator coMsg()
        {
            m_canvasGroup.alpha = 0.0f;
            var changeType = CoroutineHelper.createSpeedType(m_animationSpeed);

            yield return StartCoroutine(CoroutineHelper.instance.coChangeValue(m_canvasGroup.alpha, 1.0f, changeType, (value, done) =>
            {
                m_canvasGroup.alpha = value;
            }));

            yield return new WaitForSeconds(m_middleDelay);

            yield return StartCoroutine(CoroutineHelper.instance.coChangeValue(m_canvasGroup.alpha, 0.0f, changeType, (value, done) =>
            {
                m_canvasGroup.alpha = value;
            }));

            Dispose();
        }
    }
}