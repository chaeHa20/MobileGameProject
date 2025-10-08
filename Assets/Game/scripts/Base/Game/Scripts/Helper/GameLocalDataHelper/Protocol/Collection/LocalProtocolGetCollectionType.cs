using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class LocalProtocolGetCollectionType : GameLocalProtocol
{
    public override void process(Req_LocalData _req, Action<Res_LocalData> callback)
    {
        var req = _req as Req_GetCollectionType;
        var data = getData<LocalCollectionData>(eLocalData.Collection);
        var targetCollections = data.collections.findList(req.type);
        var res = new Res_GetCollectionType
        {
            targetCollections = targetCollections,
        };

        callback(res);
    }
}
