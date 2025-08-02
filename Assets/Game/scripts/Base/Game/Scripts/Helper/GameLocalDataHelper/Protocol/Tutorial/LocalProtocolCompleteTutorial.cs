using System;
using UnityHelper;

public class LocalProtocolCompleteTutorial : GameLocalProtocol
{
    public override void process(Req_LocalData _req, Action<Res_LocalData> callback)
    {
        var req = _req as Req_CompleteTutorial;

        var tutorialData = getData<LocalTutorialData>(eLocalData.Tutorial);
        var localTutorial = tutorialData.getTutorial(req.tutorialType);
        if (null != localTutorial)
            localTutorial.setCompleteTutorial();

        var res = new Res_CompleteTutorial();

        callback(res);
    }
}
