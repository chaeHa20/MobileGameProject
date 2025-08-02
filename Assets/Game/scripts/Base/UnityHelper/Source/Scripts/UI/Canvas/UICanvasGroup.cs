using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace UnityHelper
{
    [Serializable]
    public class UICanvasGroup
    {
        [SerializeField] List<UICanvas> m_canvases = new List<UICanvas>();

        private int m_msgBoxCanvasIndex = 0;
        private int m_firstCanvasIndex = 0;
        private int m_lastCanvasIndex = 0;

        public void initialize()
        {
#if UNITY_EDITOR
            checkDumplicateLayers();
#endif

            var firstNormalLayer = int.MaxValue;
            var lastNormalLayer = int.MinValue;

            for (int i = 0; i < m_canvases.Count; ++i)
            {
                var canvas = m_canvases[i];
                canvas.initialize();

                if (canvas.isMsgBox)
                {
                    m_msgBoxCanvasIndex = i;
                }
                else if (UICanvas.eCanvas.Normal == canvas.type)
                {
                    if (canvas.layer > lastNormalLayer)
                    {
                        lastNormalLayer = canvas.layer;
                        m_lastCanvasIndex = i;
                    }
                    if (canvas.layer < firstNormalLayer)
                    {
                        firstNormalLayer = canvas.layer;
                        m_firstCanvasIndex = i;
                    }
                }
            }
        }

#if UNITY_EDITOR
        private void checkDumplicateLayers()
        {
            var layers = new HashSet<int>();
            foreach(var canvas in m_canvases)
            {
                if (layers.Contains(canvas.layer))
                {
                    if (Logx.isActive)
                        Logx.error("Duplicated Canvas layer {0}", canvas.layer);
                }
                else
                {
                    layers.Add(canvas.layer);
                }
            }
        }
#endif

        public UICanvas getCanvas(int layer)
        {
            var canvas = (from c in m_canvases
                          where c.layer == layer
                          select c).FirstOrDefault();

            if (null == canvas)
                return m_canvases[0];

            return canvas;
        }

        public UICanvas getLastCanvas()
        {
            return m_canvases[m_lastCanvasIndex];
        }

        public UICanvas getFirstCanvas()
        {
            return m_canvases[m_firstCanvasIndex];
        }

        public int getMsgBoxLayer()
        {
            return m_canvases[m_msgBoxCanvasIndex].layer;
        }

        public GameObject getSafeArea(int layer)
        {
            return getCanvas(layer).safeArea;
        }

        public GameObject getLastSafeArea()
        {
            return getLastCanvas().safeArea;
        }

        public GameObject getFirstSafeArea()
        {
            return getFirstCanvas().safeArea;
        }
    }
}