using UnityEngine;
using UnityHelper;

public class UILobbyScene : UIGameScene
{

    public void openIntroToonWindow()
    {
        var data = new UIGameIntroToonWindowData
        {
            resourceId = eResource.UIGameIntroToonWindow,
            layer = (int)eUILobbyLayer.Main,
            inactiveCurrent = UIWindowData.eInactiveCurrent.None,
            lastIndex = 0,
        };
        GameUIHelper.getInstance().openGameWindow<UIGameIntroToonWindow>(data, true);
    }
}
