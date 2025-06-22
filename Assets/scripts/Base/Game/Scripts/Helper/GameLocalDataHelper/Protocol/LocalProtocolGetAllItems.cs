using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class LocalProtocolGetAllItems : GameLocalProtocol
{
    public override void process(Req_LocalData _req, Action<Res_LocalData> callback)
    {
        var itemData = getData<LocalItemData>(eLocalData.Item);
        var currencyData = getData<LocalCurrencyData>(eLocalData.Currency);


        var res = new Res_GetAllIems
        {
            gem = currencyData.getCurrency(eCurrency.Gem),
            gold = currencyData.getCurrency(eCurrency.Gold),
            items = itemData.items,
        };

        callback(res);
    }
}
