using System;
using UnityHelper;
// using Google.Protobuf.WellKnownTypes;
// using UnityEditor.SceneManagement;

public class LocalProtocolUpdateQuest : GameLocalProtocol
{
    public override void process(Req_LocalData _req, Action<Res_LocalData> callback)
    {
        var req = _req as Req_UpdateQuest;

        bool isUpdate = false;
        bool isClear = false;

        var questData = getData<LocalQuestData>(eLocalData.Quest);
        if (questData.IsQuestOpen)
        {
            checkQuest(req.type, req.addValue, ref isUpdate);
            isClearQuest(req.type, ref isClear);
        }

        var res = new Res_UpdateQuest
        {
            isUpdateQuest = isUpdate,
            isClearQuest = isClear,
        };

        callback(res);
    }
}
