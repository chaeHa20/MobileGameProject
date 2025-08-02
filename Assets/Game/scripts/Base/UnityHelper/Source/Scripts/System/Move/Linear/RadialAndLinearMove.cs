using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public class RadialAndLinearMove : LinearMove
    {
        private float m_radialLength = 1.0f;
        private Vector3 m_radialAxis = Vector3.up;
        private float m_radialSmoothTime = 1.0f;
        private float m_radialMoveEndWait = 0.5f;

        public float radialLength { set { m_radialLength = value; } }
        public Vector3 radialAxis { set { m_radialAxis = value; } }
        public float radialSmoothTime { set { m_radialSmoothTime = value; } }
        public float radialMoveEndWait { set { m_radialMoveEndWait = value; } }

        protected override IEnumerator coMove()
        {
            m_moveObject.transform.position = m_startPosition;

            /*
             * move radial
             * */
            Vector3 radialDestPosition = getRaidalDestPosition();
            bool isArrive = false;
            CoroutineHelper.instance.start(coSmoothDampMove(m_startPosition, radialDestPosition, m_radialSmoothTime, m_moveObject, () => { isArrive = true; }));
            while (!isArrive)
            {
                yield return null;
            }

            if (0.0f < m_radialMoveEndWait)
                yield return new WaitForSeconds(m_radialMoveEndWait);

            /*
             * linear move
             * */
            isArrive = false;
            var oldMovePosition = m_moveObject.transform.position;
            CoroutineHelper.instance.start(coLerpMove(radialDestPosition, m_destPosition, m_speed, m_moveObject, (t, isEnd) => 
            {
                if (isEnd)
                {
                    isArrive = true;
                }
                else
                {
                    m_moveCallback?.Invoke(t, oldMovePosition);
                    oldMovePosition = m_moveObject.transform.position;
                }
            }));
            while (!isArrive)
            {
                yield return null;
            }
            m_arriveCallback?.Invoke();
        }

        private Vector3 getRaidalDestPosition()
        {
            Vector3 cross = Vector3.Cross(m_radialAxis, Vector3.right);
            float angle = Random.Range(0.0f, 360.0f);
            Quaternion quat = Quaternion.AngleAxis(angle, m_radialAxis);
            Vector3 p = quat * cross;
            p *= m_radialLength;
            return p + m_startPosition;
        }
    }
}