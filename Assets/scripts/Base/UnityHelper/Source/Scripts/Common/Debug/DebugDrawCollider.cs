using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    [ExecuteInEditMode]
    public class DebugDrawCollider : DebugDrawGizmo
    {
        //[SerializeField] Color m_normalColor = new Color(160.0f / 255.0f, 227.0f / 255.0f, 126.0f / 255.0f);
        [SerializeField] Color m_selectColor = Color.red;

#if UNITY_EDITOR
        protected override void drawGizmos()
        {
            Gizmos.color = getGizmoColor();

            Collider collider = GetComponent<Collider>();
            BoxCollider boxCollider;
            SphereCollider sphereCollider;
            if (null != (boxCollider = collider as BoxCollider))
            {
                Gizmos.DrawWireCube(transform.position, boxCollider.size);
            }
            else if (null != (sphereCollider = collider as SphereCollider))
            {
                Gizmos.DrawWireSphere(transform.position, sphereCollider.radius);
            }

            drawLabel(transform.position);
        }

        private Color getGizmoColor()
        {
            if (UnityEditor.Selection.Contains(gameObject.GetInstanceID()))
            {
                return m_selectColor;
            }
            else
            {
                GameObject selectObject = UnityEditor.Selection.activeGameObject;
                if (null != selectObject)
                {
                    DebugDrawCollider[] childs = selectObject.transform.GetComponentsInChildren<DebugDrawCollider>();
                    for (int i = 0; i < childs.Length; ++i)
                    {
                        if (childs[i].gameObject.GetInstanceID() == gameObject.GetInstanceID())
                            return m_selectColor;
                    }
                }

                return color;
            }
        }
#endif
    }
}