using System;
using System.Collections;
using UnityEngine;

namespace UnityHelper
{
    public class UIGauge : MonoBehaviour
    {
        public enum eValueDisplay { Percentage, CurrentMax }

        [SerializeField] float m_animationTime = 1.0f;
        [SerializeField] GameObject m_bg = null;
        [SerializeField] UIGaugeImage m_bar = null;
        [SerializeField] TextSelector m_value = new TextSelector();
        [SerializeField] eValueDisplay m_valueDisplay = eValueDisplay.Percentage;
        [SerializeField] bool m_isInverse = false;

        private float m_curValue = 0;
        private float m_maxValue = 1;
        private Func<float, float, float, string> m_customValueFunc = null;

        public float curValue { get { return m_curValue; } }
        public float maxValue { get { return m_maxValue; } }
        public bool isFull { get { return m_curValue >= m_maxValue; } }
        public float animationTime { get { return m_animationTime; } set { m_animationTime = value; } }
        public float fillAmount => (null == m_bar) ? m_bar.fillAmount : 0.0f;
        public Color barColor { set => m_bar.color = value; }
        public Func<float, float, float, string> customValueFunc { set { m_customValueFunc = value; } }

        [Obsolete("Use UIHud intead")]
        public void initialize(GameObject positionObject, float maxValue)
        {

        }

        public virtual void initialize(float maxValue)
        {
            setValue(maxValue, maxValue);
        }

        /// <summary>
        /// coLazyActive에서 m_content를 설정하기 때문에 여기서는 m_content를 쓰지 않는다. 흠,,
        /// </summary>
        /// <param name="isActive"></param>
        public void setActive(bool isActive)
        {
            if (null != m_bg)
                m_bg.SetActive(isActive);
            if (null != m_bar)
                m_bar.gameObject.SetActive(isActive);
            if (null != m_value)
                m_value.gameObject.SetActive(isActive);
        }

        private void setMaxValue(float maxValue)
        {
            if (Logx.isActive)
                Logx.assert(0.0f <= maxValue, "Invalid max value {0}", maxValue);

            m_maxValue = maxValue;
        }

        public void setValue(float value, float maxValue)
        {
            setMaxValue(maxValue);
            setValue(value);
        }

        protected float calcFillAmount(float value)
        {
            return (0.0f < m_maxValue) ? value / m_maxValue : 0.0f;
        }

        public void setValue(float value)
        {
            if (Logx.isActive)
                Logx.assert(0.0f <= value, "Invalid value {0}", value);

            m_curValue = value;

            float p = calcFillAmount(m_curValue);
            if (m_isInverse)
                p = 1.0f - p;

            if (null != m_bar)
            {
                m_bar.fillAmount = p;
            }

            if (null != m_value)
            {
                if (null != m_customValueFunc)
                {
                    m_value.text = m_customValueFunc(p, m_curValue, m_maxValue);
                }
                else
                {
                    switch (m_valueDisplay)
                    {
                        case eValueDisplay.Percentage: m_value.text = string.Format("{0}%", (int)(p * 100.0f)); break;
                        case eValueDisplay.CurrentMax: m_value.text = string.Format("{0}/{1}", (int)m_curValue, (int)m_maxValue); break;
                    }
                }
            }
        }

        public void setAnimation(float value, Action doneCallback = null, Action<float> updateCallback = null)
        {
            setAnimation(value, m_animationTime, doneCallback, updateCallback);
        }

        public void setAnimation(float value, float animationTime, Action doneCallback = null, Action<float> updateCallback = null)
        {
            StopAllCoroutines();
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(coAnimation(value, animationTime, doneCallback, updateCallback));
            }
            else
            {
                setValue(value);
                doneCallback?.Invoke();
            }
        }

        IEnumerator coAnimation(float value, float animationTime, Action doneCallback, Action<float> updateCallback)
        {
            float speed = Mathf.Abs(m_curValue - value) / animationTime;
            var changeType = CoroutineHelper.createSpeedType(speed);
            yield return StartCoroutine(CoroutineHelper.instance.coChangeValue(m_curValue, value, changeType, (v, done) =>
            {
                setValue(v);
                updateCallback?.Invoke(v);

                if (done)
                    doneCallback?.Invoke();
            }));
        }
    }
}