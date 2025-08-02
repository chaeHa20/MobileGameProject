using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class UILanguageOptionTemplete : MonoBehaviour
{
#if UNITY_IOS

    private List<UILanguageOption> m_options = new List<UILanguageOption>();
    private int m_oldListCount = 1;

    public void addLanguageOption(UILanguageOption option)
    {
        var list = GetComponentsInChildren<UILanguageOption>();
        if (m_oldListCount < list.Length)
        {
            m_oldListCount = list.Length;
            m_options.Add(option);
            option.setLanguage((eLanguage)m_options.Count);
        }
    }
#endif
}
