using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityHelper;
using System;

/// <summary>
/// https://pastebin.com/2RX8fpJ3
/// </summary>
namespace UnityHelper
{
    //[RequireComponent(typeof(Camera))]
    public class Arcball : MonoBehaviour
    {
        public enum eRotation { Self, Parent };

        [Serializable]
        public class Option
        {
            public float rotationSensitivity = 4.0f;
            public float rotationDampening = 10.0f;
            public float zoomSensitvity = 2.0f;
            public float zoomDampening = 6.0f;
        }

        [Serializable]
        public class Freeze
        {
            public bool isVertical = false;
            public bool isHorizonal = false;
        }

        [SerializeField] eRotation m_rotationType = eRotation.Parent;
        [SerializeField] Option m_mouseOption = new Option();
        [SerializeField] Option m_touchOption = new Option();
        [SerializeField] float m_defaultCameraDistance = 2.0f;
        [SerializeField] fMinMax m_zoomBound = new fMinMax(1.5f, 3.0f);
        [SerializeField] fMinMax m_eulerBound = new fMinMax(-70.0f, 70.0f);
        [SerializeField] bool m_isEnableZoom = false;
        [SerializeField] Freeze m_freeze = new Freeze();

        private Vector3 m_euler = Vector3.zero;
        private float m_cameraDistance = 0.0f;
        private Vector3 m_lastMousePosition = new Vector3();
        private Vector3 m_mousePosition = new Vector3();
        private bool m_isDragging = false;
        private int m_touchFingerId = -1;
        private Option m_option = null;
        private float m_prevTouchDistance = 0.0f;
        private bool m_isPause = false;



        public bool isPause { get { return m_isPause; } set { m_isPause = value; } }

        private void Awake()
        {
#if UNITY_EDITOR
            m_option = m_mouseOption;
#else
            m_option = m_touchOption;
#endif
        }

        public void initialize()
        {
            m_cameraDistance = m_defaultCameraDistance;            
            transform.localPosition = new Vector3(0f, 0f, m_cameraDistance * -1f);

            if (eRotation.Parent == m_rotationType)
                transform.parent.rotation = Quaternion.Euler(m_euler.y, m_euler.x, 0);
        }

        void Update()
        {
            if (m_isPause)
                return;

            if (GraphicHelper.isPointerOverGameObject())
                return;

#if UNITY_EDITOR
            updateMouse();
#else
		    updateTouch();
#endif
        }

        private void updateMouse()
        {
            if (Input.GetMouseButtonDown(0))
            {
                m_lastMousePosition = Input.mousePosition;
                m_mousePosition = m_lastMousePosition;
                m_isDragging = true;
            }
            else if (Input.GetMouseButton(0))
            {
                if (!m_isDragging)
                {
                    m_lastMousePosition = Input.mousePosition;
                    m_isDragging = true;
                }

                m_mousePosition = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                m_isDragging = false;
            }

            float zoomOffset = Input.GetAxis("Mouse ScrollWheel");
            if (!MathHelper.isZero(zoomOffset))
            {
                float ScrollAmount = zoomOffset * m_option.zoomSensitvity;
                ScrollAmount *= (m_cameraDistance * 0.3f);
                m_cameraDistance += ScrollAmount * -1f;
                m_cameraDistance = m_zoomBound.clamp(m_cameraDistance);
            }
        }

        private void updateTouch()
        {
            if (1 == Input.touchCount)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    m_lastMousePosition = touch.position;
                    m_mousePosition = m_lastMousePosition;
                    m_touchFingerId = touch.fingerId;
                    m_isDragging = true;
                }
                else if (touch.fingerId == m_touchFingerId && touch.phase == TouchPhase.Moved)
                {
                    if (!m_isDragging)
                    {
                        m_lastMousePosition = touch.position;
                        m_isDragging = true;
                    }

                    m_mousePosition = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    m_isDragging = false;
                }
            }
            /**
             * zoom은 쫌 불안하다.
             * */
            else if (2 == Input.touchCount)
            {
                m_touchFingerId = -1;

                if (m_isEnableZoom)
                {
                    Touch t1 = Input.GetTouch(0);
                    Touch t2 = Input.GetTouch(1);

                    if (t1.phase == TouchPhase.Began || t2.phase == TouchPhase.Began)
                    {
                        m_prevTouchDistance = Vector2.Distance(t1.position, t2.position);
                    }

                    else if (t1.phase == TouchPhase.Moved || t2.phase == TouchPhase.Moved)
                    {
                        var curTouchDistance = Vector2.Distance(t1.position, t2.position);

                        float zoomOffset = curTouchDistance - m_prevTouchDistance;
                        float ScrollAmount = zoomOffset * m_option.zoomSensitvity;
                        ScrollAmount *= (m_cameraDistance * 0.3f);
                        m_cameraDistance += ScrollAmount * -1f;
                        m_cameraDistance = m_zoomBound.clamp(m_cameraDistance);

                        m_prevTouchDistance = curTouchDistance;
                    }
                }
            }
        }

        void LateUpdate()
        {
            if (m_isPause)
                return;

            if (m_isDragging)
            {
                float dx = (m_lastMousePosition.x - m_mousePosition.x) * Time.deltaTime;
                float dy = (m_lastMousePosition.y - m_mousePosition.y) * Time.deltaTime;

                if (!m_freeze.isHorizonal)
                {
                    if (eRotation.Parent == m_rotationType)
                        m_euler.x -= dx * m_option.rotationSensitivity;
                    else if (eRotation.Self == m_rotationType)
                        m_euler.x += dx * m_option.rotationSensitivity;
                }
                if (!m_freeze.isVertical)
                {
                    m_euler.y += dy * m_option.rotationSensitivity;
                    m_euler.y = m_eulerBound.clamp(m_euler.y);
                }

                m_lastMousePosition = m_mousePosition;
            }

            if (eRotation.Parent == m_rotationType)
            {
                var parent = transform.parent;
                Quaternion parentRotation = Quaternion.Euler(m_euler.y, m_euler.x, 0);
                parent.rotation = Quaternion.Slerp(parent.rotation, parentRotation, Time.deltaTime * m_option.rotationDampening);
            }
            else if (eRotation.Self == m_rotationType)
            {
                Quaternion rotation = Quaternion.Euler(m_euler.y, m_euler.x, 0);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * m_option.rotationDampening);
            }

            if (m_isEnableZoom)
            {
                transform.localPosition = new Vector3(0f, 0f, Mathf.Lerp(transform.localPosition.z, m_cameraDistance * -1f, Time.deltaTime * m_option.zoomDampening));
            }
        }

        protected void setRotation(Vector3 targetPosition)
        {
            Transform tr = null;
            if (eRotation.Parent == m_rotationType)
                tr = transform.parent;
            else if (eRotation.Self == m_rotationType)
                tr = transform;
            else
                return;

            var forward = tr.position - targetPosition;
            forward.Normalize();

            tr.forward = forward;
            setEuler(tr.rotation);
        }

        public void toRotation(Vector3 targetPosition)
        {
            Transform tr = null;
            if (eRotation.Parent == m_rotationType)
                tr = transform.parent;
            else if (eRotation.Self == m_rotationType)
                tr = transform;
            else
                return;

            var forward = tr.position - targetPosition;
            forward.Normalize();
            setEuler(Quaternion.LookRotation(forward));
        }

        private void setEuler(Quaternion quat)
        {
            m_euler = quat.eulerAngles;

            float temp = m_euler.x;
            m_euler.x = m_euler.y;
            m_euler.y = MathHelper.wrapAngle(temp); // bound check 때문에 바꿔줘야 된다.
            m_euler.y = m_eulerBound.clamp(m_euler.y);
        }
    }
}