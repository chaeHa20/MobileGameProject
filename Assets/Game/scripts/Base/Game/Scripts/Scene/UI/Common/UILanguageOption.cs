using UnityEngine;
using UnityHelper;

public class UILanguageOption : MonoBehaviour
{
    [SerializeField] UILanguageOptionTemplete m_templete;

#if UNITY_IOS
    private eLanguage m_language = eLanguage.None;

    private void OnEnable()
    {
        if(null != m_templete)
            m_templete.addLanguageOption(this);
    }

    public void setLanguage(eLanguage lan)
    {
        m_language = lan;
    }

    private void Update()
    {
        if (eLanguage.Th == m_language && gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }
#endif
}
