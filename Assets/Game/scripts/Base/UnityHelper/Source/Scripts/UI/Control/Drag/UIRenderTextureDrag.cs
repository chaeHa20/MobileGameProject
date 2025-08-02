using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityHelper;
using System;

namespace UnityHelper
{
    public class UIRenderTextureDrag : Disposable
    {
        [SerializeField] RenderTexture m_renderTexture = null;
        [SerializeField] Vector2 m_dragSpeed = new Vector2(2.0f, 2.0f);

        private bool m_isDragging = false;
        private ObjectTextureRenderer m_textureRenderer = null;
        private Coroutine m_coRotation = null;
        private float m_rotationY = 0.0f;

        protected RenderTexture renderTexture => m_renderTexture;
        protected float orthographicSize => m_textureRenderer.orthographicSize;

        public virtual void initialize(int textureRendererResourceId)
        {
            m_textureRenderer = loadObjectTextureRenderer(textureRendererResourceId);
        }

        protected virtual ObjectTextureRenderer loadObjectTextureRenderer(int textureRendererResourceId)
        {
            return null;
        }

        protected void setRenderTextureObject(GameObject obj)
        {
            m_textureRenderer.setObject(obj);
        }

        public void onBeginDrag(BaseEventData eventData)
        {
            m_isDragging = true;
        }

        public void onDrag(BaseEventData eventData)
        {
            var ed = eventData as PointerEventData;
            drag(ed.delta.x * m_dragSpeed.x, ed.delta.y * m_dragSpeed.y);
        }

        public void onEndDrag(BaseEventData eventData)
        {
            m_isDragging = false;
        }

        protected void startRotation(GameObject target, float speed)
        {
            stopRotation();

            m_coRotation = StartCoroutine(coRotation(target, speed));
        }

        private void stopRotation()
        {
            if (null == m_coRotation)
                return;

            StopCoroutine(m_coRotation);
            m_coRotation = null;
        }

        IEnumerator coRotation(GameObject target, float speed)
        {
            while (true)
            {
                if (null == target)
                    break;

                if (!m_isDragging)
                {
                    m_rotationY -= TimeHelper.deltaTime * speed;
                }

                if (0.0f >= m_rotationY)
                    m_rotationY += 360.0f;

                target.transform.localRotation = Quaternion.Euler(0.0f, m_rotationY, 0.0f);
                yield return null;
            }
        }

        protected virtual void drag(float deltaX, float deltaY)
        {
            m_rotationY -= deltaX;
        }

        private void destroyTextureRenderer()
        {
            if (null != m_textureRenderer)
                GameObject.Destroy(m_textureRenderer.gameObject);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                stopRotation();
                destroyTextureRenderer();
            }
        }
    }
}