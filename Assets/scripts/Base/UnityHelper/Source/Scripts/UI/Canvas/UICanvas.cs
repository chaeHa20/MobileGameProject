using Crystal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    [Serializable]
    public class UICanvas
    {
        public enum eCanvas { Normal, Block, MsgBox };

        [SerializeField] int m_layer = 0;
        [SerializeField] Canvas m_canvas = null;        
        [SerializeField] eCanvas m_type = eCanvas.Normal;

        private SafeArea m_safeArea = null;
        private RectTransform m_rtCanvas = null;

        public int layer => m_layer;
        public bool isMsgBox => eCanvas.MsgBox == m_type;
        public GameObject safeArea => m_safeArea.gameObject;
        public RectTransform rtCanvas => m_rtCanvas;
        public Canvas canvas => m_canvas;
        public eCanvas type => m_type;

        public void initialize()
        {
            m_rtCanvas = m_canvas.GetComponent<RectTransform>();
            m_safeArea = m_canvas.GetComponentInChildren<SafeArea>();

            m_canvas.sortingOrder = m_layer;
        }
    }
}