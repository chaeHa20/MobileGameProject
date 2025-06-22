using UnityEngine;
using UnityHelper;
using System;

public class UILogoScene : UIGameScene
{
    [SerializeField] GameObject m_biKr = null;
    [SerializeField] GameObject m_biEn = null;
    [SerializeField] GameObject m_biJp = null;

    public void initBi()
    {
        //var language = LanguageHelper.getDeviceLanguage();
        var language = (eLanguage)GamePlayerPrefsHelper.instance.getInt(PlayerPrefsKey.Language, (int)eLanguage.Kr);
        if (eLanguage.Kr == language)
        {
            language = LanguageHelper.getDeviceLanguage();
        }

        if (eLanguage.Kr == language)
        {
            m_biKr.SetActive(true);
            m_biEn.SetActive(false);
            m_biJp.SetActive(false);
        }
        else if(eLanguage.Jp == language)
        {
            m_biKr.SetActive(false);
            m_biEn.SetActive(false);
            m_biJp.SetActive(true);
        }
        else
        {
            m_biKr.gameObject.SetActive(false);
            m_biEn.gameObject.SetActive(true);
            m_biJp.gameObject.SetActive(false);
        }
    }
}
