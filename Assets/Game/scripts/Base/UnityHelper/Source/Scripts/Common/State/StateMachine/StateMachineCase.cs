using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public class StateMachineCase
    {
        private StateMachine m_stateMachine = null;

        public virtual void initialize(StateMachine stateMachine)
        {
            if (Logx.isActive)
                Logx.assert(null != stateMachine, "Invalid StateMachineCase.initialize() parameter, stateMachine is null");

            m_stateMachine = stateMachine;
        }

        public virtual void begin(params object[] args)
        {

        }

        public virtual void end(int nextCase)
        {

        }

        public virtual void update()
        {

        }

        public virtual void fixedUpdate()
        {

        }

        protected void setCase(int key, params object[] args)
        {
            m_stateMachine.setCase(key, args);
        }
    }
}