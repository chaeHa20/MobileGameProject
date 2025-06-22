using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public class UINotificationHelper<T> : MonoSingleton<UINotificationHelper<T>>, IDisposable where T : Enum
    {
        /// <summary>
        /// <notification type, Data>
        /// </summary>
        private Dictionary<T, UINotificationContainer<T>> m_containers = new Dictionary<T, UINotificationContainer<T>>();

        public void add(UINotification<T> noti)
        {
            if (Logx.isActive)
            {
                Logx.assert(null != noti, "noti is null");
                //Logx.trace("--------------------> add Notification {0}", noti.GetInstanceID());
            }

            var e_types = noti.e_types;
            while (e_types.MoveNext())
            {
                if (!m_containers.TryGetValue(e_types.Current, out UINotificationContainer<T> container))
                {
                    container = new UINotificationContainer<T>();
                    m_containers.Add(e_types.Current, container);
                }

                container.add(noti);
            }
        }

        public void setActive(T type, bool isActive)
        {
            if (Logx.isActive)
                Logx.trace("nnnnnnnnnnnnnnnnnnnnnnn> setActive Notification {0} {1}", type, isActive);

            if (!m_containers.TryGetValue(type, out UINotificationContainer<T> container))
            {
                container = new UINotificationContainer<T>();
                m_containers.Add(type, container);
            }

            container.setActive(isActive);
        }

        public Dictionary<int, UINotification<T>>.Enumerator getEnumerator(T type)
        {
            if (m_containers.TryGetValue(type, out UINotificationContainer<T> container))
                return container.getEnumerator();

            return new Dictionary<int, UINotification<T>>().GetEnumerator();
        }

        public void remove(UINotification<T> noti)
        {
            if (Logx.isActive)
            {
                Logx.assert(null != noti, "noti is null");
                //Logx.trace("--------------------> remove Notification {0}", noti.GetInstanceID());
            }

            var e_types = noti.e_types;
            while (e_types.MoveNext())
            {
                if (m_containers.TryGetValue(e_types.Current, out UINotificationContainer<T> container))
                {
                    container.remove(noti);
                }
            }
        }

        public bool isExist(UINotification<T> noti)
        {
            var e_types = noti.e_types;
            while (e_types.MoveNext())
            {
                if (m_containers.TryGetValue(e_types.Current, out UINotificationContainer<T> container))
                {
                    // 하나만 체크하자, 하나만 있어도 있는 거니까
                    return container.isExist(noti);
                }
            }

            return false;
        }

        public void refresh(List<T>.Enumerator e_notiTypes, params object[] args)
        {
            HashSet<int> duplicateChecker = new HashSet<int>();

            while (e_notiTypes.MoveNext())
            {
                if (m_containers.TryGetValue(e_notiTypes.Current, out UINotificationContainer<T> container))
                {
                    var e = container.getEnumerator();
                    while (e.MoveNext())
                    {
                        UINotification<T> noti = e.Current.Value;
                        if (duplicateChecker.Contains(noti.GetInstanceID()))
                            continue;

                        duplicateChecker.Add(noti.GetInstanceID());

                        if (noti.gameObject.activeInHierarchy)
                            noti.refresh(args);
                    }
                }
            }
        }

        public void refresh(T notiType, params object[] args)
        {
            HashSet<int> duplicateChecker = new HashSet<int>();

            if (m_containers.TryGetValue(notiType, out UINotificationContainer<T> container))
            {
                var e = container.getEnumerator();
                while (e.MoveNext())
                {
                    UINotification<T> noti = e.Current.Value;
                    if (!noti.gameObject.activeInHierarchy)
                        continue;

                    if (duplicateChecker.Contains(noti.GetInstanceID()))
                        continue;

                    duplicateChecker.Add(noti.GetInstanceID());

                    if (noti.gameObject.activeInHierarchy)
                        noti.refresh(args);
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_containers.Clear();
            }
        }
    }
}