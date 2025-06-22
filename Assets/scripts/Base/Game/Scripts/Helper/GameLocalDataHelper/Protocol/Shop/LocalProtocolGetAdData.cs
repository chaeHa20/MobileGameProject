using System;
using UnityHelper;

public class LocalProtocolGetAdData : GameLocalProtocol
{
    public override void process(Req_LocalData _req, Action<Res_LocalData> callback)
    {
        var res = new Res_GetLocalAd
        {
            adData = getData<LocalAdData>(eLocalData.Ad),
        };

        callback(res);
    }
}
