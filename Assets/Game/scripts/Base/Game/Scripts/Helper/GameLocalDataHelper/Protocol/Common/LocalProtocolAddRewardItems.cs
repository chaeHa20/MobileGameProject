using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class LocalProtocolAddRewardItems : GameLocalProtocol
{
    public override void process(Req_LocalData _req, Action<Res_LocalData> callback)
    {
        var req = _req as Req_AddRewardItems;
        var addItemResult = new LocalAddItemResult();

        foreach(var pair in req.rewardItems)
        {
            addItem(pair, 1, req.isShowAd, addItemResult);
        }

        var res = new Res_AddRewardItems
        {
            addItemResult = addItemResult,
        };

        callback(res);
    }
}
