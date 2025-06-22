using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    [RequireComponent(typeof(LineRenderer))]
    public class LineRendererCircle : MonoBehaviour
    {
        [SerializeField] float m_radius = 1.0f;
        [SerializeField] int m_arcCount = 1;

        private LineRenderer m_lineRenderer = null;

        public float radius { get { return m_radius; } set { m_radius = value; } }

        void Start()
        {
            m_lineRenderer = GetComponent<LineRenderer>();
            m_lineRenderer.positionCount = m_arcCount+1;
        }

        void Update()
        {
            if (m_lineRenderer.positionCount != m_arcCount + 1)
                m_lineRenderer.positionCount = m_arcCount + 1;

            float arc_angle = 360.0f / m_arcCount;
            float angle = 0.0f;
            Vector3 origin = transform.position;
            Vector3 r = new Vector3(m_radius, 0.0f, 0.0f);
            Vector3 start = r;
            for (int i = 0; i < m_arcCount; ++i)
            {
                m_lineRenderer.SetPosition(i, start + origin);

                angle += arc_angle;
                var quat = Quaternion.AngleAxis(angle, transform.up);
                Vector3 end = quat * r;                
                start = end;
            }

            m_lineRenderer.SetPosition(m_arcCount, m_lineRenderer.GetPosition(0));
        }
    }
}