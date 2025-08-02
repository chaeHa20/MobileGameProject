using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityHelper
{
    public class UIGaugeImage : MonoBehaviour
    {
        public enum eType { FillAmount, SizeDelta, LocalPosition };

        [SerializeField] eType m_type = eType.SizeDelta;

        private Image m_image = null;
        private RectTransform m_rt = null;
        private Vector2 m_oriSizeDelta = Vector2.zero;

        public Color color { get { return m_image.color; } set { m_image.color = value; } }
        public Vector2 oriSizeDelta { get { return m_oriSizeDelta; } }
        public float fillAmount
        {
            get
            {
                if (null == m_image)
                    return 0.0f;

                return m_image.fillAmount;
            }
            set
            {
                if (null == m_image || null == m_rt)
                    return;

                float p = Mathf.Min(value, 1.0f);
                if (eType.FillAmount == m_type)
                {
                    m_image.fillAmount = p;
                }
                else if (eType.SizeDelta == m_type)
                {
                    m_rt.sizeDelta = new Vector2(m_oriSizeDelta.x * p, m_oriSizeDelta.y);
                }
                else if (eType.LocalPosition == m_type)
                {
                    var pos = new Vector3(p * m_oriSizeDelta.x, 0.0f);
                    m_rt.anchoredPosition = pos;
                }
            }
        }

        private void Awake()
        {
            m_image = GetComponent<Image>();
            if (Logx.isActive)
                Logx.assert(null != m_image, "m_image is null");

            m_image.fillAmount = 1.0f;
            m_rt = GetComponent<RectTransform>();
            if (eType.LocalPosition == m_type)
                m_rt.pivot = new Vector2(1.0f, 0.5f);
            else if (eType.SizeDelta == m_type)
                m_rt.pivot = new Vector2(0.0f, 0.5f);
            m_oriSizeDelta = m_rt.sizeDelta;
        }
    }
}