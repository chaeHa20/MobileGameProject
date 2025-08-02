using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public class VerletLine : Disposable 
    {
        [Serializable]
        public struct Data
        {
            public float stiffness;            
            public float friction;
            public Vector3 gravity;
        }

        [SerializeField] protected LineRenderer m_lineRenderer = null;

        private Verlet m_verlet = new Verlet();
        private PinConstraint m_verletStartConstraint = null;
        private PinConstraint m_verletEndConstraint = null;

        public PinConstraint start { get { return m_verletStartConstraint; } }
        public PinConstraint end { get { return m_verletEndConstraint; } }

        public virtual void initialize(Vector3 startPosition, Color color)
        {
            initLineRenderer(color);
            createVerlet(startPosition);            
        }

        protected virtual void initLineRenderer(Color color)
        {
            
        }

        protected virtual List<Vector3> makeParticlePositions(Vector3 startPosition)
        {
            return null;
        }

        protected virtual void createVerlet(Vector3 startPosition)
        {

        }

        protected void createVerlet(Data data, Vector3 startPosition)
        {
            m_verlet.Dispose();

            var particlePositions = makeParticlePositions(startPosition);

            VerletParticle lastParticle = null;
            for (int i = 0; i < particlePositions.Count; i++)
            {
                VerletParticle currentParticle = new VerletParticle(particlePositions[i]);
                m_verlet.addParticle(currentParticle);

                if (0 == i)
                {
                    m_verletStartConstraint = new PinConstraint(currentParticle);
                    m_verletStartConstraint.pos = particlePositions[0];
                    m_verlet.addConstraint(m_verletStartConstraint);
                }
                else
                {
                    m_verlet.addConstraint(new DistanceConstraint(lastParticle, currentParticle, data.stiffness));
                }

                lastParticle = currentParticle;
            }

            m_verletEndConstraint = new PinConstraint(lastParticle);
            m_verlet.addConstraint(m_verletEndConstraint);
            setData(data);

            m_lineRenderer.positionCount = m_verlet.particleCount;
        }

        void Update()
        {
            update();
        }

        protected virtual void update()
        {
            updateStartEnd();
            m_verlet.ForceUpdate(Time.deltaTime);
            updateLinePositions();
        }

        protected virtual void updateStartEnd()
        {
           
        }

        private void updateLinePositions()
        {
            int index = 0;
            var e = m_verlet.getParticleEnumerator();
            while (e.MoveNext())
            {
                m_lineRenderer.SetPosition(index++, e.Current.pos);
            }
        }

        public void remove(PinConstraint constraint)
        {
            m_verlet.removeConstraint(constraint);
        }

        public void setData(Data data)
        {
            m_verlet.Gravity = data.gravity;
            m_verlet.Friction = data.friction;
            m_verlet.SetStiffness(data.stiffness);
        }

        public void setActiveRenderLine(bool isActive)
        {
            m_lineRenderer.gameObject.SetActive(isActive);
        }

        public Vector3 getLastParticlePosition()
        {
            return m_verlet.getLastParticlePosition();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                m_verlet.Dispose();
            }
        }
    }
}