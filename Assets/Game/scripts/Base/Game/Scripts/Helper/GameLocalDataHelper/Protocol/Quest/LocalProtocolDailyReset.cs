using System;
using UnityHelper;
// using Google.Protobuf.WellKnownTypes;
// using UnityEditor.SceneManagement;

public class LocalProtocolDailyReset : GameLocalProtocol
{
    public override void process(Req_LocalData _req, Action<Res_LocalData> callback)
    {
        var req = _req as Req_DailyReset;

        var userData = getData<LocalUserData>(eLocalData.User);
        //var gameSettingTable = getTable<GameSettingTable>(eTable.GameSetting);
        //if (userData.isDailyLogin(gameSettingTable.getValueInt(GameSettingRow.eType.ResetTime)))
        //{
        //    GameLocalDataHelper.instance.setDailyReset();
        //    GameLocalDataHelper.instance.saveUsedDatas();
        //}

        userData.login(out bool isFirstAppStart);

        var res = new Res_DailyReset();

        callback(res);
    }
}
