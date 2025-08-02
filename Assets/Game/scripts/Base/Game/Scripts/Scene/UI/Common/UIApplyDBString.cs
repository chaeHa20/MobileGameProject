using UnityEngine;
using UnityEngine.UI;
using UnityHelper;

public class UIApplyDBString : MonoBehaviour
{
    [SerializeField] string m_stringCode = null;
    [SerializeField] bool m_isUpper = false;

    private void Awake()
    {
        applyDBString();
    }

    public void updateLanguage()
    {
        applyDBString();
    }

    private void applyDBString()
    {
        var text = GetComponent<Text>();
        if (null == text)
        {
            if (Logx.isActive)
                Logx.error("{0} TMP_Text is null", name);

            return;
        }

        if (string.IsNullOrEmpty(m_stringCode))
        {
            if (Logx.isActive)
                Logx.error("{0} string code is null", name);

            return;
        }

        var str = StringHelper.get(m_stringCode);
        if (null != text)
            text.text = m_isUpper ? str.ToUpper() : str;
    }
}
