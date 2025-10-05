using System;
using UnityHelper;

public class LocalProtocolShowIntro : GameLocalProtocol
{
    public override void process(Req_LocalData _req, Action<Res_LocalData> callback)
    {
        var userData= getData<LocalUserData>(eLocalData.User);

        userData.showIntorToon();

        var res = new Res_ShowIntro();

        callback(res);
    }
}
