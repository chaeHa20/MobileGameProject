using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public abstract class NonMonoSingleton<T> where T : class, new()
    {
        // 상속받은 클래스의 생성자에서 설정 할 수 있도록 protected로 하자
        protected static T m_instance = null;

        public static T instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new T();
                }

                return m_instance;
            }
        }

        public static U getInstance<U>() where U : class
        {
            T t = instance;
            if (null == t)
                m_instance = new T();

            return t as U;
        }

        public static bool isNullInstance()
        {
            return null == m_instance;
        }
    }
}