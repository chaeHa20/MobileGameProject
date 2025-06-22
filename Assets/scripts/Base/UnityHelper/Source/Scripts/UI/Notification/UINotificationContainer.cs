using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public class UINotificationContainer<T> where T : Enum
    {
        private Dictionary<int, UINotification<T>> m_notifications = new Dictionary<int, UINotification<T>>();

        public void add(UINotification<T> noti)
        {
            if (isExist(noti))
                return;

            m_notifications.Add(noti.GetInstanceID(), noti);
        }

        public void remove(UINotification<T> noti)
        {
            if (!isExist(noti))
                return;

            m_notifications.Remove(noti.GetInstanceID());
        }

        public bool isExist(UINotification<T> noti)
        {
            return m_notifications.ContainsKey(noti.GetInstanceID());
        }

        public Dictionary<int, UINotification<T>>.Enumerator getEnumerator()
        {
            return m_notifications.GetEnumerator();
        }

        public void setActive(bool isActive)
        {
            var e = getEnumerator();
            while (e.MoveNext())
            {
                e.Current.Value.setActive(isActive);
            }
        }
    }
}