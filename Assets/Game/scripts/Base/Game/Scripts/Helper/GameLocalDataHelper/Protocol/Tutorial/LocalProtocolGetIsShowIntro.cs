using System;
using UnityHelper;

public class LocalProtocolGetIsShowIntro : GameLocalProtocol
{
    public override void process(Req_LocalData _req, Action<Res_LocalData> callback)
    {
        var userData= getData<LocalUserData>(eLocalData.User);

        

        var res = new Res_GetIsShowIntro
        {
            isShowIntro = userData.isShowIntroToon,
        };

        callback(res);
    }
}
