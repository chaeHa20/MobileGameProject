using UnityHelper;
using System;
using UnityEngine;
using UnityEngine.UI;
using JetBrains.Annotations;

public class UIPlayerHealthItem : PoolObject
{
    [SerializeField] Image m_activeIcon;
    [SerializeField] Image m_deactiveIcon;

    private int m_index = -1;

    public int index   => m_index;

    public static void pop(GameObject parent,int index, Action<UIPlayerHealthItem> callback)
    {
        pop(parent, index, eResource.UIPlayerHealthItem, callback);
    }

    private static void pop(GameObject parent, int index, eResource resourceId, Action<UIPlayerHealthItem> callback)
    {
        GamePoolHelper.getInstance().pop<UIPlayerHealthItem>(resourceId, (item) =>
        {
            UIHelper.instance.setParent(parent, item.gameObject, SetParentOption.fullAndReset());

            item.initialize(index);

            callback?.Invoke(item);
        });
    }

    public void initialize(int index)
    {
        m_index = index;
        setActiveItem();
    }

    public void restoreItem()
    {
        m_activeIcon.gameObject.SetActive(true);
        runHealthRestore(0.5f, () =>
        {
            m_deactiveIcon.gameObject.SetActive(false);
        });        
    }

    public void setDamageItem()
    {
        m_deactiveIcon.gameObject.SetActive(true);
        runHealthDamage(0.5f, () =>
        {
            m_activeIcon.gameObject.SetActive(false);
        });
    }

    public void setActiveItem()
    {
        m_activeIcon.gameObject.SetActive(true);
        m_deactiveIcon.gameObject.SetActive(false);
    }

    public void setDeactiveItem()
    {
        m_activeIcon.gameObject.SetActive(false);
        m_deactiveIcon.gameObject.SetActive(true);
    }

    private void runHealthRestore(float delay,Action callback)
    {
        var changeType = CoroutineHelper.createTimeType(delay);
        GameCoroutineHelper.getInstance().changeValue(0.0f, 1.0f, changeType, (value, done) =>
        {
            m_activeIcon.color = new Color(m_activeIcon.color.r, m_activeIcon.color.g, m_activeIcon.color.b, value);
            if(done)
            {
                callback?.Invoke();
            }
        });
    }

    private void runHealthDamage(float delay, Action callback)
    {
        var changeType = CoroutineHelper.createTimeType(delay);
        GameCoroutineHelper.getInstance().changeValue(1.0f, 0.0f, changeType, (value, done) =>
        {
            m_activeIcon.color = new Color(m_activeIcon.color.r, m_activeIcon.color.g, m_activeIcon.color.b, value);
            if (done)
            {
                callback?.Invoke();
            }
        });
    }

    protected override void Dispose(bool disposing)
    {
        m_index = -1;
        base.Dispose(disposing);
    }
}

