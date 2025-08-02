using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public class StateFunctionCase
    {
        public delegate void BeginAction(params object[] args);
        public delegate void UpdateAction(float dt);

        private BeginAction m_begin = null;
        private UpdateAction m_update = null;
        private Action m_end = null;

        public BeginAction begin { get { return m_begin; } }
        public UpdateAction update { get { return m_update; } }
        public Action end { get { return m_end; } }

        public StateFunctionCase(BeginAction _begin, UpdateAction _update = null, Action _end = null)
        {
            m_begin = _begin;
            m_update = _update;
            m_end = _end;
        }
    }
}