using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public class MoveBoundaryCircle : MoveBoundary
    {
        private Vector3 m_center = Vector3.zero;
        private float m_radius = 0.0f;

        public override void check(ref Vector3 position)
        {
            Vector3 dir = position - m_center;
            float len = dir.magnitude;
            if (len < m_radius)
                return;

            dir.Normalize();
            position = m_center + dir * m_radius;
        }

        public override bool isIn(ref Vector3 position)
        {
            Vector3 dir = position - m_center;
            return (dir.sqrMagnitude <= m_radius * m_radius);
        }
    }
}