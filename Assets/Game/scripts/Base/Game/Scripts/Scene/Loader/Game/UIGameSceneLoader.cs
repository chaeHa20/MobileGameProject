using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityHelper;

public class UIGameSceneLoader : UISceneLoader
{
    [SerializeField] GameObject m_debug = null;
    

    protected virtual void Start()
    {
        if (null != m_debug)
            m_debug.SetActive(Debugx.isActive);
    }

    public virtual void setActiveDefeatDefenseTip(bool isActive)
    {
    }

    protected override void destroy()
    {
        base.destroy();
    }
}
