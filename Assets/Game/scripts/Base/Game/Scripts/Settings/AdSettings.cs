using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "App/AdSettings instance")]
public class AdSettings : BaseAdSettings
{
    private static AdSettings m_instance = null;

    public static AdSettings instance
    {
        get
        {
            if (null == m_instance)
            {
                m_instance = Resources.Load<AdSettings>("Settings/AdSettings");
            }

            return m_instance;
        }
    }

#if UNITY_EDITOR
    [MenuItem("Settings/AdSettings")]
    static void Settings()
    {
        Selection.activeObject = instance;
    }

    public static void create()
    {
        if (null == instance)
        {
            var asset = ScriptableObject.CreateInstance<AdSettings>();

            var name = UnityEditor.AssetDatabase.GenerateUniqueAssetPath("Assets/Game/Resources/Settings/AdSettings.asset");
            AssetDatabase.CreateAsset(asset, name);
            AssetDatabase.SaveAssets();
        }
    }
#endif
}
