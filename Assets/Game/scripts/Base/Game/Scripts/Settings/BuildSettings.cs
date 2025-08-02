using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "App/BuildSettings instance")]
public class BuildSettings : BaseBuildSettings
{
    private static BuildSettings m_instance = null;

    public static BuildSettings instance
    {
        get
        {
            if (null == m_instance)
            {
                m_instance = Resources.Load<BuildSettings>("Settings/BuildSettings");
            }

            return m_instance;
        }
    }

#if UNITY_EDITOR
    [MenuItem("Settings/BuildSettings")]
    static void Settings()
    {
        Selection.activeObject = instance;
    }

    public static void create()
    {
        if (null == instance)
        {
            var asset = ScriptableObject.CreateInstance<BuildSettings>();

            var name = UnityEditor.AssetDatabase.GenerateUniqueAssetPath("Assets/Game/Resources/Settings/BuildSettings.asset");
            AssetDatabase.CreateAsset(asset, name);
            AssetDatabase.SaveAssets();
        }
    }
#endif
}
