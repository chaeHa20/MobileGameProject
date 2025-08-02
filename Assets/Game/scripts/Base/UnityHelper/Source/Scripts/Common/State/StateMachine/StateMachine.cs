using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public class StateMachine
    {
        private struct CaseData
        {
            public int key;
            public object[] args;

            public CaseData(int _key, object[] _args)
            {
                key = _key;
                args = _args;
            }
        }

        private Dictionary<int, StateMachineCase> m_cases = new Dictionary<int, StateMachineCase>();
        private StateMachineCase m_curCase = null;
        private int m_curCaseKey = -1;
        private int m_oldCaseKey = -1;
        private object[] m_curArgs = null;
        private Stack<CaseData> m_caseStack = new Stack<CaseData>();

        public int curCaseKey => m_curCaseKey;
        public bool isEmptyCaseStack => 0 == m_caseStack.Count;

        public virtual void initialize()
        {

        }

        public bool isCurCase(int key)
        {
            return m_curCaseKey == key;
        }

        protected void addCase<T>(int key) where T : StateMachineCase, new()
        {
            if (existKey(key))
            {
                if (Logx.isActive)
                    Logx.error("Failed StateMachine addCase, {0} is duplicated", key);
                return;
            }

            T t = new T();
            t.initialize(this);
            m_cases.Add(key, t);
        }

        public void setCase(int key, params object[] args)
        {
            setCase(key, true, args);
        }

        private void setCase(int key, bool isPush, params object[] args)
        {
            if (!existKey(key))
            {
                if (Logx.isActive)
                    Logx.error("Error setCase, not exist key : {0}", key);
                return;
            }

            m_oldCaseKey = m_curCaseKey;

            endCurCase(key);

            if (isPush && 0 < m_curCaseKey)
                pushCaseStack(m_curCaseKey, m_curArgs);

            m_curCaseKey = key;
            m_curArgs = args;

            m_cases.TryGetValue(key, out m_curCase);
            m_curCase.begin(args);
        }

        private void endCurCase(int nextCase)
        {
            if (null == m_curCase)
                return;

            m_curCase.end(nextCase);
            m_curCase = null;
        }

        public virtual void update()
        {
            if (null == m_curCase)
                return;

            m_curCase.update();
        }

        public virtual void fixedUpdate()
        {
            if (null == m_curCase)
                return;

            m_curCase.fixedUpdate();
        }

        private bool existKey(int key)
        {
            return m_cases.ContainsKey(key);
        }

        private void pushCaseStack(int key, object[] args)
        {
            m_caseStack.Push(new CaseData(key, args));
        }

        public bool popCaseStack()
        {
            if (0 == m_caseStack.Count)
                return false;

            var caseStack = m_caseStack.Pop();
            setCase(caseStack.key, false, caseStack.args);
            return true;
        }

        public void clearCaseStack()
        {
            m_caseStack.Clear();
        }
    }
}