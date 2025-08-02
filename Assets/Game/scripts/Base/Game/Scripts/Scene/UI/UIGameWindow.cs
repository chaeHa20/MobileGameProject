using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class UIGameWindowData : UIWindowData
{
    public eResource resourceId;
}

public class UIGameWindow : UIWindow
{
    [SerializeField] bool m_isIgnoreBackButton = false;
    [SerializeField] eSfx m_openSfxId = eSfx.None;

    public bool isIgnoreBackButton => m_isIgnoreBackButton;

    public override void open()
    {
        base.open();

        playOpenSfx();
    }

    private void playOpenSfx()
    {
        if (eSfx.None != m_openSfxId)
            GameSoundHelper.getInstance().playShare((int)m_openSfxId);
    }
}
