using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityHelper;
using System;

public class UIGameToastMsg : PoolObject
{
    private enum eMotion { Open, Close };

    [SerializeField] Text m_title = null;
    [SerializeField] Text m_msg = null;
    [SerializeField] Animator m_animator = null;

    private Action m_disposeCallback = null;
    private float m_showTime = 0.1f;

    public static void create(GameObject parent, string title, string msg, float showTime = 1.0f, Action disposeCallback = null, Action<UIGameToastMsg> callback = null)
    {
        GamePoolHelper.getInstance().pop<UIGameToastMsg>(eResource.UIGameToastMsg, (t) =>
        {
            if (null != t)
            {
                t.initialize(parent, title, msg, showTime, disposeCallback);
            }

            callback?.Invoke(t);
        });
    }

    public static void create(string title, string msg, int layer, float showTime = 3.0f, Action disposeCallback = null, Action<UIGameToastMsg> callback = null)
    {
        GameObject parent = UIHelper.instance.canvasGroup.getSafeArea(layer);
        create(parent, title, msg, showTime, disposeCallback, callback);
    }

    public static void create(string title, string msg, float showTime = 3.0f, Action disposeCallback = null, Action<UIGameToastMsg> callback = null)
    {
        GameObject parent = UIHelper.instance.canvasGroup.getLastSafeArea();
        create(parent, title, msg, showTime, disposeCallback, callback);
    }

    public static void createClearQuest()
    {
        create(null, StringHelper.get("clear_quest"), 1.5f);
    }

    public static void createNotEnoughCurrency(eCurrency currencyType)
    {
        // GameSoundHelper.getInstance().playShare((int)eSfx.not_engough_currency);// TODO : 2024-07-31 by pms

        var msg = StringHelper.toNotEnoughCurrency(currencyType);
        create(null, msg, 1.5f);
    }

    public static void createNotEnoughCurrency(eBuyType buyType)
    {
        switch(buyType)
        {
            case eBuyType.Gem: createNotEnoughCurrency(eCurrency.Gem); break;
        }
    }

    protected void initialize(GameObject parent, string title, string msg, float showTime, Action disposeCallback)
    {
        UIHelper.instance.setParent(parent, gameObject, SetParentOption.fullAndReset());

        m_disposeCallback = disposeCallback;
        setMsg(title, msg, showTime);
    }

    public void setMsg(string title, string msg, float showTime)
    {
        StopAllCoroutines();

        m_title.text = title;
        m_msg.text = msg;
        m_showTime = showTime;

        m_title.gameObject.SetActive(!String.IsNullOrEmpty(title));
        m_msg.gameObject.SetActive(!String.IsNullOrEmpty(msg));

        playMotion(eMotion.Open);
    }

    private void playMotion(eMotion motion)
    {
        m_animator.Play(motion.ToString(), 0, 0.0f);
    }

    public void onAnimationEventOpenEnd()
    {
        StartCoroutine(coPlayCloseMotion());
    }

    public void onAnimationEventCloseEnd()
    {
        Dispose();
    }

    IEnumerator coPlayCloseMotion()
    {
        yield return new WaitForSeconds(m_showTime);

        playMotion(eMotion.Close);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        m_disposeCallback?.Invoke();
    }
}
