using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public class BezierPath : MonoBehaviour
    {
        [SerializeField] GameObject m_start = null;
        [SerializeField] GameObject m_end = null;
        [SerializeField] GameObject m_center = null;
        [SerializeField] float m_length = 0.0f;

        public Vector3 startPosition => m_start.transform.position;
        public Vector3 endPosition => m_end.transform.position;
        public Vector3 centerPosition => m_center.transform.position;

        public void setStart(GameObject start)
        {
            m_start = start;
        }

        public void setEnd(GameObject end)
        {
            m_end = end;
        }

        public Vector3 getPosition(float t)
        {
            return MathHelper.bezierCurve(t, m_start.transform.position, m_center.transform.position, m_end.transform.position);
        }

        public Vector3 getPosition(float t, Vector3 startPosition, Vector3 centerPosition, Vector3 endPosition)
        {
            return MathHelper.bezierCurve(t, startPosition, centerPosition, endPosition);
        }

        public void getForward(float pathLength, ref Vector3 position, out Vector3 forward)
        {
            var t = pathLength / m_length;
            var p = getPosition(t);

            forward = p - position;
            forward.Normalize();

            position = p;
        }

        public Vector3 getForwardAtCustomStartPosition(float t, Vector3 startPosition, float offset_t = 0.01f)
        {
            t = Mathf.Min(1.0f, t);

            var p = getPosition(t, startPosition, m_center.transform.position, m_end.transform.position);
            var forward = Vector3.zero;

            if (0.0f >= t)
            {
                var nextP = getPosition(offset_t, startPosition, m_center.transform.position, m_end.transform.position);
                forward = nextP - p;
            }
            else
            {
                var prevP = getPosition(t - offset_t, startPosition, m_center.transform.position, m_end.transform.position);
                forward = p - prevP;
            }

            return forward.normalized;
        }

        public Vector3 getForward(float t, float offset_t = 0.01f)
        {
            t = Mathf.Min(1.0f, t);

            var p = getPosition(t);
            var forward = Vector3.zero;

            if (0.0f >= t)
            {
                var nextP = getPosition(offset_t);
                forward = nextP - p;
            }
            else
            {
                var prevP = getPosition(t - offset_t);
                forward = p - prevP;
            }

            return forward.normalized;
        }

        public void getBackward(float pathLength, ref Vector3 position, out Vector3 backward)
        {
            var t = pathLength / m_length;
            var p = getPosition(t);

            backward = position - p;
            backward.Normalize();

            position = p;
        }

        public Vector3 getBackward(float t, float offset_t = 0.01f)
        {
            t = Mathf.Min(1.0f, t);

            var p = getPosition(t);
            var backward = Vector3.zero;

            if (1.0f <= t)
            {
                var nextP = getPosition(1.0f - offset_t);
                backward = nextP - p;
            }
            else
            {
                var prevP = getPosition(t + offset_t);
                backward = p - prevP;
            }

            return backward.normalized;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (null == m_start || null == m_end || null == m_center)
                return;

            var prevPosition = m_start.transform.position;
            for (int i = 0; i <= 10; ++i)
            {
                var t = i / 10.0f;

                var p = getPosition(t);
                Gizmos.DrawLine(prevPosition, p);
                prevPosition = p;
            }
        }

        public void setCenterPositionZ(float z)
        {
            var cp = m_center.transform.position;
            cp.z = z;
            m_center.transform.position = cp;
        }
#endif
    }
}