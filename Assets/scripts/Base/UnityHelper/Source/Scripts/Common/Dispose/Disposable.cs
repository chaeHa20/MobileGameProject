using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public class Disposable : MonoBehaviour, IDisposable
    {
        public virtual void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {

        }

        public static void destroy(Disposable behaviour)
        {
            if (null == behaviour)
                return;

            behaviour.Dispose();
            GameObject.Destroy(behaviour.gameObject);
        }
    }
}