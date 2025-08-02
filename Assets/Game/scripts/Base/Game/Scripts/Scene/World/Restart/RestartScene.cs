using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartSceneLoadData : GameSceneLoadData
{
    public bool isLoadTable = false;
}

public class RestartScene : GameScene
{
    protected override void initialize()
    {
        base.initialize();

        var sceneLoadData = GameSceneHelper.instance.sceneLoadData as RestartSceneLoadData;

        if (sceneLoadData.isLoadTable)
            GameTableHelper.instance.initialize(AESSettings.instance.table);

        GameSceneHelper.getInstance().loadStartScene(false, false);
    }
}
