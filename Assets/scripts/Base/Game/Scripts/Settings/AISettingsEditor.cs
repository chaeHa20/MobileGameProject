using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(AISettings))]
public class AISettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
    }
}

#endif