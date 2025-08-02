using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    [ExecuteInEditMode]
    public class DebugDrawDirection : DebugDrawGizmo
    {
        [SerializeField] float m_length = 1.0f;

        private Vector3 m_dir = Vector3.zero;

        public Vector3 dir { set { m_dir = value; } }

#if UNITY_EDITOR
        protected override void drawGizmos()
        {
            base.drawGizmos();

            var start = transform.position;
            var end = start + m_dir * m_length;

            Debug.DrawLine(start, end, color);
            drawLabel(end);
        }
#endif
    }
}