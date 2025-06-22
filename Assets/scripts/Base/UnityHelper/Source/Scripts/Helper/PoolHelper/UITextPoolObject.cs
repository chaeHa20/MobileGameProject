using System.Collections.Generic;
using System.Xml.Serialization;
using UnityHelper;

public class UITextPoolObject : PoolObject
{
    private eLanguage m_lang = eLanguage.None;
    private void Awake()
    {
        m_lang = LanguageHelper.language;
    }

    private void OnEnable()
    {
        setLanguage();
    }

    private void setLanguage()
    {
        if (LanguageHelper.language == m_lang)
            return;

        updateLanguageCallback();
        m_lang = LanguageHelper.language;
    }

    private void updateLanguageCallback()
    {
        var stringDBAppliers = GetComponentsInChildren<UIApplyDBString>();
        if (null == stringDBAppliers)
            return;

        foreach (var stringDBApplier in stringDBAppliers)
        {
            if (null != stringDBApplier)
                stringDBApplier.updateLanguage();
        }
    }
}
