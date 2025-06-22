using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityHelper;

public class UIStaffRoll : UIComponent
{
    [SerializeField] List<Text> m_infos = new List<Text>();
    [SerializeField] List<Image> m_imgs = new List<Image>();

    private RectTransform m_rectTransform = null;
    private CoroutineHelper.Data m_showCoroutine;
    private CoroutineHelper.Data m_hideCoroutine;

    private void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
        if (null != m_rectTransform)
            m_rectTransform.localScale = new Vector3(m_rectTransform.localScale.x, 0.0f, m_rectTransform.localScale.z);

        foreach (var info in m_infos)
        {
            info.color = new Color(info.color.r, info.color.g, info.color.b, 0.0f);
        }

        foreach (var img in m_imgs)
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0.0f);
        }
    }

    public void showStaffRoll(float waitTime, Action callback)
    {
        var changeType = CoroutineHelper.createTimeType(0.5f);
        m_showCoroutine = CoroutineHelper.instance.start(CoroutineHelper.instance.coChangeValue(0.0f, 1.0f, changeType, (value, done) =>
        {
            foreach (var info in m_infos)
            {
                info.color = new Color(info.color.r, info.color.g, info.color.b, value);
            }

            foreach (var img in m_imgs)
            {
                img.color = new Color(img.color.r, img.color.g, img.color.b, value);
            }

            if (null != m_rectTransform)
                m_rectTransform.localScale = new Vector3(m_rectTransform.localScale.x, value, m_rectTransform.localScale.z);

            if (done)
                waitASecond(waitTime, () =>
                {
                    hideStaffRoll(callback);
                });
        }));
    }

    private void waitASecond(float waitTime, Action callback)
    {
        GameCoroutineHelper.getInstance().wait(waitTime, callback);
    }

    private void hideStaffRoll(Action callback)
    {
        var changeType = CoroutineHelper.createTimeType(0.5f);
        m_hideCoroutine = CoroutineHelper.instance.start(CoroutineHelper.instance.coChangeValue(1.0f, 0.0f, changeType, (value, done) =>
        {
            foreach (var info in m_infos)
            {
                info.color = new Color(info.color.r, info.color.g, info.color.b, value);
            }

            foreach (var img in m_imgs)
            {
                img.color = new Color(img.color.r, img.color.g, img.color.b, value);
            }

            if (null != m_rectTransform)
                m_rectTransform.localScale = new Vector3(m_rectTransform.localScale.x, value, m_rectTransform.localScale.z);

            if (done)
            {
                endStafRoll();
                callback?.Invoke();
            }
        }));
    }

    private void endStafRoll()
    {
        m_showCoroutine.stop();
        m_hideCoroutine.stop();
    }
}
