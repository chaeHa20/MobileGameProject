using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityHelper
{
    public class UIGrayScaleImage : MonoBehaviour
    {
        private Image m_image = null;

        private void Awake()
        {
            m_image = GetComponent<Image>();

            GraphicHelper.setGrayScaleMaterial(m_image, 0.0f);
        }

        public void setGrayScale(float value)
        {
            GraphicHelper.setGrayScaleValue(m_image, value);
        }

        public void setActive(bool isActive)
        {
            setGrayScale(isActive ? 0.0f : 1.0f);
        }
    }

}