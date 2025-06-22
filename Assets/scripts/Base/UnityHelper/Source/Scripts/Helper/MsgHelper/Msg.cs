using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public abstract class Msg
    {
        public abstract void action();

        public virtual string toString()
        {
            return string.Format("{0}", ToString());
        }
    }
}