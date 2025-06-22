using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace UnityHelper
{
    [Serializable]
    public class TextSelector
    {
        [SerializeField] Text uiText = null;

        public string text
        {
            get
            {
                if (null != uiText)
                    return uiText.text;                
                else
                    return null;
            }

            set
            {
                if (null != uiText)
                    uiText.text = value;
            }
        }

        public GameObject gameObject
        {
            get
            {
                if (null != uiText)
                    return uiText.gameObject;
                else
                    return null;
            }
        }

        public Color color
        {
            get
            {
                if (null != uiText)
                    return uiText.color;
                else
                    return Color.white;
            }

            set
            {
                if (null != uiText)
                    uiText.color = value;
            }
        }
    }
}