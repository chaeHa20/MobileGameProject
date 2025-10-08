using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class LocalProtocolGetCollections : GameLocalProtocol
{
    public override void process(Req_LocalData _req, Action<Res_LocalData> callback)
    {
        var data = getData<LocalCollectionData>(eLocalData.Collection);

        var res = new Res_GetCollections
        {
            collections = data.collections,
        };

        callback(res);
    }
}
