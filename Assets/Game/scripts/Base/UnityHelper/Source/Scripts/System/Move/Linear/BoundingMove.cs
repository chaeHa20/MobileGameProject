using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    /// <summary>
    /// 이동하다가 바운딩 영역에 충돌 했을 경웨 반사 벡터 방향으로 방향을 틀어서 이동하는 방식
    /// </summary>
    public class BoundingMove : LinearMove
    {
        protected float m_rotationWeight = 1.0f;

        public float rotationWeight { set { m_rotationWeight = value; } }

        protected override IEnumerator coMove()
        {
            float speed = m_speed;
            getMoveDir(m_destPosition, out Vector3 moveDir, out float moveLen);
            float totalMoveLen = moveLen;
            float elapsedTime = 0.0f;
            float totalMoveTime = moveLen / m_speed;
            bool isCollisioned = false;

            while (true)
            {
                Vector3 lastMoveDir = Vector3.Slerp(m_moveObject.transform.forward, moveDir, Time.deltaTime * m_rotationWeight);

                elapsedTime += Time.deltaTime;
                
                float s = speed * Time.deltaTime;
                Vector3 offset = lastMoveDir * s;
                Vector3 position = m_moveObject.transform.position + offset;
                checkBoundary(ref position);
                m_moveObject.transform.position = position;
                m_moveObject.transform.forward = lastMoveDir;

                if (!isCollisioned)
                {
                    var collision = isCollision(ref position);
                    if (MoveBoundary.eCollision.None != collision)
                    {
                        isCollisioned = true;

                        setReflectionDir(collision, ref moveDir);

                        // 바뀐 방향으로 목적지 위치를 다시 설정해 준다.
                        float movedLength = Vector3.Distance(position, m_startPosition);
                        float lastLength = totalMoveLen - movedLength;
                        if (0.0f >= lastLength)
                            break;

                        m_destPosition = m_moveObject.transform.position + moveDir * lastLength;
                    }
                }

                yield return null;

                getMoveDir(m_destPosition, out moveDir, out moveLen);

                if (!setDeceleration(moveLen, ref speed))
                    break;

                if (isArrive(moveLen))
                    break;

                if (elapsedTime >= totalMoveTime)
                    break;
            }

            m_arriveCallback?.Invoke();
        }

        private void setReflectionDir(MoveBoundary.eCollision collision, ref Vector3 dir)
        {
            if (MoveBoundary.eCollision.LX == collision)
            {
                if (0.0f > dir.x)
                    dir.x *= -1.0f;
            }
            else if (MoveBoundary.eCollision.RX == collision)
            {
                if (0.0f < dir.x)
                    dir.x *= -1.0f;
            }
            else if (MoveBoundary.eCollision.BY == collision)
            {
                if (0.0f > dir.z)
                    dir.z *= -1.0f;
            }
            else if (MoveBoundary.eCollision.TY == collision)
            {
                if (0.0f < dir.z)
                    dir.z *= -1.0f;
            }
        }
    }
}