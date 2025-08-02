using System;
using UnityHelper;
// using Google.Protobuf.WellKnownTypes;
// using UnityEditor.SceneManagement;

public class LocalProtocolGetQuest : GameLocalProtocol
{
    public override void process(Req_LocalData _req, Action<Res_LocalData> callback)
    {
        var req = _req as Req_GetQuest;
        
        var res = new Res_GetQuest
        {
            quest = getQuest(req.type),
        };
        
        callback(res);
    }
}
