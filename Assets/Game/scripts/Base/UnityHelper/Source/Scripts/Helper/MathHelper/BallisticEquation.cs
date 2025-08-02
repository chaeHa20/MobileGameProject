using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public class BallisticEquation
    {
        private bool m_isFire = false;
        private float m_elapsedTime = 0.0f;
        private Vector3 m_oriPosition;
        private Vector2 m_v0 = Vector2.zero;
        private Vector2 m_v0Sign = Vector2.zero;
        private float m_accel = 0.0f;
        private float m_radian = 0.0f;
        private Action<float, float, float> m_updateCallback = null;

        public bool isFire { get { return m_isFire; } }

        public void fire(Vector3 position, float v0, float accel, float radian, Action<float, float, float> updateCallback)
        {
            m_isFire = true;
            m_elapsedTime = 0.0f;
            m_oriPosition = position;
            m_radian = radian;
            m_accel = accel;
            m_v0 = new Vector2(v0 * Mathf.Cos(m_radian), v0 * Mathf.Sin(m_radian));
            setV0Sign();
            m_updateCallback = updateCallback;
        }

        private void setV0Sign()
        {
            m_v0Sign.x = (0.0f > m_v0.x) ? -1.0f : 1.0f;
            m_v0Sign.y = (0.0f > m_v0.y) ? -1.0f : 1.0f;
        }

        public void stop()
        {
            m_isFire = false;
        }

        public void update(Transform transform)
        {
            if (!m_isFire)
                return;

            float a = m_accel * m_elapsedTime;
            m_v0 += a * m_v0Sign;

            Vector3 oldPosition = transform.position;

            m_elapsedTime += Time.deltaTime;
            float x = m_v0.x * m_elapsedTime;
            float y = m_v0.y * m_elapsedTime - 0.5f * 9.8f * m_elapsedTime * m_elapsedTime;

            Vector3 newPosition = new Vector3(m_oriPosition.x + x, m_oriPosition.y + y, m_oriPosition.z);
            Quaternion newRotation = getRotation(in newPosition, in oldPosition);

            transform.position = newPosition;
            transform.rotation = newRotation;

            m_updateCallback?.Invoke(newPosition.x, newPosition.y, newPosition.z);
        }

        private Quaternion getRotation(in Vector3 newPosition, in Vector3 oldPosition)
        {
            Vector3 dir = newPosition - oldPosition;
            dir.Normalize();

            float angle = Vector3.Angle(Vector3.right, dir);
            Vector3 c = Vector3.Cross(Vector3.right, dir);
            if (0.0f < c.z)
                angle *= -1.0f;

            return Quaternion.AngleAxis(angle, Vector3.back);
        }
    }
}