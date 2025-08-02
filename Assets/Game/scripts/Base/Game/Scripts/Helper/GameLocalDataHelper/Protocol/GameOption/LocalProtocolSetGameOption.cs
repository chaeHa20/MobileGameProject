using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class LocalProtocolSetGameOption : GameLocalProtocol
{
    public override void process(Req_LocalData _req, Action<Res_LocalData> callback)
    {
        var req = _req as Req_SetGameOption;

        var gameData = getData<LocalGameData>(eLocalData.Game);
        gameData.gameOption = req.localGameOption;

        var res = new Res_SetGameOption
        {
            localGameOption = gameData.gameOption,
        };

        callback(res);
    }
}
