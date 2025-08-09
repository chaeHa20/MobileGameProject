using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class UIGameGetThrowItemEvent : UIGetItemEvent
{
    private eResource m_resourceId = eResource.None;

    public float speed { get; set; }
    public int iconId { get; set; }
    public eResource resourceId { set { m_resourceId = value; } }

    protected override void createEventObject(Action<Disposable> callback)
    {
        var parent = UIHelper.instance.canvasGroup.getSafeArea((int)eUIMainLayer.OverCurrency);

        UIGetThrowItemEventObject obj = null;
        ResourceRow resourceRow = GameTableHelper.instance.getRow<ResourceRow>((int)eTable.Resource, (int)m_resourceId);
        if (0 < resourceRow.poolId)
        {
            GamePoolHelper.getInstance().pop<UIGetThrowItemEventObject>(m_resourceId, (obj) =>
            {
                if (null == obj)
                {
                    callback?.Invoke(null);
                }
                else
                {
                    obj.setIcon(iconId);
                    
                    GameUIHelper.instance.setParent(parent, obj.gameObject, SetParentOption.notFullAndReset());

                    callback?.Invoke(obj);
                }
            });
        }
        else
        {
            obj = GameResourceHelper.getInstance().instantiate<UIGetThrowItemEventObject>((int)m_resourceId);
            obj.setIcon(iconId);

            GameUIHelper.instance.setParent(parent/*UIHelper.instance.getMainLastSafeArea()*/, obj.gameObject, SetParentOption.notFullAndReset());

            callback?.Invoke(obj);
        }
    }

    protected override BaseMove createMover()
    {
        return null;
    }
}