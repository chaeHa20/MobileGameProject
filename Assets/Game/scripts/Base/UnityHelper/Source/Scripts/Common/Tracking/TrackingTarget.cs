using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public class TrackingTarget : MonoBehaviour
    {
        [SerializeField] GameObject m_target = null;
        [SerializeField] float m_trackingSpeed = 1.0f;

        private Vector3 m_targetOffset = Vector3.zero;
        private Quaternion m_oriRotation = Quaternion.identity;
        private Vector3 m_oriPosition = Vector3.zero;
        private bool m_isStop = false;

        void Awake()
        {
            m_oriRotation = transform.rotation;
            m_oriPosition = transform.position;

            setTargetOffset();
        }

        public void setTarget(GameObject target)
        {
            m_target = target;

            transform.position = m_oriPosition;
            setTargetOffset();
        }

        private void setTargetOffset()
        {
            if (null != m_target)
                m_targetOffset = transform.position - m_target.transform.position;
        }

        private void Update()
        {
            if (m_isStop)
                return;

            if (null != m_target)
            {
                tracking();
            }
        }

        private void tracking()
        {
                Vector3 oldTargetPosition = transform.position - m_targetOffset;
                oldTargetPosition = Vector3.Lerp(oldTargetPosition, m_target.transform.position, Time.smoothDeltaTime * m_trackingSpeed);
                transform.position = oldTargetPosition + m_targetOffset;
            }

        public void stop()
        {
            m_isStop = true;
        }

        public void resume(bool isSetTargetOffset)
        {
            if (isSetTargetOffset)
                setTargetOffset();

            m_isStop = false;
            transform.position = m_target.transform.position + m_targetOffset;
            transform.rotation = m_oriRotation;
        }
    }
}