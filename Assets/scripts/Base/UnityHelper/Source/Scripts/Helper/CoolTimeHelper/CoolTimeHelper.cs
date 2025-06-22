using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public class CoolTimeHelper : MonoSingleton<CoolTimeHelper>
    {
        private Dictionary<long, CoolTimeListener> m_listeners = new Dictionary<long, CoolTimeListener>();

        public virtual void initialize()
        {
            clear();
        }

        public T addListener<T>(long id, long coolTime, int interval, bool isLoop) where T : CoolTimeListener
        {
            if (Logx.isActive)
                Logx.assert(0 < id, "Invalid cooltime listener id {0}", id);

            if (existListener(id))
                return findListener(id) as T;

            GameObject coolTimeObj = new GameObject
            {
                name = typeof(T).Name + id.ToString()
            };
            GraphicHelper.setParent(gameObject, coolTimeObj);

            T t = coolTimeObj.AddComponent<T>();
            t.initialize(id, coolTime, interval, isLoop);

            m_listeners.Add(id, t);

            return t;
        }

        public void delListener(long id)
        {
            if (m_listeners.TryGetValue(id, out CoolTimeListener listener))
            {
                Disposable.destroy(listener);
                m_listeners.Remove(id);
            }
        }

        public void startAll()
        {
            foreach (var pair in m_listeners)
            {
                start(pair.Key);
            }
        }

        public void start(long id)
        {
            CoolTimeListener listener = findListener(id);
            if (null == listener)
                return;

            listener.start();
        }

        public void restart(long id, long coolTime)
        {
            CoolTimeListener listener = findListener(id);
            if (null == listener)
                return;

            listener.restart(coolTime);
        }

        public void stop(long id)
        {
            CoolTimeListener listener = findListener(id);
            if (null == listener)
                return;

            listener.stop();
        }

        public void regist(CoolTimeListener.RegistCallback callback)
        {
            CoolTimeListener listener = findListener(callback.id);
            if (null == listener)
                return;

            listener.regist(callback);
        }

        public void unregist(CoolTimeListener.RegistCallback callback)
        {
            CoolTimeListener listener = findListener(callback.id);
            if (null == listener)
                return;

            listener.unregist(callback);
        }

        private CoolTimeListener findListener(long id)
            {
            if (m_listeners.TryGetValue(id, out CoolTimeListener listener))
                return listener;

            if (Logx.isActive)
                Logx.error("failed find cooltime listener id {0}", id);

            return null;
            }

        private bool existListener(long id)
        {
            return m_listeners.ContainsKey(id);
        }

        public long getCoolTime(long id)
        {
            CoolTimeListener listener = findListener(id);
            if (null == listener)
                return 0;

            return listener.coolTime;
        }

        public bool isCoolTime(long id)
        {
            long coolTime = getCoolTime(id);
            return TimeHelper.isCoolTime(coolTime);
        }

        private void clear()
        {
            foreach(var pair in m_listeners)
            {
                GameObject.Destroy(pair.Value.gameObject);
            }
            m_listeners.Clear();
        }
    }
}