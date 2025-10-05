using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class LobbyScene : GameScene
{
    [SerializeField] UILobbyScene m_ui;

    protected override void initialize()
    {
        base.initialize();

        if (null != m_ui)
            m_ui.checkIntroToonWindow();
    }
}
