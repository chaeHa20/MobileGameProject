using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    /// <summary>
    /// 메인 쓰레드가 아닌 곳에서 사용
    /// </summary>
    public abstract class MonoSingletonNotNew<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T m_instance = null;
        private static bool m_isDestroyed = false;

        public static T instance
        {
            get
            {
                if (m_isDestroyed)
                {
                    if (Logx.isActive)
                        Logx.warn("Already singleton destroyed " + typeof(T));
                    return null;
                }

                return m_instance;
            }
        }

        protected virtual void Awake()
        {
            if (m_instance == null)
            {
                m_instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            m_isDestroyed = true;
        }

        protected virtual void OnApplicationQuit()
        {
            m_isDestroyed = true;
        }
    }
}