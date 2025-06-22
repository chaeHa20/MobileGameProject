using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
namespace UnityHelper
{
    public class BaseDebugSettingsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.DrawDefaultInspector();
            onClearLocalData();
        }

        private void onClearLocalData()
        {
            Color oldBackground = GUI.backgroundColor;
            GUI.backgroundColor = Color.red;

            GUILayout.Space(10.0f);

            if (GUILayout.Button("Clear LocalData"))
            {
                PlayerPrefs.DeleteAll();
                if (Logx.isActive)
                    Logx.trace("Success clear local data");

                EditorUtility.DisplayDialog("Info", "Success clear local data", "Ok");
            }

            GUI.backgroundColor = oldBackground;
        }
    }
}
#endif
