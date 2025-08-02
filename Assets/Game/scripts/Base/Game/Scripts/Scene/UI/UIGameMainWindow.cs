using UnityEngine;
using UnityHelper;

public class UIGameMainWindowData : UIGameWindowData
{
}

public class UIGameMainWindow : UIGameWindow
{

    protected void showGetCompleteToast(int titleId)
    {
        GameObject parent = UIHelper.instance.canvasGroup.getLastSafeArea();
        string title = StringHelper.get(titleId);
        string msg = StringHelper.get("item_reward_dec", title);

        UIGameToastMsg.create(parent, msg, "");
    }

}
