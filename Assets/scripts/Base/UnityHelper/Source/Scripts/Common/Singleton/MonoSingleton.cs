using UnityEngine;

namespace UnityHelper
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] bool m_isDontDestroyOnLoad = true;

        private static bool m_isDestroyed = false;
        private static T m_instance = null;

        public static T instance
        {
            get
            {
                if (m_instance == null)
                {
                    if (Logx.isActive)
                        Logx.error("m_instance is null");
                }

                return m_instance;
            }
        }

        public static U getInstance<U>() where U : MonoBehaviour
        {
            T t = instance;
            if (null == t)
                return null;

            return t as U;
        }

        protected virtual void Awake()
        {
            if (m_instance == null)
            {
                m_instance = this as T;

                if (Application.isPlaying)
                {
                    if (m_isDontDestroyOnLoad)
                        DontDestroyOnLoad(gameObject);
                }
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

        public static bool isDestroyed()
        {
            return m_isDestroyed;
        }

        public static bool isNullInstance()
        {
            return null == m_instance;
        }
    }
}