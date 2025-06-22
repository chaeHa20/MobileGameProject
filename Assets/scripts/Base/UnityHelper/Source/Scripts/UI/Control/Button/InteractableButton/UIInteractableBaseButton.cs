using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace UnityHelper
{
    public class UIInteractableBaseButton : MonoBehaviour
    {
        [SerializeField] Button m_button = null;

        protected virtual void Awake()
        {
            checkButtonComponent();
        }

        public void setActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public virtual void setInteractable(bool isInteractable, bool isOnlyGraphicModify = false)
        {
            checkButtonComponent();

            if (!isOnlyGraphicModify)
                m_button.interactable = isInteractable;
        }

        public bool isInteractable()
        {
            return m_button.interactable;
        }

        protected void checkButtonComponent()
        {
            if (null != m_button)
                return;

            if (!gameObject.activeSelf)
                gameObject.SetActive(true);

            m_button = GetComponent<Button>();

            if (Logx.isActive)
                Logx.assert(null != m_button, "Failed UIInteractableButton, not find {0} button", name);
        }
    }
}