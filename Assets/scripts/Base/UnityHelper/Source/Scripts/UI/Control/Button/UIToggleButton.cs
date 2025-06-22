using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

namespace UnityHelper
{
    public class UIToggleButton : MonoBehaviour, IPointerClickHandler
    {
        [Serializable] class ButtonClickedEvent1 : UnityEvent<bool> { }
        [Serializable] class ButtonClickedEvent2 : UnityEvent { }

        [Serializable]
        private class Status
        {
            [Serializable]
            private class StatusGraphic
            {
                public Graphic graphic = null;
                public Color color = Color.white;
            }
            [SerializeField] List<StatusGraphic> graphics = new List<StatusGraphic>();

            public void setActive(bool isActive)
            {
                foreach (var graphic in graphics)
                {
                    graphic.graphic.gameObject.SetActive(isActive);
                    if (isActive)
                        graphic.graphic.color = graphic.color;
                }
            }
        }
        [SerializeField] Status m_on = new Status();
        [SerializeField] Status m_off = new Status();
        [SerializeField] ButtonClickedEvent1 m_onClick1 = new ButtonClickedEvent1();
        [SerializeField] ButtonClickedEvent2 m_onClick2 = new ButtonClickedEvent2();

        private bool m_isOn = false;

        public bool isOn { get { return m_isOn; } }

        public virtual void initialize(bool defaultOn)
        {
            setToggle(defaultOn);
        }

        protected virtual void toggle()
        {
            setToggle(!m_isOn);
        }

        protected virtual void setToggle(bool on)
        {
            m_isOn = on;

            m_on.setActive(false);
            m_off.setActive(false);

            if (on)
                m_on.setActive(true);
            else
                m_off.setActive(true);
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            toggle();
            m_onClick1.Invoke(m_isOn);
            m_onClick2.Invoke();
        }
    }
}