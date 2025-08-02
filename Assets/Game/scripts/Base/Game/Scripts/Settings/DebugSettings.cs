using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;
using System;
using System.Linq;
using System.Numerics;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "App/DebugSettings instance")]
public class DebugSettings : BaseDebugSettings
{
    private static DebugSettings m_instance = null;

    public static DebugSettings instance
    {
        get
        {
            if (null == m_instance)
            {
                m_instance = Resources.Load<DebugSettings>("Settings/DebugSettings");
            }

            return m_instance;
        }
    }

    [Serializable]
    public class EditorValues
    {
        public eCurrency addCurrencyType = eCurrency.Gold;
        public string addCurrencyValue = "0";
        public string targetMapId = "0";
        public string workerId = "0";
        public eMotion motion = eMotion.None;
    }

    [Serializable]
    public class Gizmo
    {
        public bool isDrawGoalMovePath = true;
        public bool isDrawEntityRadius = false;
        public bool isDrawGoalAlas = false;        
    }


    private EditorValues m_editorValues = new EditorValues();

    [SerializeField] Gizmo m_gizmo = new Gizmo();
    
    public EditorValues editorValues => m_editorValues;
    public Gizmo gizmo => m_gizmo;
   

#if UNITY_EDITOR
    [MenuItem("Settings/DebugSettings")]
    static void Settings()
    {
        Selection.activeObject = instance;
    }

    public static void create()
    {
        if (null == instance)
        {
            var asset = ScriptableObject.CreateInstance<DebugSettings>();

            var name = UnityEditor.AssetDatabase.GenerateUniqueAssetPath("Assets/Game/Resources/Settings/DebugSettings.asset");
            AssetDatabase.CreateAsset(asset, name);
            AssetDatabase.SaveAssets();
        }
    }
#endif
}
