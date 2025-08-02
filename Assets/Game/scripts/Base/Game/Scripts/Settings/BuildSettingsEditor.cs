using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(BuildSettings))]
public class BuildSettingsEditor : BaseBuildSettingsEditor
{
    protected override void Awake()
    {
        base.Awake();

        PlayerSettings.Android.keystorePass = "nexelon123!@#";
        PlayerSettings.Android.keyaliasPass = "nexelon123!@#";
    }
}

#endif