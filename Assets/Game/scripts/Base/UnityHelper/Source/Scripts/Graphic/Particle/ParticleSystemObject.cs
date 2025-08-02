using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public class ParticleSystemObject : MonoBehaviour
    {
        [SerializeField] bool m_isAutoDestroy = false;
        // 계산하는 방법을 모르겠어서 직접 입력 받게 함.
        [SerializeField] float m_stopAndDestroyWaitTime = 2.0f;
        [SerializeField] List<ParticleSystem> m_ps = new List<ParticleSystem>();

        void Start()
        {
            if (m_isAutoDestroy)
                setAutoDestroy();
        }

        private void setAutoDestroy()
        {
            float maxDuration = GraphicHelper.getParticlePlayTime(m_ps);

            if (0.0f < maxDuration)
                GameObject.Destroy(gameObject, maxDuration);
        }

        public void restart()
        {
            foreach(var ps in m_ps)
            {
                ps.Clear();
                ps.Play();
            }
        }

        public void play()
        {
            foreach (var ps in m_ps)
            {
                ps.Play();
            }
        }

        public void stop(bool isDestroy)
        {
            GraphicHelper.setParent(null, gameObject);
            foreach (var ps in m_ps)
            {
                ps.Stop();
            }

            if (isDestroy)
                Invoke("destroy", m_stopAndDestroyWaitTime);
        }

        private void destroy()
        {
            GameObject.Destroy(gameObject);
        }
    }
}