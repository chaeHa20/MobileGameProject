using System;
using UnityEngine;
using UnityHelper;

public class ProtocolLogin : GameLocalProtocol
{
    public override void process(Req_LocalData _req, Action<Res_LocalData> callback)
    {
        var userData = getData<LocalUserData>(eLocalData.User);
        bool isDailyLogin = false;

        var gameSettingTable = getTable<GameSettingTable>(eTable.GameSetting);
        if (userData.isDailyLogin(gameSettingTable.getValueInt(GameSettingRow.eType.ResetTime)))
        {
            isDailyLogin = true;
            GameLocalDataHelper.instance.setDailyReset();
        }

        userData.login(out bool isFirstAppStart);

        if (isFirstAppStart)
        {
            // call default localdata setting code
        }

        
        var res = new Res_Login
        {
            isFirstAppStart = true,
            isDailyLogin = isDailyLogin,
        };

        callback(res);
    }
}
