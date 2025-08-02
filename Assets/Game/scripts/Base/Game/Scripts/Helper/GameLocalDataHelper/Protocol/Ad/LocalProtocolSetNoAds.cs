using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;
using System;

public class LocalProtocolSetNoAds : GameLocalProtocol
{
    public override void process(Req_LocalData _req, Action<Res_LocalData> callback)
    {
        var adData = getData<LocalAdData>(eLocalData.Ad);
        adData.isNoAds = true;

        callback(new Res_SetNoAds());
    }
}
