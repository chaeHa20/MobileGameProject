using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityHelper
{
    [ExecuteInEditMode]
    public class DebugDrawGizmo : MonoBehaviour
    {
        [SerializeField] bool m_isShow = true;
        [SerializeField] Color m_color = Color.white;
        [SerializeField] string m_label = null;

        protected Color color => m_color;

        public string label { get { return m_label; } set { m_label = value; } }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (!m_isShow)
                return;

            drawGizmos();
        }

        protected virtual void drawGizmos()
        {

        }

        protected void drawLabel(Vector3 position)
        {
            if (string.IsNullOrEmpty(m_label))
                return;

            var restoreColor = GUI.color;

            GUI.color = m_color;
            Handles.Label(position, m_label);
            GUI.color = restoreColor;
        }
#endif
    }
}