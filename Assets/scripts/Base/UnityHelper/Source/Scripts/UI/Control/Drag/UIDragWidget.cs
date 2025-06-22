using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace UnityHelper
{
    public class UIDragWidget : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerUpHandler, IPointerDownHandler
    {
        [SerializeField] RectTransform m_parent = null;
        [SerializeField] RectTransform m_draggingPlane = null;

        private Vector2 m_diffDragPoint = Vector2.zero;
        private Vector2 m_parentHalfSize = Vector2.zero;
        private bool m_isDraging = false;
        private bool m_isFoldOut = true;
        private Action<bool> m_foldCallback = null;


        public void setDraggingPlane(GameObject draggingPlane)
        {
            if (Logx.isActive)
                Logx.assert(null != draggingPlane, "draggingPlane is null");

            m_draggingPlane = draggingPlane.GetComponent<RectTransform>();
        }

        /// <param name="callback">isFoldOut</param>
        public void setFoldCallback(Action<bool> callback, bool defaultValue = true)
        {
            m_isFoldOut = defaultValue;
            m_foldCallback = callback;
        }

        private void setParentHalfSize()
        {
            m_parentHalfSize.x = m_parent.rect.width * 0.5f;
            m_parentHalfSize.y = m_parent.rect.height * 0.5f;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            m_isDraging = true;

            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, m_parent.transform.position);
            m_diffDragPoint = eventData.position - screenPoint;
            setParentHalfSize();

            SetDraggedPosition(eventData);
        }

        public void OnDrag(PointerEventData data)
        {
            SetDraggedPosition(data);
        }

        private void SetDraggedPosition(PointerEventData data)
        {
            Vector2 dragPoint = data.position - m_diffDragPoint;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_draggingPlane, dragPoint, data.pressEventCamera, out Vector3 globalMousePos))
            {
                setBoundedPosition(data.pressEventCamera, ref globalMousePos);
                m_parent.position = globalMousePos;
            }
        }

        private void setBoundedPosition(Camera camera, ref Vector3 worldPosition)
        {
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(camera, worldPosition);

            bool isBounded = false;
            float left = screenPoint.x - m_parentHalfSize.x;
            float right = screenPoint.x + m_parentHalfSize.x;
            float top = screenPoint.y + m_parentHalfSize.y;
            float bottom = screenPoint.y - m_parentHalfSize.y;

            if (0.0f >= left)
            {
                screenPoint.x = m_parentHalfSize.x;
                RectTransformUtility.ScreenPointToWorldPointInRectangle(m_draggingPlane, screenPoint, camera, out worldPosition);
            }
            else if (Screen.width <= right)
            {
                screenPoint.x = Screen.width - m_parentHalfSize.x;
                isBounded = true;
            }

            if (0.0f >= bottom)
            {
                screenPoint.y = m_parentHalfSize.y;
                isBounded = true;

            }
            else if (Screen.height <= top)
            {
                screenPoint.y = Screen.height - m_parentHalfSize.y;
                isBounded = true;
            }

            if (isBounded)
                RectTransformUtility.ScreenPointToWorldPointInRectangle(m_draggingPlane, screenPoint, camera, out worldPosition);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            m_isDraging = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {

        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (m_isDraging)
                return;

            m_isFoldOut = !m_isFoldOut;

            m_foldCallback?.Invoke(m_isFoldOut);
        }
    }
}
