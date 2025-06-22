using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public class StateFunction : IDisposable
    {
        private Dictionary<int, StateFunctionCase> m_cases = new Dictionary<int, StateFunctionCase>();
        private StateFunctionCase m_curCase = null;
        private int m_curType = -1;

        public int curType => m_curType;
        public bool isEmpty => 0 == m_cases.Count;

        public virtual void initialize()
        {
            m_curType = -1;
        }

        public void addCase(int type, StateFunctionCase stateFunctionCase)
        {
            if (m_cases.ContainsKey(type))
            {
                if (Logx.isActive)
                    Logx.error("Failed addCase(), {0} type is already exist", type);
                return;
            }

            m_cases.Add(type, stateFunctionCase);
        }

        public void setCase(int type, params object[] args)
        {
            if (Logx.isActive)
                Logx.assert(0 <= type);

            //if (Logx.isActive)
            //    Logx.trace("{0} state machine {1}", GetType().Name, type);

            if (null != m_curCase)
            {
                if (null != m_curCase.end)
                    m_curCase.end();
                m_curCase = null;
            }

            m_curType = type;

            if (!m_cases.TryGetValue(type, out m_curCase))
            {
                if (Logx.isActive)
                    Logx.error("Failed setCase(), {0} type is not found", type);
                return;
            }

            if (null != m_curCase.begin)
                m_curCase.begin(args);
        }

        public virtual void update(float dt)
        {
            if (null == m_curCase)
                return;

            if (null != m_curCase.update)
                m_curCase.update(dt);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_cases.Clear();
                m_curCase = null;
                m_curType = -1;
            }
        }
    }
}