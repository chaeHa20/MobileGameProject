using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    /// <summary>
    /// object의 forward를 이용해서 이동
    /// </summary>
    public class ForwardMove : LinearMove
    {
        protected float m_rotationWeight = 1.0f;
        private float m_maxMoveTime = 0.0f;

        public float rotationWeight { set { m_rotationWeight = value; } }
        public float maxMoveTime { set { m_maxMoveTime = value; } }

        protected override IEnumerator coMove()
        {
            float speed = m_speed;
            getMoveDir(m_destPosition, out Vector3 moveDir, out _);

            float elapsedMoveTime = 0.0f;
            while (true)
            {
                elapsedMoveTime += Time.deltaTime;

                // Lerp로 하면 반대편 방향으로 가지 않는다.
                //Vector3 lastMoveDir = Vector3.Lerp(m_moveObject.transform.forward, moveDir, Time.deltaTime * m_rotationWeight);
                Vector3 lastMoveDir = Vector3.Slerp(m_moveObject.transform.forward, moveDir, Time.deltaTime * m_rotationWeight);

                var oldMovePosition = m_moveObject.transform.position;
                float s = speed * Time.deltaTime;
                Vector3 offset = lastMoveDir * s;
                Vector3 position = m_moveObject.transform.position + offset;
                checkBoundary(ref position);
                m_moveObject.transform.position = position;
                m_moveObject.transform.forward = lastMoveDir;

                yield return null;
                getMoveDir(m_destPosition, out moveDir, out float moveLen);

                if (!setDeceleration(moveLen, ref speed))
                    break;

                if (isArrive(moveLen))
                    break;

                if (isOverMaxMoveTime(elapsedMoveTime))
                    break;

                m_moveCallback?.Invoke(0.0f, oldMovePosition);
            }

            m_arriveCallback?.Invoke();
        }

        private bool isOverMaxMoveTime(float elapsedMoveTime)
        {
            if (0.0f >= m_maxMoveTime)
                return false;

            return elapsedMoveTime >= m_maxMoveTime;
        }
    }
}