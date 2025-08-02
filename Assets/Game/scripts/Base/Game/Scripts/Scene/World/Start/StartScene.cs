using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class StartSceneLoadData : GameSceneLoadData
{
    public bool isAppStart;
}

public class StartScene : GameScene
{
    protected override void initialize()
    {
        base.initialize();

        //string deviceId = SystemInfo.deviceUniqueIdentifier;
        //Debug.Log("Test Device Unique ID: " + deviceId);

        requestLogin();
        setQualitySettings();
    }

    private void requestLogin()
    {
        var req = new Req_Login();
        GameLocalDataHelper.instance.request<Res_Login>(req, (res) =>
        {
            if (res.isSuccess)
            {
                // var loadData = GameSceneHelper.instance.sceneLoadData as StartSceneLoadData;
                GameSceneHelper.getInstance().loadLobbyScene();
            }
        });
    }
}
