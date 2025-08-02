using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class LocalProtocolGetCurrency : GameLocalProtocol
{
    public override void process(Req_LocalData _req, Action<Res_LocalData> callback)
    {
        var res = new Res_GetCurrency
        {
            gold = getCurrency(eCurrency.Gold),
            gem = getCurrency(eCurrency.Gem),
        };

        callback(res);
    }
}
