using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityHelper;

public class GameOptionHelper : NonMonoSingleton<GameOptionHelper>
{
    private bool m_isActiveViveration = false;

    public bool isActiveViveration => m_isActiveViveration;

    public void initialize()
    {
        GameLocalDataHelper.getInstance().requestGetGameOption((gameOption) =>
        {
            setGameOption(gameOption);
        });
    }

    public void setGameOption(LocalGameOption gameOption)
    {
        m_isActiveViveration = gameOption.isViverationOn;
    }
}
