using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public class DirectlyMove : BaseMove
    {
        // RectTransform의 SizeDelta를 이용
        private bool m_isApplySizeDelta = false;
        private Vector2 m_startSizeDelta = Vector2.zero;
        private Vector2 m_destSizeDelta = Vector2.zero;

        /// <summary>
        /// start와 end 사이에 center가 위치하는 비율 범위
        /// </summary>
        private float m_centerPositionRate = 0.0f;
        
        
        public float centerPositionRate { set { m_centerPositionRate = value; } }
        public bool isApplySizeDelta { set { m_isApplySizeDelta = value; } }
        public Vector2 startSizeDelta { set { m_startSizeDelta = value; } }
        public Vector2 destSizeDelta { set { m_destSizeDelta = value; } }

        protected override IEnumerator coMove()
        {
            float totalLen = Vector3.Distance(m_destPosition, m_startPosition);
            float elapsedLen = 0.0f;

            Vector3 centerPosition = calcCenterPosition();

            /*
             * size delta
             * */
            RectTransform rtMoveObject = null;
            if (m_isApplySizeDelta)
                rtMoveObject = m_moveObject.GetComponent<RectTransform>();

            var oldMoveObjectPosition = m_moveObject.transform.position;
            while (elapsedLen < totalLen)
            {
                float s = Time.unscaledDeltaTime * m_speed;
                if (elapsedLen + s > totalLen)
                {
                    s = totalLen - elapsedLen;
                }

                elapsedLen += s;
                float t = elapsedLen / totalLen;

                Vector3 p = MathHelper.bezierCurve(t, m_startPosition, centerPosition, m_destPosition);
                m_moveObject.transform.position = p;

                if (m_isApplySizeDelta && null != rtMoveObject)
                    rtMoveObject.sizeDelta = Vector2.Lerp(m_startSizeDelta, m_destSizeDelta, t);

                m_moveCallback?.Invoke(t, oldMoveObjectPosition);

                oldMoveObjectPosition = p;

                yield return null;
            }

            m_arriveCallback?.Invoke();
        }

        protected virtual float getCenterPositionAngle()
        {
            float angle = (0 == UnityEngine.Random.Range(0, 2)) ? 90.0f : -90.0f;
            return angle;
        }

        protected virtual Vector3 getCenterPositionAngleAxis(Vector3 moveDir)
        {
            return Vector3.back;
        }

        protected virtual Vector3 calcCenterPosition()
        {
            Vector3 center = Vector3.Lerp(m_startPosition, m_destPosition, m_centerPositionRate);
            return center;
        }

#if UNITY_EDITOR
        public void onDrawGizmos()
        {
            Vector3 centerPosition = calcCenterPosition();

            var oldPosition = m_startPosition;
            float t = 0.0f;
            while (1.0f > t)
            {
                t += 0.1f;
                t = Mathf.Min(1.0f, t);

                Vector3 p = MathHelper.bezierCurve(t, m_startPosition, centerPosition, m_destPosition);
                Debug.DrawLine(oldPosition, p, Color.red);
                oldPosition = p;
            }

            Debug.DrawLine(m_startPosition, centerPosition, Color.red);
            Debug.DrawLine(centerPosition, m_destPosition, Color.red);
        }
#endif
    }
}