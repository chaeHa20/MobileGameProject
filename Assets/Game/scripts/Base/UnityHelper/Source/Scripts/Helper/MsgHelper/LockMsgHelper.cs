using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

/*
 *  여기서 쓰레드 관련 함수를 호출하면 안된다.
 * */
namespace UnityHelper
{
    public class LockMsgHelper<T> : MonoSingleton<T> where T : MonoBehaviour
    {
        private int m_oneTimeProcessCount = 1;
        private LinkedList<Msg> m_msgs = new LinkedList<Msg>();
        private System.Object lockThis = new System.Object();

        protected int msgCount { get { return m_msgs.Count; } }

        public void initialize(int oneTimeProcessCount)
        {
            m_oneTimeProcessCount = oneTimeProcessCount;
        }

        public virtual void add(Msg msg)
        {
            if (Logx.isActive)
                Logx.assert(null != msg);

            lock (lockThis)
            {
                m_msgs.AddLast(msg);
            }
        }

        void LateUpdate()
        {
            if (0 < m_msgs.Count)
            {
                try
                {
                    lock (lockThis)
                    {
                        for (int i = 0; i < m_oneTimeProcessCount; ++i)
                        {
                            if (0 == m_msgs.Count)
                                break;

                            Msg msg = m_msgs.First.Value;
                            msg.action();

                            m_msgs.RemoveFirst();
                        }

                        // 3은 여유분이다.
                        if (m_oneTimeProcessCount * 3 <= m_msgs.Count)
                        {
                            if (Logx.isActive)
                                Logx.warn("Over unity msg count {0}, oneTimeProcessCount {1}", m_msgs.Count, m_oneTimeProcessCount);
                        }
                    }
                }
                catch (Exception e)
                {
                    if (Logx.isActive)
                        Logx.exception(e);
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                StopAllCoroutines();
                m_msgs.Clear();
            }
        }
    }
}