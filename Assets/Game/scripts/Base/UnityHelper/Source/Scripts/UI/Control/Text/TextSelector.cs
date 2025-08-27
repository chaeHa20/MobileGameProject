using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace UnityHelper
{
    [Serializable]
    public class TextSelector
    {
        [SerializeField] Text uiText = null;
        [SerializeField] TMP_Text tmpText = null;

        public string text
        {
            get
            {
                if (null != tmpText)
                    return tmpText.text;
                else if (null != uiText)
                    return uiText.text;
                else
                    return null;
            }

            set
            {
                if (null != tmpText)
                    tmpText.text = value;
                else if (null != uiText)
                    uiText.text = value;
            }
        }

        public GameObject gameObject
        {
            get
            {
                if (null != tmpText)
                    return tmpText.gameObject;
                else if (null != uiText)
                    return uiText.gameObject;
                else
                    return null;
            }
        }

        public Color color
        {
            get
            {
                if (null != tmpText)
                    return tmpText.color;
                else if (null != uiText)
                    return uiText.color;
                else
                    return Color.white;
            }

            set
            {
                if (null != tmpText)
                    tmpText.color = value;
                else if (null != uiText)
                    uiText.color = value;
            }
        }
    }
}