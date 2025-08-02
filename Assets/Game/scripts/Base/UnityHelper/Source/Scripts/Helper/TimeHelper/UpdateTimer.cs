using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public class UpdateTimer
    {
        private float m_updateTime = 0.0f;
        private float m_elapsedTime = 0.0f;
        private bool m_isUpdateOne = false;

        public void initialize(float updateTime, bool immediately)
        {
            m_updateTime = updateTime;
            m_isUpdateOne = false;

            if (immediately)
                m_elapsedTime = m_updateTime;
            else
                m_elapsedTime = 0.0f;
        }

        public void clearElapsedTime()
        {
            m_elapsedTime = 0.0f;
        }

        /// <summary>
        /// 업데이트가 완료되면 true를 리턴합니다.
        /// </summary>
        public bool update(float dt)
        {
            m_elapsedTime += dt;
            if (m_elapsedTime >= m_updateTime)
            {
                m_elapsedTime = 0.0f;
                return true;
            }

            return false;
        }

        public bool updateOne(float dt)
        {
            if (m_isUpdateOne)
                return false;

            m_elapsedTime += dt;
            if (m_elapsedTime >= m_updateTime)
            {
                m_isUpdateOne = true;
                m_elapsedTime = 0.0f;
                return true;
            }

            return false;
        }
    }
}