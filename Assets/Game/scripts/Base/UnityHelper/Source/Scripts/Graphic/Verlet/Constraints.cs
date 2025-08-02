using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public abstract class Constraint
    {
        public abstract void Relax(float deltaTime, float stepCoef);
    }

    public class DistanceConstraint : Constraint
    {
        public VerletParticle a;
        public VerletParticle b;
        /// <summary>
        /// ´Ü´ÜÇÔ
        /// </summary>
        public float stiffness;
        public float distance = 0f;

        public DistanceConstraint(VerletParticle a, VerletParticle b, float stiffness)
        {
            if (a == b)
            {
                Debug.Log("Can't constrain a particle to itself!");
                return;
            }
            this.a = a;
            this.b = b;
            this.stiffness = stiffness;
            this.distance = (a.pos - b.pos).sqrMagnitude;
        }

        public DistanceConstraint(VerletParticle a, VerletParticle b, float stiffness, float distance)
        {
            this.a = a;
            this.b = b;
            this.stiffness = stiffness;
            this.distance = distance;
        }

        public override void Relax(float deltaTime, float stepCoef)
        {
            Vector3 normal = a.pos - b.pos;

            float m = normal.magnitude;

            normal *= ((distance * distance - m) / m) * stepCoef * stiffness * deltaTime;

            a.pos += normal;
            b.pos -= normal;
        }
    }

    public class PinConstraint : Constraint
    {
        public VerletParticle a;
        public Vector3 pos;

        public PinConstraint(VerletParticle a, Vector3 pos)
        {
            this.a = a;
            this.pos = pos;
            a.pos = pos;
        }

        public PinConstraint(VerletParticle a)
        {
            this.a = a;
            this.pos = a.pos;
        }

        public override void Relax(float deltaTime, float stepCoef)
        {
            a.pos = pos;
        }
    }

    /*
    public class AngleConstraint : Constraint
    {
        public Vector2 a, b, c;
        public float angle;
        public float stiffness;

        public AngleConstraint(Vector2 a, Vector2 b, Vector2 c, float stiffness)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.stiffness = stiffness;
            angle = b.Angle2(a, c);
        }

        public override void Relax(float stepCoef)
        {
            float angleBetween = b.Angle2(a, c);
            float diff = angleBetween - angle;

            if (diff <= -Mathf.PI)
                diff += 2f * Mathf.PI;
            else if (diff >= Mathf.PI)
                diff -= 2f * Mathf.PI;

            diff *= stepCoef * stiffness;

            a = a.Rotate(b, diff);
            c = c.Rotate(b, -diff);
            b = b.Rotate(a, diff);
            b = b.Rotate(c, -diff);
        }
    }
    */
}