using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

namespace UnityHelper
{
    public class UIToggleButtonGroup : MonoBehaviour
    {
        [Serializable]
        public class ToggleColor
        {
            public Color bg = Color.white;
            public Color text = Color.white;
        }

        [Serializable]
        public class ToggleButton
        {
            public Image bg = null;
            public Image icon = null;
            public TextSelector text = new TextSelector();

            public void setColor(ToggleColor color)
            {
                if (null != bg) bg.color = color.bg;
                if (null != icon) icon.color = color.text;
                if (null != text) text.color = color.text;
            }
        }

        [Serializable] class ButtonClickedEvent1 : UnityEvent<int> { }
        [Serializable] class ButtonClickedEvent2 : UnityEvent { }

        [SerializeField] ToggleColor m_onColor = new ToggleColor();
        [SerializeField] ToggleColor m_offColor = new ToggleColor();
        [SerializeField] List<ToggleButton> m_toggleButtons = new List<ToggleButton>();

        [SerializeField] ButtonClickedEvent1 m_onClick1 = new ButtonClickedEvent1();
        [SerializeField] ButtonClickedEvent2 m_onClick2 = new ButtonClickedEvent2();

        public virtual void initialize(int onIndex)
        {
            setOn(onIndex);
        }

        private void setOn(int onIndex)
        {
            m_toggleButtons.ForEach(x => x.setColor(m_offColor));
            m_toggleButtons[onIndex].setColor(m_onColor);
        }

        public void onClick(int index)
        {
            setOn(index);
            m_onClick1.Invoke(index);
            m_onClick2.Invoke();
        }
    }
}