using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityHelper
{
    public class UIValue : MonoBehaviour
    {
        [SerializeField] Image m_icon = null;
        [SerializeField] protected TextSelector m_valueText = new TextSelector();
        [SerializeField] protected float m_animationTime = 0.3f;

        private Vector3 m_oriIconLocalScale = Vector3.zero;
        private Coroutine m_coIconScaleEffect = null;
        protected Coroutine m_coValueEffect = null;

        public Image icon => m_icon;
        protected GameObject valueText => m_valueText.gameObject;

        protected virtual void Awake()
        {
            if (null != m_icon)
                m_oriIconLocalScale = m_icon.transform.localScale;
        }

        public virtual void setValue(BigMoney value, bool isAnimation, Action<BigMoney> animationCallback = null)
        {

        }

        protected virtual float getTextAnimationSpeed(BigMoney curValue, BigMoney lastValue)
        {
            return 0.0f;
        }

        protected virtual void setValue(BigMoney value)
        {

        }

        public virtual void setValue(long value, bool isDigit, bool isAnimation, Action<long> animationCallback = null)
        {

        }

        protected virtual float getTextAnimationSpeed(long curValue, long lastValue)
        {
            return 0.0f;
        }

        protected virtual void setValue(long value, bool isDigit)
        {

        }

        protected virtual string valueToStr(long value, bool isDigit)
        {
            return (isDigit) ? StringHelper.toString(value, "N0") : StringHelper.toString(value);
        }

        public void setIconScaleEffect()
        {
            if (null != m_coIconScaleEffect)
            {
                StopCoroutine(m_coIconScaleEffect);
                m_coIconScaleEffect = null;
            }

            if (gameObject.activeSelf)
            {
                var changeType = CoroutineHelper.createSpeedType(10.0f);
                m_coIconScaleEffect = StartCoroutine(CoroutineHelper.instance.coPingPongValue(this, 1.0f, 1.1f, changeType, false, iconScaleEffectUpdateCallback,
                                                                                                                        iconScaleEffectEndCallback));
            }
        }

        private void iconScaleEffectUpdateCallback(float t, bool isEnd)
        {
            if (null != m_icon)
                m_icon.transform.localScale = m_oriIconLocalScale * t;
        }

        private void iconScaleEffectEndCallback()
        {
            if (null != m_icon)
                m_icon.transform.localScale = m_oriIconLocalScale;
        }

        public void setValueColor(Color color)
        {
            if (null == m_valueText)
                return;

            m_valueText.color = color;
        }
        
        protected void setCurrencyIcon(eCurrency type)
        {
            if (null == m_icon || type == eCurrency.None)
                return;

            m_icon.sprite = GameResourceHelper.getInstance().getSprite(type);                
        }
    }
}