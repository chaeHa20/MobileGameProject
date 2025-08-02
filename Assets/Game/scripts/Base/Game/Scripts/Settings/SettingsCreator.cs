using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityHelper;
using System;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "App/Create All Settings")]
public class SettingsCreator : ScriptableObject
{
#if UNITY_EDITOR
    [MenuItem("Settings/[Create All Settings]")]
    static void Settings()
    {
        AdSettings.create();
        AESSettings.create();
        BuildSettings.create();
        DebugSettings.create();
        GameSettings.create();
        AISettings.create();
    }
#endif
}
