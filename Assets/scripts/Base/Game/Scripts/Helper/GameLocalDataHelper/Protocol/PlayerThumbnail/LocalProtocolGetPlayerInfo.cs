using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class LocalProtocolGetPlayerInfo : GameLocalProtocol
{
    public override void process(Req_LocalData _req, Action<Res_LocalData> callback)
    {
        var gameData = getData<LocalGameData>(eLocalData.Game);
        var playerData = getData<LocalPlayerData>(eLocalData.Player);

        var res = new Res_GetPlayerInfo
        {
            playerThumbnailId = gameData.playerThumbnailId,
            playerData = playerData,
        };

        callback(res);
    }
}
