using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityHelper
{
    [ExecuteInEditMode]
    public class DebugDrawGizmoCombo : MonoBehaviour
    {
        [Serializable]
        public class BaseGizmo
        {
            public bool isShow = false;
            public Color color = Color.white;
        }

        [Serializable]
        public class Label : BaseGizmo
        {
            public bool isShowParentName = false;
            public string label = null;
            public Vector3 nameLabelOffset = Vector3.zero;
        }

        [Serializable]
        public class BoxCollider : BaseGizmo
        {

        }

        [Serializable]
        public class Forward : BaseGizmo
        {
            public float arrowSize = 1.0f;
            public float circleRadius = 1.0f;
        }

        [Serializable]
        public class Cube : BaseGizmo
        {
            public float size = 1.0f;
        }

        [Serializable]
        public class Sphere : BaseGizmo
        {
            public float radius = 1.0f;
        }

        [Serializable]
        public class Circle : BaseGizmo
        {
            public Vector3 normal = Vector3.up;
            public float radius = 1.0f;
        }

        [SerializeField] bool m_isHideWhenPlay = false;
        [SerializeField] Label m_label = new Label();
        [SerializeField] BoxCollider m_boxCollider = new BoxCollider();
        [SerializeField] Forward m_forward = new Forward();
        [SerializeField] Cube m_cube = new Cube();
        [SerializeField] Sphere m_sphere = new Sphere();
        [SerializeField] Circle m_circle = new Circle();

        private Color m_oldColor = Color.white;

#if UNITY_EDITOR
        private void Awake()
        {
            if (EditorApplication.isPlaying)
            {
                if (m_isHideWhenPlay)
                {
                    m_label.isShow = false;
                    m_boxCollider.isShow = false;
                    m_forward.isShow = false;
                    m_cube.isShow = false;
                    m_sphere.isShow = false;
                    m_circle.isShow = false;
                }
            }
        }

        void OnDrawGizmos()
        {
            onDrawLabel();
            onDrawBoxCollider();
            onDrawForward();
            onDrawCube();
            onDrawSphere();
            onDrawCircle();
        }

        private void beginColor(Color color)
        {
            m_oldColor = Handles.color;
            Handles.color = color;
            Gizmos.color = color;
        }

        private void endColor()
        {
            Handles.color = m_oldColor;
            Gizmos.color = m_oldColor;
        }

        private void onDrawLabel()
        {
            if (!m_label.isShow)
                return;

            var name = (string.IsNullOrEmpty(m_label.label)) ? base.name : m_label.label;
            beginColor(m_label.color);
            string _name = (m_label.isShowParentName) ? transform.parent.name + "_" + name : name;
            Handles.Label(transform.position + m_label.nameLabelOffset, _name);
            endColor();
        }

        private void onDrawBoxCollider()
        {
            if (!m_boxCollider.isShow)
                return;

            var boxCollider = GetComponent<UnityEngine.BoxCollider>();
            if (null == boxCollider)
                return;

            beginColor(m_boxCollider.color);
            var oldMatrix = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
            Gizmos.matrix = oldMatrix;
            endColor();
        }

        private void onDrawForward()
        {
            if (!m_forward.isShow)
                return;

            beginColor(m_forward.color);
            Handles.DrawWireDisc(transform.position, transform.forward, m_forward.circleRadius);
            EditorHelper.drawArrowHandlCap(transform, EditorHelper.eAxis.Z, m_forward.arrowSize);
            endColor();
        }

        private void onDrawCube()
        {
            if (!m_cube.isShow)
                return;

            beginColor(m_cube.color);
            Gizmos.DrawWireCube(transform.position, transform.lossyScale * m_cube.size);
            endColor();
        }

        private void onDrawSphere()
        {
            if (!m_sphere.isShow)
                return;

            beginColor(m_sphere.color);
            Gizmos.DrawWireSphere(transform.position, m_sphere.radius);
            endColor();
        }

        private void onDrawCircle()
        {
            if (!m_circle.isShow)
                return;

            beginColor(m_circle.color);
            Handles.DrawWireDisc(transform.position, m_circle.normal, m_circle.radius);
            endColor();
        }
#endif
    }
}