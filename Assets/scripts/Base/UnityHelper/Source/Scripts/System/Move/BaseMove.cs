using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public class BaseMove
    {
        protected float m_speed = 3.0f;
        protected GameObject m_moveObject = null;
        /// <summary>
        /// 일부 클래스에서는 호출 되지 않으므로 주의하자.
        /// </summary>
        protected Vector3 m_startPosition = Vector3.zero;
        protected Vector3 m_destPosition = Vector3.zero;
        protected Action m_arriveCallback = null;
        /// <summary>
        /// 일부 클래스에서는 호출 되지 않으므로 주의하자.
        /// </summary>
        protected Action<float, Vector3> m_moveCallback = null;
        private CoroutineHelper.Data m_coMove = new CoroutineHelper.Data();
        private MoveBoundary m_moveBoundary = null;

        public float speed { set { m_speed = value; } }
        public GameObject moveObject { set { m_moveObject = value; } }
        public Vector3 startPosition { set { m_startPosition = value; } }
        public Vector3 destPosition { set { m_destPosition = value; } }
        public Action arriveCallback { set { m_arriveCallback = value; } }
        public Action<float, Vector3> moveCallback { set { m_moveCallback = value; } }
        public MoveBoundary moveBoundary { set { m_moveBoundary = value; } }

        public virtual void move()
        {
            if (Logx.isActive)
                Logx.assert(null != m_moveObject, "m_moveObject is null");

            stop();

            //if (Logx.isActive)
            //    Logx.trace("start move");

            m_coMove.mono = null;
            m_coMove.enumerator = coMove();
            CoroutineHelper.instance.start(ref m_coMove);
        }

        public virtual void stop()
        {
            //if (Logx.isActive)
            //    Logx.trace("stop move");

            m_coMove.stop();
        }

        public virtual void move(MonoBehaviour mono)
        {
            if (Logx.isActive)
                Logx.assert(null != m_moveObject, "m_moveObject is null");

            stop();

            //if (Logx.isActive)
            //    Logx.trace("start move");

            m_coMove.mono = mono;
            m_coMove.enumerator = coMove();
            m_coMove.coroutine = mono.StartCoroutine(m_coMove.enumerator);
        }

        protected void checkBoundary(ref Vector3 position)
        {
            if (null == m_moveBoundary)
                return;

            m_moveBoundary.check(ref position);
        }

        protected bool isInBoundary(ref Vector3 position)
        {
            if (null == m_moveBoundary)
                return true;

            return m_moveBoundary.isIn(ref position);
        }

        protected MoveBoundary.eCollision isCollision(ref Vector3 position)
        {
            if (null == m_moveBoundary)
                return MoveBoundary.eCollision.None;

            return m_moveBoundary.isCollision(ref position);
        }

        protected virtual IEnumerator coMove()
        {
            yield return null;
        }
    }
}