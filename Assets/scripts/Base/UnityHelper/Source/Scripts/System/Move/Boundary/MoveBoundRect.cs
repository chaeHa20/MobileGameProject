using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public class MoveBoundaryRect : MoveBoundary
    {
        private Rect m_rect;

        public Rect rect { get { return m_rect; } set { m_rect = value; } }

        public MoveBoundaryRect(Rect _rect)
        {
            m_rect = _rect;
        }

        public override void check(ref Vector3 position)
        {
            if (position.x < m_rect.xMin)
                position.x = m_rect.xMin;
            else if (position.x > m_rect.xMax)
                position.x = m_rect.xMax;

            if (position.z < m_rect.yMin)
                position.z = m_rect.yMin;
            else if (position.z > m_rect.yMax)
                position.z = m_rect.yMax;
        }

        public override bool isIn(ref Vector3 position)
        {
            if (m_rect.xMin <= position.x && m_rect.xMax >= position.x &&
                m_rect.yMin <= position.z && m_rect.yMax >= position.z)
                return true;

            return false;
        }

        public override eCollision isCollision(ref Vector3 position)
        {
            if (m_rect.xMin >= position.x)
                return eCollision.LX;
            else if (m_rect.xMax <= position.x)
                return eCollision.RX;
            else if (m_rect.yMin >= position.z)
                return eCollision.BY;
            else if (m_rect.yMax <= position.z)
                return eCollision.TY;

            return eCollision.None;
        }
    }
}