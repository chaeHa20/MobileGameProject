using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public class Scene : Disposable
    {
        private StateMachine m_stateMachine = null;

        protected virtual void Awake()
        {
#if UNITY_EDITOR
            Application.targetFrameRate = 120;
#else
            Application.targetFrameRate = 60;
#endif
        }

        protected virtual void Start()
        {
            // 싱글톤이 먼저 초기화 되어야 되는 경우가 있기 때문에 Start에서 호출한다.
            initialize();
        }

        protected virtual void initialize()
        {

        }

        private void Update()
        {
            update();
        }

        private void FixedUpdate()
        {
            fixedUpdate();
        }

        protected virtual void update()
        {
            if (null != m_stateMachine)
                m_stateMachine.update();
        }

        protected virtual void fixedUpdate()
        {
            if (null != m_stateMachine)
                m_stateMachine.fixedUpdate();
        }

        protected void initStateMachine<T>(int defaultKey, params object[] defaultArgs) where T : StateMachine, new()
        {
            m_stateMachine = new T();
            m_stateMachine.initialize();
            setStateMachineCase(defaultKey, defaultArgs);
        }

        public void setStateMachineCase(int key, params object[] args)
        {
            if (null == m_stateMachine)
                return;

            m_stateMachine.setCase(key, args);
        }

        public bool isCurStateMachineCase(int key)
        {
            if (null == m_stateMachine)
                return false;

            return m_stateMachine.isCurCase(key);
        }

        public bool popCaseStack()
        {
            if (null == m_stateMachine)
                return false;

            return m_stateMachine.popCaseStack();
        }

        protected void clearCaseStack()
        {
            if (null == m_stateMachine)
                return;

            m_stateMachine.clearCaseStack();
        }

        public bool isEmptyCaseStack()
        {
            if (null == m_stateMachine)
                return true;

            return m_stateMachine.isEmptyCaseStack;
        }

        public int getCurStateMachineCase()
        {
            if (null == m_stateMachine)
                return -1;

            return m_stateMachine.curCaseKey;
        }
    }
}