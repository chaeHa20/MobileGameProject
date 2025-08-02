using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityHelper
{
    public class UIStickController : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IDragHandler
    {
        [SerializeField] GameObject m_stick = null;

        private float m_radius = 0.0f;
        private bool m_isDown = false;
        private RectTransform m_rtCanvas = null;
        private Vector3 m_canvasPosition = Vector3.zero;
        private Vector3 m_downOffset = Vector3.zero;

        protected float radius => m_radius;

        void Awake()
        {
            setRadius();
        }

        void Start()
        {
            //m_rtCanvas = UIHelper.instance.mainCanvas.GetComponent<RectTransform>();
            m_rtCanvas = UIHelper.instance.canvasGroup.getLastCanvas().rtCanvas;
            m_canvasPosition = UIHelper.instance.worldToCanvasPosition(UIHelper.instance.uiCamera, m_rtCanvas, transform.position);
        }

        public virtual void initialize()
        {

        }

        void Update()
        {
#if UNITY_EDITOR
            Vector3 stickDir = Vector3.zero;
            float radius = 0.0f;
            if (Input.GetKey(KeyCode.A))
            {
                stickDir.x = -1.0f;
                radius = m_radius;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                stickDir.x = 1.0f;
                radius = m_radius;
            }

            if (Input.GetKey(KeyCode.S))
            {
                stickDir.y = -1.0f;
                radius = m_radius;
            }
            else if (Input.GetKey(KeyCode.W))
            {
                stickDir.y = 1.0f;
                radius = m_radius;
            }

            moveStick(stickDir, radius);
#else
            if (!m_isDown)
                restoreStick();
#endif
        }

        private void setRadius()
        {
            var sizeDelta = GetComponent<RectTransform>().sizeDelta;
            m_radius = sizeDelta.x * 0.5f;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Vector3 mouseCanvasPosition = UIHelper.instance.screenToCanvasPosition(m_rtCanvas, eventData.position);
            m_downOffset = mouseCanvasPosition - m_canvasPosition;

            m_isDown = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            m_isDown = false;
            m_stick.transform.localPosition = Vector3.zero;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (m_isDown)
                moveStick(eventData.position);
        }

        private void moveStick(Vector3 screenPosition)
        {
            Vector3 mouseCanvasPosition = UIHelper.instance.screenToCanvasPosition(m_rtCanvas, screenPosition);
            Vector3 localCanvasPosition = mouseCanvasPosition - m_canvasPosition;
            localCanvasPosition.z = 0.0f;

            float localRadius = localCanvasPosition.magnitude;
            if (localRadius > m_radius)
            {
                var localDir = localCanvasPosition.normalized;
                m_stick.transform.localPosition = localDir * m_radius;
            }
            else
            {
                m_stick.transform.localPosition = localCanvasPosition;
            }

            var stickDir =  mouseCanvasPosition - m_canvasPosition - m_downOffset;
            var stickRadius = stickDir.magnitude;
            stickDir.Normalize();
            moveStick(stickDir, stickRadius);
        }

        private void restoreStick()
        {
            moveStick(Vector3.zero, 0.0f);
        }

        protected virtual void moveStick(Vector3 stickDir, float stickRadius)
        {

        }
    }
}