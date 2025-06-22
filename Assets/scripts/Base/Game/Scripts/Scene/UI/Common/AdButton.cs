using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityHelper;

public class AdButton : MonoBehaviour
{
    [SerializeField] UICoolTime m_coolTime = null;
    [SerializeField] GameObject m_text = null;
    [SerializeField] Image m_btnBg = null;
    [SerializeField] Image m_icon = null;
    [SerializeField] GameObject m_coolTimeBg = null;

    private Color oriBgColor { get; set; }
    private Color oriIconColor { get; set; }

    private void Awake()
    {
        setOriColor();
    }

    public void setCoolTime(long coolTime)
    {
        gameObject.SetActive(true);

        m_coolTime.gameObject.SetActive(false);
        m_coolTimeBg.gameObject.SetActive(false);
        m_text.gameObject.SetActive(false);

        if (TimeHelper.isCoolTime(coolTime))
        {
            setDisableColor();
            m_coolTimeBg.gameObject.SetActive(true);
            m_coolTime.gameObject.SetActive(true);

            m_coolTime.start(coolTime, () =>
            {
                setCoolTime(0);
            });
        }
        else
        {
            restoreOriColor();
            m_text.gameObject.SetActive(true);
        }
    }

    private void setOriColor()
    {
        oriBgColor = m_btnBg.color;
        oriIconColor = m_icon.color;
    }

    private void restoreOriColor()
    {
        m_btnBg.color = oriBgColor;
        m_icon.color = oriIconColor;
    }

    private void setDisableColor()
    {
        m_btnBg.color = Color.gray;
        m_icon.color = Color.gray;
    }

    public bool isCoolTime()
    {
        return m_coolTime.isCoolTime();
    }
}
