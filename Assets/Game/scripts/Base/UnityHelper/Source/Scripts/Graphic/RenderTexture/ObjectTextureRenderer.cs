using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public class ObjectTextureRenderer : MonoBehaviour
    {
        [SerializeField] Camera m_camera = null;
        [SerializeField] GameObject m_objectPosition = null;

        private RenderTexture m_renderTexture = null;
        private GameObject m_object = null;
        private Dictionary<int, int> m_objectOldLayers = new Dictionary<int, int>();

        public float orthographicSize => m_camera.orthographicSize;

        public void setObject(GameObject obj, int layer = -1)
        {
            if (null == obj)
                return;

            m_object = obj;
            GraphicHelper.setParent(m_objectPosition, obj);

            m_objectOldLayers.Clear();
            if (0 <= layer)
            {                
                GraphicHelper.setLayer(m_object, layer, m_objectOldLayers);
            }
        }

        public void restoreObjectLayers()
        {
            if (null != m_object)
            {
                if (0 < m_objectOldLayers.Count)
                {
                    GraphicHelper.setLayer(m_object, m_objectOldLayers);

                    m_objectOldLayers.Clear();
                    m_object = null;
                }
            }
        }

        public void setRenderTexture(RenderTexture texture)
        {
            m_renderTexture = texture;
            m_camera.targetTexture = texture;
        }

        public void setCullingMask(LayerMask layerMask)
        {
            m_camera.cullingMask = layerMask.value;
        }

        public void setCullingMask(int cullingMask)
        {
            m_camera.cullingMask = cullingMask;
        }

        private void OnDestroy()
        {
            restoreObjectLayers();

            if (null != m_renderTexture)
            {
                m_renderTexture.Release();
                m_renderTexture = null;
            }
        }
    }
}