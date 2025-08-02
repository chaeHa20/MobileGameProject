using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public class PutInOnCamera : MonoBehaviour
    {
        [Flags]
        public enum eType
        {
            Width = 1 << 0,
            Height = 1 << 1,
            All = Width | Height,
        };

        [SerializeField] Camera m_onCamera = null;
        [SerializeField] GameObject m_target = null;
        [SerializeField] GameObject m_rotationObject = null;
        [SerializeField] eType m_type = eType.Width;
        [SerializeField] float m_fovScrollBound = 0.6f;
        [SerializeField] float m_moveAccel = 10.0f;

        private float m_boundVerticalFov = 0.0f;
        private float m_boundHorizontalFov = 0.0f;

        public GameObject target { set { m_target = value; } }
        public Camera onCamera { set { m_onCamera = value; } }
        public GameObject rotationObject { set { m_rotationObject = value; } }

        void Start()
        {
            if (Logx.isActive)
                Logx.assert(null != m_onCamera, "camera is null");

            float halfVerticalFov = m_onCamera.fieldOfView * 0.5f;
            m_boundVerticalFov = halfVerticalFov * m_fovScrollBound;

            float halfHorizontalFov = GraphicHelper.getHorizontalFov(m_onCamera) * 0.5f;
            m_boundHorizontalFov = halfHorizontalFov * m_fovScrollBound;
        }

        void Update()
        {
            if (null == m_target)
                return;

            Vector3 cameraForward = m_rotationObject.transform.forward;
            Vector3 cameraToTargetDir = m_target.transform.position - m_rotationObject.transform.position;
            cameraToTargetDir.Normalize();

            Vector3 cameraEuler = m_rotationObject.transform.localRotation.eulerAngles;

            if (0 != (eType.Width & m_type))
                putInOnWidth(cameraForward, cameraToTargetDir, ref cameraEuler);
            if (0 != (eType.Height & m_type))
                putInOnHeight(cameraForward, cameraToTargetDir, ref cameraEuler);

            m_rotationObject.transform.localRotation = Quaternion.Euler(cameraEuler);
        }

        private void putInOnWidth(Vector3 cameraForward, Vector3 cameraToTargetDir, ref Vector3 cameraEuler)
        {
            cameraForward.y = 0.0f;
            cameraToTargetDir.y = 0.0f;
            float cameraToTargetAngle = Vector3.Angle(cameraForward, cameraToTargetDir);

            float diffAngle = cameraToTargetAngle - m_boundHorizontalFov;
            if (0.0f < diffAngle)
            {
                Vector3 cross = Vector3.Cross(cameraToTargetDir, cameraForward);

                // 왼쪽
                if (0.0f < cross.y)
                {
                    cameraEuler.y -= diffAngle * Time.smoothDeltaTime * m_moveAccel;
                }
                // 오른쪽
                else
                {
                    cameraEuler.y += diffAngle * Time.smoothDeltaTime * m_moveAccel;
                }
            }
        }

        private void putInOnHeight(Vector3 cameraForward, Vector3 cameraToTargetDir, ref Vector3 cameraEuler)
        {
            cameraForward.x = 0.0f;
            cameraToTargetDir.x = 0.0f;
            float cameraToTargetAngle = Vector3.Angle(cameraForward, cameraToTargetDir);

            float diffAngle = cameraToTargetAngle - m_boundVerticalFov;
            if (0.0f < diffAngle)
            {
                Vector3 cross = Vector3.Cross(cameraToTargetDir, cameraForward);

                // 위쪽
                if (0.0f < cross.x)
                {
                    cameraEuler.x -= diffAngle * Time.smoothDeltaTime * m_moveAccel;
                }
                // 아래쪽
                else
                {
                    cameraEuler.x += diffAngle * Time.smoothDeltaTime * m_moveAccel;
                }
            }
        }
    }
}