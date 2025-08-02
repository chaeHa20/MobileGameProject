using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public class BezierMove : CurveMove
    {
        // RectTransform의 SizeDelta를 이용
        private bool m_isApplySizeDelta = false;
        private Vector2 m_startSizeDelta = Vector2.zero;
        private Vector2 m_destSizeDelta = Vector2.zero;

        /// <summary>
        /// start와 end 사이에 center가 위치하는 비율 범위
        /// </summary>
        private fRange m_centerPositionRate = new fRange(0.2f, 0.8f);
        /// <summary>
        /// 곡선의 휘어짐 크기
        /// </summary>
        private fRange m_centerCurveScale = new fRange(0.0f, 3.0f);
        private AnimationCurve m_animationCurve = null;
        
        public fRange centerPositionRate { set { m_centerPositionRate = value; } }
        public fRange centerCurveScale { set { m_centerCurveScale = value; } }
        public bool isApplySizeDelta { set { m_isApplySizeDelta = value; } }
        public Vector2 startSizeDelta { set { m_startSizeDelta = value; } }
        public Vector2 destSizeDelta { set { m_destSizeDelta = value; } }
        public AnimationCurve animationCurve { set { m_animationCurve = value; } }

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

                if (null != m_animationCurve)
                {
                    t = m_animationCurve.Evaluate(t);
                }

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
            Vector3 dir = m_destPosition - m_startPosition;
            dir.Normalize();

            float angle = getCenterPositionAngle();
            Quaternion quat = Quaternion.AngleAxis(angle, getCenterPositionAngleAxis(dir));
            Vector3 orthoDir = quat * dir;

            Vector3 center = Vector3.Lerp(m_startPosition, m_destPosition, m_centerPositionRate.randValue);
            return center + orthoDir * m_centerCurveScale.randValue;
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