using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class UIRunnerSceneLoader : UISceneLoader
{
    [SerializeField] GameObject m_debug = null;
    
    protected virtual void Start()
    {
        if (null != m_debug)
            m_debug.SetActive(Debugx.isActive);
    }

    protected override void destroy()
    {
        base.destroy();
    }
}
