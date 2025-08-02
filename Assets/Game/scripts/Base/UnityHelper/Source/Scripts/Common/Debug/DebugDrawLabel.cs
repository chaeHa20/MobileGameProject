using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityHelper
{
    [ExecuteInEditMode]
    public class DebugDrawLabel : DebugDrawGizmo
    {
#if UNITY_EDITOR
        protected override void drawGizmos()
        {
            drawLabel(transform.position);
        }
#endif
    }
}