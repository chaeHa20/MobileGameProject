using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public abstract class LocalDataPacket
    {
    }

    public class Req_LocalData : LocalDataPacket
    {
        protected int m_pid = 0;
        protected eLocalData m_dataType;

        public int pid { get { return m_pid; } }
        public eLocalData dataType { get { return m_dataType; } }
    }

    public class Res_LocalData : LocalDataPacket
    {
        private int m_error = 0;
        private object[] m_errorArgs = null;

        public int error => m_error;
        public object[] errorArgs => m_errorArgs;
        public bool isSuccess => 0 == error;

        public static U createError<U>(int _error, params object[] args) where U : Res_LocalData, new()
        {
            U res = new U
            {
                m_error = _error,
                m_errorArgs = args,
            };
            return res;
        }
    }
}