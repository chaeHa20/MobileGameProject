using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public class SmoothRotationMove : LinearMove
    {
        private float m_smoothMoveTime = 1.0f;
        private float m_smoothRotationTime = 1.0f;
        private CoroutineHelper.Data m_smoothDampMoveData;
        private CoroutineHelper.Data m_slerpRotationData;

        public Quaternion startRotation { get; set; }
        public Quaternion destRotation { get; set; }
        public bool isRotation { get; set; }
        public float smoothMoveTime { get { return m_smoothMoveTime; } set { m_smoothMoveTime = value; } }
        public float smoothRotationTime { get { return m_smoothRotationTime; } set { m_smoothRotationTime = value; } }

        protected override IEnumerator coMove()
        {
            bool isMoveArrive = false;
            m_smoothDampMoveData = CoroutineHelper.instance.start(coSmoothDampMove(m_startPosition, m_destPosition, smoothMoveTime, m_moveObject, () => 
            {
                isMoveArrive = true;
            }));

            bool isRotationArrive = false;
            if (isRotation)
            {
                m_slerpRotationData = CoroutineHelper.instance.start(coSlerpRotation(startRotation, destRotation, smoothRotationTime, m_moveObject, () =>
                {
                    isRotationArrive = true;
                }));
            }
            else
            {
                isRotationArrive = true;
            }

            while (!isMoveArrive && !isRotationArrive)
            {
                yield return null;
            }

            m_arriveCallback?.Invoke();
        }

        public override void stop()
        {
            base.stop();

            CoroutineHelper.instance.stop(ref m_smoothDampMoveData);
            CoroutineHelper.instance.stop(ref m_slerpRotationData);
        }
    }
}