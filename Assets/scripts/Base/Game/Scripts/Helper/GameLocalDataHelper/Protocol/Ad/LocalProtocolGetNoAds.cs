using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;
using System;

public class LocalProtocolGetNoAds : GameLocalProtocol
{
    public override void process(Req_LocalData _req, Action<Res_LocalData> callback)
    {
        var adData = getData<LocalAdData>(eLocalData.Ad);

        var res = new Res_GetNoAds
        {
            isNoAds = adData.isNoAds,
        };

        callback(res);
    }
}
