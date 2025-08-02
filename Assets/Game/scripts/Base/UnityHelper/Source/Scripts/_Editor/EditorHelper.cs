using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
#endif

namespace UnityHelper
{
#if UNITY_EDITOR
    public class EditorHelper
    {
        public enum eAxis { X, Y, Z };

        public static void drawArrowHandlCap(Transform transform, eAxis axis, float size)
        {
            var forward = Vector3.zero;
            switch(axis)
            {
                case eAxis.X: forward = Vector3.right; break;
                case eAxis.Y: forward = Vector3.up; break;
                case eAxis.Z: forward = Vector3.forward; break;
            }

            drawArrowHandlCap(transform, forward, size);
        }

        public static void drawArrowHandlCap(Transform transform, Vector3 forward, float size)
        {
            Quaternion lookRotation = Quaternion.LookRotation(forward);
            Handles.ArrowHandleCap(0, transform.position, transform.rotation * lookRotation, size, EventType.Repaint);
        }

        public static void drawRotationArrowHandlCap(Transform transform, float size)
        {
            Handles.ArrowHandleCap(0, transform.position, transform.rotation, size, EventType.Repaint);
        }

        public static void drawHandleLabel(Vector3 position, string label, Color color)
        {
            var restoreColor = GUI.color;
            GUI.color = color;
            drawHandleLabel(position, label);
            GUI.color = restoreColor;
        }

        public static void drawHandleLabel(Vector3 position, string label)
        {
            Handles.Label(position, label);
        }

        public static void drawGuiLabel(Vector3 position, string label, Color color)
        {
            var view = SceneView.currentDrawingSceneView;
            if (null == view)
            {
                return;
            }

            Vector3 screenPos = view.camera.WorldToScreenPoint(position);
            if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.z < 0)
            {
                return;
            }

            var oldGuiColor = GUI.color;

            Handles.BeginGUI();
            GUI.color = color;
            Vector2 size = GUI.skin.label.CalcSize(new GUIContent(label));
            GUI.Label(new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height - (size.y / 2), size.x, size.y), label);
            GUI.color = oldGuiColor;
            Handles.EndGUI();
        }

        public static void drawGizmoCircle(Vector3 position, float radius, int detail = 10)
        {
            var rpos = Vector3.right * radius;
            float angle = 360.0f / detail;
            var sr = rpos;
            for (float a = angle; a <= 360.0f; a += angle)
            {
                var q = Quaternion.AngleAxis(a, Vector3.up);
                var er = q * rpos;

                Gizmos.DrawLine(position + sr, position + er);
                sr = er;
            }
        }

        public static void drawGizmoCircle(Vector3 position, float radius, Color color, int detail = 10)
        {
            var oldColor = Gizmos.color;
            Gizmos.color = color;

            drawGizmoCircle(position, radius, detail);

            Gizmos.color = oldColor;
        }

        public static void drawGizmoBox(Vector3 sp, Vector3 ep)
        {
            float y = (sp.y + ep.y) * 0.5f;
            Gizmos.DrawLine(new Vector3(sp.x, y, sp.z), new Vector3(ep.x, y, sp.z));
            Gizmos.DrawLine(new Vector3(ep.x, y, sp.z), new Vector3(ep.x, y, ep.z));
            Gizmos.DrawLine(new Vector3(ep.x, y, ep.z), new Vector3(sp.x, y, ep.z));
            Gizmos.DrawLine(new Vector3(sp.x, y, ep.z), new Vector3(sp.x, y, sp.z));
        }

        public static void drawGizmoBox(Vector3 sp, Vector3 ep, Color color)
        {
            var oldColor = Gizmos.color;
            Gizmos.color = color;

            drawGizmoBox(sp, ep);

            Gizmos.color = oldColor;
        }

        public static void drawLines(List<Vector3> positions, Color color)
        {
            var oldColor = Gizmos.color;
            Gizmos.color = color;

            for (int i = 0; i < positions.Count-1; ++i)
            {
                Gizmos.DrawLine(positions[i], positions[i + 1]);
            }

            Gizmos.color = oldColor;
        }

        public static void setSceneViewHitPosition(GameObject gameObject)
        {
            var sceneView = SceneView.lastActiveSceneView;
            if (Physics.Raycast(sceneView.camera.transform.position, sceneView.camera.transform.forward, out RaycastHit hitInfo))
            {
                gameObject.transform.position = hitInfo.point;
            }
        }

        /// <summary>
        /// 스크립트의 property를 수정한 후에 호출해 주자
        /// </summary>
        public static void markSceneDirty()
        {
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }

        public static void markPrefabDirty()
        {
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (null != prefabStage)
                EditorSceneManager.MarkSceneDirty(prefabStage.scene);
        }

        public static string[] getScenes()
        {
            return EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes);
        }
    }
#endif
}