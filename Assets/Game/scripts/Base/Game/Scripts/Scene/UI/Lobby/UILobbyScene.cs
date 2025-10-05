using UnityEngine;
using UnityHelper;

public class UILobbyScene : UIGameScene
{

    public void checkIntroToonWindow()
    {
        var req = new Req_GetIsShowIntro();

        GameLocalDataHelper.instance.request<Res_GetIsShowIntro>(req, (res) =>
        {
            if(res.isSuccess && !res.isShowIntro)
            {
                openIntroToonWindow();
            }
        });
    }

    private void openIntroToonWindow()
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


