using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public abstract class LocalDataProtocol
    {
        private bool m_isSaveData;

        public bool isSaveData { get { return m_isSaveData; } set { m_isSaveData = value; } }

        public virtual void process(Req_LocalData _req, Action<Res_LocalData> callback)
        {
            if (Logx.isActive)
                Logx.error("Need override process");
        }

        public virtual void test()
        {

        }
    }
}