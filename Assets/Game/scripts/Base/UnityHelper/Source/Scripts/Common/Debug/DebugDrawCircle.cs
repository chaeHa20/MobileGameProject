using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    [ExecuteInEditMode]
    public class DebugDrawCircle : DebugDrawGizmo
    {
        [SerializeField] float m_radius = 1.0f;
        [SerializeField] int m_arcCount = 1;
        [SerializeField] Vector3 m_offset = Vector3.zero;

        public float radius { get { return m_radius; } set { m_radius = value; } }

#if UNITY_EDITOR
        protected override void drawGizmos()
        {
            float arc_angle = 360.0f / m_arcCount;
            float angle = 0.0f;
            Vector3 origin = transform.position;
            Vector3 r = new Vector3(m_radius, 0.0f, 0.0f);
            r += m_offset;
            Vector3 start = r;
            for (int i = 0; i < m_arcCount; ++i)
            {
                angle += arc_angle;
                var quat = Quaternion.AngleAxis(angle, transform.up);
                Vector3 end = quat * r;
                Debug.DrawLine(start + origin, end + origin, color);
                start = end;
            }

            drawLabel(origin);
        }
#endif
    }
}