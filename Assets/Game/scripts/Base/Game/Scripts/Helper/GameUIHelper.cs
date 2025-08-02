using UnityHelper;

public class GameUIHelper : UIHelper
{
    public static GameUIHelper getInstance()
    {
        return getInstance<GameUIHelper>();
    }

    public T openGameWindow<T>(UIGameWindowData data, bool isMain = false) where T : UIGameWindow
    {
        if (Logx.isActive)
            Logx.assert(null != data, "data is null");

        if (isCurrentWindow(data.resourceId.ToString()))
            return null;

        data.name = data.resourceId.ToString();
        data.resPath = GameTableHelper.instance.getResourcePath((int)data.resourceId);
        return openWindow<T>(data, isMain);
    }

    public T openGameWindow<T>(eResource resourceId, int layer) where T : UIGameWindow
    {
        var data = new UIGameWindowData
        {
            resourceId = resourceId,
            layer = layer,
        };

        return openGameWindow<T>(data);
    }

    public T openGamePanel<T>(UIGamePanelData data) where T : UIGamePanel
    {
        if (Logx.isActive)
            Logx.assert(null != data, "data is null");

        data.name = data.resourceId.ToString();
        data.resPath = GameTableHelper.instance.getResourcePath((int)data.resourceId);
        return openPanel<T>(data);
    }

    protected override string messageIdToStr(int messageId)
    {
        return ((eUIMessage)messageId).ToString();
    }

    /// <param name="disposeCallback">is show window</param>
    //public void openOfflineGoldWindow(BigInteger offlineGold, int minTime, int maxTime, int offlineTime, Action<bool> disposeCallback)
    //{
    //    closeCurrentWindow(0.0f);

    //    var data = new UIOfflineGoldWindowData
    //    {
    //        minOfflineTime = minTime,
    //        maxOfflineTime = maxTime,
    //        offlineGold = offlineGold,
    //        offlineTime = offlineTime,
    //        resourceId = eResource.UIOfflineGoldWindow,
    //        layer = (int)getSceneMediumLayerByAuto(),
    //        inactiveCurrent = UIWindowData.eInactiveCurrent.None,
    //        disposeCallback = disposeCallback
    //    };

    //    openGameWindow<UIOfflineGoldWindow>(data);
    //}


    public int getSceneMediumLayerByAuto()
    {
        int layer = 0;
        layer = (int)eUIMainLayer.Main;

        return layer;
    }

    public override bool backWindow()
    {
        var currentWindow = getCurrentWindow<UIGameWindow>();
        if (null != currentWindow)
        {
            if (currentWindow.isIgnoreBackButton)
                return false;
        }

        return false;
    }

    protected override void OnApplicationPause(bool pauseStatus)
    {
        // pause 처리는 GameSimulator에서 하도록 하자
    }
}