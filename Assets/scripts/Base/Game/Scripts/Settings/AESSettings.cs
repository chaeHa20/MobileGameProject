using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "App/AESSettings instance")]
public class AESSettings : BaseAESSettings
{
    private static AESSettings m_instance = null;

    public static AESSettings instance
    {
        get
        {
            if (null == m_instance)
            {
                m_instance = Resources.Load<AESSettings>("Settings/AESSettings");
            }

            return m_instance;
        }
    }

#if UNITY_EDITOR
    [MenuItem("Settings/AESSettings/Settings")]
    static void Settings()
    {
        Selection.activeObject = instance;
    }

    public static void create()
    {
        if (null == instance)
        {
            var asset = ScriptableObject.CreateInstance<AESSettings>();

            var name = UnityEditor.AssetDatabase.GenerateUniqueAssetPath("Assets/Game/Resources/Settings/AESSettings.asset");
            AssetDatabase.CreateAsset(asset, name);
            AssetDatabase.SaveAssets();
        }
    }
#endif
}
