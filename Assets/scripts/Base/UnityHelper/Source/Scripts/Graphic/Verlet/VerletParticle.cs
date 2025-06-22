using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public class VerletParticle
    {
        public Vector3 pos = Vector3.zero;
        public Vector3 lastPos = Vector3.zero;

        public VerletParticle(Vector3 pos)
        {
            this.pos = pos;
            this.lastPos = pos;
        }
    }
}