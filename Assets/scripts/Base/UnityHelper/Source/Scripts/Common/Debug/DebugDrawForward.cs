using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityHelper
{
    [ExecuteInEditMode]
    public class DebugDrawForward : DebugDrawDirection
    {
#if UNITY_EDITOR
        protected override void drawGizmos()
        {
            dir = transform.forward;

            base.drawGizmos();
        }
#endif
    }
}