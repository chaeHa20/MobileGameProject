using UnityEngine;
using UnityHelper;

public class UIGameEasterEggWindow : UIGameWindow
{
    [SerializeField] GameObject m_titleEn = null;
    [SerializeField] GameObject m_titleJp = null;
    [SerializeField] GameObject m_titleKr = null;

    [SerializeField] GameObject m_contentEn = null;
    [SerializeField] GameObject m_contentJp = null;
    [SerializeField] GameObject m_contentKr = null;

    public override void initialize(UIWidgetData data)
    {
        base.initialize(data);

        checkLanguage();
    }

    private void checkLanguage()
    {
        var language = (eLanguage)GamePlayerPrefsHelper.instance.getInt(PlayerPrefsKey.Language, (int)eLanguage.Kr);
        if (eLanguage.Kr == language)
        {
            language = LanguageHelper.getDeviceLanguage();
        }

        if (eLanguage.Kr == language)
        {
            m_titleEn.SetActive(false);
            m_titleJp.SetActive(false);
            m_titleKr.SetActive(true);

            m_contentEn.SetActive(false);
            m_contentJp.SetActive(false);
            m_contentKr.SetActive(true);
        }
        else if (eLanguage.Jp == language)
        {
            m_titleEn.SetActive(false);
            m_titleJp.SetActive(true);
            m_titleKr.SetActive(false);

            m_contentEn.SetActive(false);
            m_contentJp.SetActive(true);
            m_contentKr.SetActive(false);
        }
        else
        {
            m_titleEn.SetActive(true);
            m_titleJp.SetActive(false);
            m_titleKr.SetActive(false);

            m_contentEn.SetActive(true);
            m_contentJp.SetActive(false);
            m_contentKr.SetActive(false);
        }
    }
}
