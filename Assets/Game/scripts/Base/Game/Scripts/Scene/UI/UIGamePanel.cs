using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class UIGamePanelData : UIPanelData
{
    public eResource resourceId;
}

public class UIGamePanel : UIPanel
{
    [SerializeField] eSfx m_openSfxId = eSfx.None;

    public override void open()
    {
        base.open();

        playOpenSfx();
    }

    protected void playOpenSfx()
    {
        if (eSfx.None != m_openSfxId)
            GameSoundHelper.getInstance().playShare((int)m_openSfxId);
    }
}
