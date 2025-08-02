using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace UnityHelper
{
    public class PoolParticle : PoolObject
    {
        public enum ePlay { None, AutoDestroy, AutoStop };

        [SerializeField] List<ParticleSystem> m_particleSystems = new List<ParticleSystem>();

        private float m_playTime = 0.0f;

        void Awake()
        {
            setActiveParticle(false);
        }

        public override void initializePool(long _poolUuid, string _poolName)
        {
            base.initializePool(_poolUuid, _poolName);

            m_playTime = getPlayTime();
        }

        public void play(Vector3 position, ePlay playType)
        {
            gameObject.transform.position = position;
            play(playType);
        }

        public void play(ePlay playType)
        {
            setActiveParticle(true);

            if (ePlay.AutoDestroy == playType)
                StartCoroutine(coAutoDestroy());
            else if (ePlay.AutoStop == playType)
                StartCoroutine(coAutoStop());
        }

        IEnumerator coAutoDestroy()
        {
            yield return StartCoroutine(coWaitPlayEnd());

            Dispose();
        }

        IEnumerator coAutoStop()
        {
            yield return StartCoroutine(coWaitPlayEnd());

            setActiveParticle(false);
        }

        IEnumerator coWaitPlayEnd()
        {
            yield return new WaitForSeconds(m_playTime);
        }

        private void setActiveParticle(bool isActive)
        {
            m_particleSystems.ForEach(x => x.gameObject.SetActive(isActive));
        }

        private float getPlayTime()
        {
            float maxDuration = m_particleSystems.Max(x => x.main.duration + x.main.startLifetime.constant);
            return maxDuration;
        }

        public void setStartColor(Color startColor)
        {
            foreach(var ps in m_particleSystems)
            {
                var main = ps.main;
                main.startColor = startColor;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                setActiveParticle(false);

            base.Dispose(disposing);
        }
    }
}