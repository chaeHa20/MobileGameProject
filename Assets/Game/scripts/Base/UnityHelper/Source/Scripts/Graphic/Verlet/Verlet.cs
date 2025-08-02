using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public class Verlet : IDisposable
    {
        private List<VerletParticle> m_particles = new List<VerletParticle>();
        private List<Constraint> m_constraints = new List<Constraint>();
        private Vector3 m_gravity = new Vector3(0.0f, 0.2f, 0.0f);
        private float m_friction = 0.99f;
        private int m_relaxStepCount = 60;

        public Vector3 Gravity { get { return m_gravity; } set { m_gravity = value; } }
        public float Friction { get { return m_friction; } set { m_friction = value; } }
        public int particleCount => m_particles.Count;

        public void addParticle(VerletParticle particle)
        {
            m_particles.Add(particle);
        }

        public void addConstraint(Constraint constraint)
        {
            m_constraints.Add(constraint);
        }

        public void removeConstraint(Constraint constraint)
        {
            m_constraints.Remove(constraint);
        }

        public Vector3 getLastParticlePosition()
        {
            return m_particles[m_particles.Count - 1].pos;
        }

        public List<VerletParticle>.Enumerator getParticleEnumerator()
        {
            return m_particles.GetEnumerator();
        }

        public void SetStiffness(float stiffness)
        {
            for (int i = 0; i < m_constraints.Count; i++)
            {
                Constraint constraint = m_constraints[i];
                if (constraint.GetType() == typeof(DistanceConstraint))
                {
                    DistanceConstraint distanceConstraint = (DistanceConstraint)constraint;
                    distanceConstraint.stiffness = stiffness;
                }
            }
        }

        public void ForceUpdate(float deltaTime)
        {
            //Move
            foreach (VerletParticle p in m_particles)
            {
                Vector3 vel = (p.pos - p.lastPos) * m_friction;
                p.lastPos = p.pos;
                p.pos += m_gravity * deltaTime;
                p.pos += vel * deltaTime;
            }


            float stepCoef = 1.0f / m_relaxStepCount;

            for (int i = 0; i < m_relaxStepCount; ++i)
            {
                foreach (Constraint co in m_constraints)
                {
                    if (co.GetType() == typeof(DistanceConstraint))
                    {
                        co.Relax(deltaTime, stepCoef);
                    }
                }

                foreach (Constraint co in m_constraints)
                {
                    if (co.GetType() == typeof(PinConstraint))
                    {
                        co.Relax(deltaTime, stepCoef);
                    }
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_particles.Clear();
                m_constraints.Clear();
            }
        }
    }
}