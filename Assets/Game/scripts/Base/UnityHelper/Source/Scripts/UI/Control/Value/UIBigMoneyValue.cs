using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace UnityHelper
{
    public class UIBigValue : UIValue
    {
        [SerializeField] bool m_isShowOriginalValue = false;

        private BigMoney m_value = new BigMoney();

        public BigMoney value => m_value;

        public override void setValue(BigMoney value, bool isAnimation, Action<BigMoney> animationCallback = null)
        {
            if (null != m_coValueEffect)
            {
                StopCoroutine(m_coValueEffect);
                m_coValueEffect = null;
            }

            if (isAnimation && gameObject.activeInHierarchy)
            {
                var changeTime = getTextAnimationSpeed(m_value, value);
                m_coValueEffect = StartCoroutine(CoroutineHelper.instance.coChangeBigValue(m_value.value, value.value, changeTime, true, (v, isEnd) =>
                {
                    var _v = (isEnd) ? value.value : v;
                    var _money = new BigMoney(_v);
                    setValue(_money);

                    animationCallback?.Invoke(_money);
                }));
            }
            else
            {
                setValue(value);
            }
        }

        protected override float getTextAnimationSpeed(BigMoney curValue, BigMoney lastValue)
        {
            return m_animationTime;
        }

        protected override void setValue(BigMoney value)
        {
            m_value = value;
            m_valueText.text = m_isShowOriginalValue ? StringHelper.toString(m_value.value, "N0") : m_value.toString();
        }
    }
}