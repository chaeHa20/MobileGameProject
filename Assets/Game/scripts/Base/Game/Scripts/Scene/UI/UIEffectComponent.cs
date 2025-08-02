using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class UIEffectComponent : Disposable
{
    [SerializeField] List<ParticleSystem> m_particles = new List<ParticleSystem>();

    private float m_maxDuration = 2.5f;

    public void initialize()
    {
        foreach (var particle in m_particles)
        {
            var duration = particle.main.duration + particle.main.startDelayMultiplier;
            if (m_maxDuration < duration)
                m_maxDuration = duration;
        }

        CoroutineHelper.instance.start(coPlay(() =>
        {
            endParticle();
        }));
    }

    IEnumerator coPlay(Action callback)
    {
        yield return new WaitForSeconds(m_maxDuration + 0.5f);

        callback?.Invoke();
    }

    private void endParticle()
    {
        Dispose();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            m_particles.Clear();
            Destroy(gameObject);
        }
    }
}