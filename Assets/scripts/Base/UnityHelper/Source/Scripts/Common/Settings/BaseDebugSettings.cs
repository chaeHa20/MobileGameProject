using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public class BaseDebugSettings : ScriptableObject
    {   
        [Serializable]
        public class Log
        {
            public Logx.eLevel level = Logx.eLevel.All;
            public string filter;
        }

        [Serializable]
        public class CheckValue
        {
            public bool isActive;
            public float value;

            public CheckValue(float _value)
            {
                value = _value;
            }
        }

        [SerializeField] bool m_isDebug = false;
        [SerializeField] bool m_isSkipTutorial = false;
        [SerializeField] Log m_log = new Log();
        [SerializeField] eLanguage m_language = eLanguage.Kr;

        public bool isDebug { get { return m_isDebug; } set { m_isDebug = value; } }
        public bool isSkipTutorial { get { return m_isSkipTutorial; } set { m_isSkipTutorial = value; } }
        public eLanguage language { get { return m_language; } }

        public virtual void apply()
        {
            Debug.LogFormat("Is Debug {0}", m_isDebug);

            Debugx.isActive = m_isDebug;

            if (m_isDebug)
            {
                Logx.level = m_log.level;
                Logx.filter = m_log.filter;
            }
            else
            {
                Logx.level = Logx.eLevel.Off;
            }
        }
    }
}