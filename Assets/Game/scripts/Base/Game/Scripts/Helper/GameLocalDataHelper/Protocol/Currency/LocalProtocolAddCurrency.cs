using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class LocalProtocolAddCurrency : GameLocalProtocol
{
    public override void process(Req_LocalData _req, Action<Res_LocalData> callback)
    {
        var req = _req as Req_AddCurrency;

        addCurrency(req.currencyType, req.addValue, out LocalBigMoneyItem currency);

        var res = new Res_AddCurrency
        {
            currency = currency,
        };

        callback(res);
    }
}
