using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class GameVibrationHelper : VibrationHelper
{
    public static GameVibrationHelper getInstance()
    {
        return getInstance<GameVibrationHelper>();
    }

    public override void initialize()
    {
        base.initialize();
    }

}
