using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityHelper;

public class UILanguage : MonoBehaviour
{
    [SerializeField] Text m_language = null;

    private void OnEnable()
    {
        m_language.text = StringHelper.get(LanguageHelper.language);
    }
}
