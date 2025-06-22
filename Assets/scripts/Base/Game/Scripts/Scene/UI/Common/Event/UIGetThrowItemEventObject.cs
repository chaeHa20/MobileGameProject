using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityHelper;

public class UIGetThrowItemEventObject : PoolObject
{
    [SerializeField] Image m_icon = null;

    public void setIcon(int iconId)
    {
        if (null == m_icon)
            return;
        if (0 == iconId)
            return;

        m_icon.sprite = GameResourceHelper.getInstance().getSprite(iconId);
    }
}
