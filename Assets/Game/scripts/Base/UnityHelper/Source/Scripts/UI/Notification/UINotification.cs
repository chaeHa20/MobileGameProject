using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

namespace UnityHelper
{
    public class UINotification<T> : UIComponent where T : Enum
    {
        [SerializeField] List<T> m_types = new List<T>();
        [SerializeField] GameObject m_icon = null;

        private object[] m_args = null;

        public List<T>.Enumerator e_types { get { return m_types.GetEnumerator(); } }

        protected virtual void Start()
        {
            if (UINotificationHelper<T>.isNullInstance())
                return;

            UINotificationHelper<T>.instance.add(this);

            setActive(false);
            refresh();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (UINotificationHelper<T>.isNullInstance())
                return;

            UINotificationHelper<T>.instance.remove(this);
        }

        private void OnEnable()
        {
            if (UINotificationHelper<T>.isNullInstance())
                return;

            if (!UINotificationHelper<T>.instance.isExist(this))
                return;

            refresh(m_args);
        }

        public virtual void refresh(params object[] args)
        {
            m_args = args;
        }

        public void setActive(bool isActive)
        {
            if (null == m_icon)
                return;
            
            m_icon.SetActive(isActive);
        }

        protected bool isActive()
        {
            if (null == m_icon)
                return false;

            return m_icon.activeSelf;
        }

        protected bool isNotificationType(T type)
        {
            if (null == m_icon)
                return false;

            return 0 <= m_types.IndexOf(type);
        }
    }
}