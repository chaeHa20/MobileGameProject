using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class UIEmptySceneLoader : UISceneLoader
{
    [SerializeField] GameObject m_biKr = null;
    [SerializeField] GameObject m_biEn = null;
    [SerializeField] GameObject m_biJp = null;

    private void Start()
    {
        var language = (eLanguage)GamePlayerPrefsHelper.instance.getInt(PlayerPrefsKey.Language, (int)eLanguage.Kr);
        if (eLanguage.Kr == language)
        {
            m_biKr.gameObject.SetActive(true);
            m_biEn.gameObject.SetActive(false);
            m_biJp.SetActive(false);
        }
        else if (eLanguage.Jp == language)
        {
            m_biKr.SetActive(false);
            m_biEn.SetActive(false);
            m_biJp.SetActive(true);
        }
        else
        {
            m_biKr.gameObject.SetActive(false);
            m_biEn.gameObject.SetActive(true);
            m_biJp.SetActive(false);
        }
    }
}
