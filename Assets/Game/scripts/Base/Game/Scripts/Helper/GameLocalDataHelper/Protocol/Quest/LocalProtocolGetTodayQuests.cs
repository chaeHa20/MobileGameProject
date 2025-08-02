using NUnit.Framework;
using System;
using UnityHelper;
// using Google.Protobuf.WellKnownTypes;
// using UnityEditor.SceneManagement;

public class LocalProtocolGetTodayQuests : GameLocalProtocol
{
    public override void process(Req_LocalData _req, Action<Res_LocalData> callback)
    {
        var req = _req as Req_GetAllQuests;

        var questData = getData<LocalQuestData>(eLocalData.Quest);

        var res = new Res_GetAllQuests
        {
            quests = questData.getTodayQuests(),
            isOpenQuest = questData.IsQuestOpen,
        };
        
        callback(res);
    }
}
