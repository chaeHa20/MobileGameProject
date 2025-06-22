using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public class LerpMove : LinearMove
    {
        private bool m_isRotation = false;
        private float m_rotationWeight = 1.0f;

        public bool isRotation { set { m_isRotation = value; } }
        public float rotationWeight { set { m_rotationWeight = value; } }

        protected override IEnumerator coMove()
        {
            var moveDir = Vector3.zero;
            if (m_isRotation)
            {
                moveDir = m_destPosition - m_startPosition;
                moveDir.Normalize();
            }

            var oldMovePosition = m_moveObject.transform.position;
            CoroutineHelper.instance.start(coLerpMove(m_startPosition, m_destPosition, m_speed, m_moveObject, (t, isEnd) =>
            {
                if (m_isRotation)
                    m_moveObject.transform.forward = Vector3.Lerp(m_moveObject.transform.forward, moveDir, Time.deltaTime * m_rotationWeight);

                if (isEnd)
                {
                    m_arriveCallback?.Invoke();
                }
                else
                {
                    m_moveCallback?.Invoke(t, oldMovePosition);
                    oldMovePosition = m_moveObject.transform.position;
                }
            }));

            yield return null;
        }
    }
}