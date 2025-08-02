using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public class MoveBoundary
    {
        public enum eCollision { None, LX, RX, BY, TY };

        public virtual void check(ref Vector3 position)
        {

        }

        public virtual bool isIn(ref Vector3 position)
        {
            return false;
        }

        public virtual eCollision isCollision(ref Vector3 position)
        {
            return eCollision.None;
        }
    }
}