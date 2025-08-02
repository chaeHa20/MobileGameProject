using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public class UILongValue : UIValue
    {
        private long m_value = 0;

        public long value => m_value;

        public override void setValue(long value, bool isDigit, bool isAnimation, Action<long> animationCallback = null)
        {
            if (null != m_coValueEffect)
            {
                StopCoroutine(m_coValueEffect);
                m_coValueEffect = null;
            }

            if (isAnimation)
            {
                float speed = getTextAnimationSpeed(m_value, value);
                var changeType = CoroutineHelper.createSpeedType(speed);
                m_coValueEffect = StartCoroutine(CoroutineHelper.instance.coChangeValue((float)m_value, (float)value, changeType, (v, isEnd) =>
                {
                    var _v = (isEnd) ? value : (long)v;
                    setValue(_v, isDigit);

                    animationCallback?.Invoke(_v);
                }));
            }
            else
            {
                setValue(value, isDigit);
            }
        }

        protected override float getTextAnimationSpeed(long curValue, long lastValue)
        {
            return Mathf.Abs(lastValue - curValue) / m_animationTime;
        }

        protected override void setValue(long value, bool isDigit)
        {
            m_value = value;
            m_valueText.text = valueToStr(value, isDigit);
        }
    }
}