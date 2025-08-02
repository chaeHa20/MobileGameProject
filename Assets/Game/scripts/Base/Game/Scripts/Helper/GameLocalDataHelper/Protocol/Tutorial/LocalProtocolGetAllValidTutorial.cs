using System;
using UnityHelper;

public class LocalProtocolGetAllValidTutorial : GameLocalProtocol
{
    public override void process(Req_LocalData _req, Action<Res_LocalData> callback)
    {
        var req = _req as Req_GetAllTutorials;

        var tutorialData = getData<LocalTutorialData>(eLocalData.Tutorial);        

        var res = new Res_GetAllTutorials
        {
            // tutorials = tutorialData.findTutorialsInStage(mapId, stageId),
        };

        callback(res);
    }
}
