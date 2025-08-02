using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class LocalProtocolSetSocialId : GameLocalProtocol
{
    public override void process(Req_LocalData _req, Action<Res_LocalData> callback)
    {
        var req = _req as Req_SetSocialId;
        var playerData = getData<LocalPlayerData>(eLocalData.Player);
        playerData.setPlayerSocialId(req.id);

        var res = new Res_SetSocialId
        {
        };

        callback(res);
    }
}
