using System;
using UnityHelper;

public class LocalProtocolGetTutorial : GameLocalProtocol
{
    public override void process(Req_LocalData _req, Action<Res_LocalData> callback)
    {
        var req = _req as Req_GetTutorial;

        var tutorialData = getData<LocalTutorialData>(eLocalData.Tutorial);

        var res = new Res_GetTutorial
        {
            tutorial = tutorialData.getTutorial(req.tutorialType),
        };

        callback(res);
    }
}
