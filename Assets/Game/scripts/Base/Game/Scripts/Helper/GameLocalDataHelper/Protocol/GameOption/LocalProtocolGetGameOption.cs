using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class LocalProtocolGetGameOption : GameLocalProtocol
{
    public override void process(Req_LocalData _req, Action<Res_LocalData> callback)
    {
        var gameData = getGameData();

        var res = new Res_GetGameOption
        {
            localGameOption = gameData.gameOption,
        };

        callback(res);
    }
}
