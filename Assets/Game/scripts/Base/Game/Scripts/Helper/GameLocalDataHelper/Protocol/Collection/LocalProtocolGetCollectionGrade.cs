using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class LocalProtocolGetCollectionGrade : GameLocalProtocol
{
    public override void process(Req_LocalData _req, Action<Res_LocalData> callback)
    {
        var req = _req as Req_GetCollectionGrade;
        var data = getData<LocalCollectionData>(eLocalData.Collection);
        var targetType = new LocalCollection.CollectionType((int)req.type, req.grade);
        var targetCollections = data.collections.findList(targetType);
        var res = new Res_GetCollectionGrade
        {
            targetCollections = targetCollections,
        };

        callback(res);
    }
}
