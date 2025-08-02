using UnityEngine;
using UnityHelper;

public class UIGameHud : UIHud
{
    public static T create<T>(GameObject parent, eResource resourceId) where T : UIGameHud
    {
        var hud = GameResourceHelper.getInstance().instantiate<T>((int)resourceId);
        UIHelper.instance.setParent(parent, hud.gameObject, SetParentOption.notFullAndReset());
        return hud;
    }

    public virtual void initialize(GameObject positionObject)
    {
        var canvas = UIHelper.instance.canvasGroup.getFirstCanvas().canvas;
        base.initialize(positionObject, canvas, Camera.main);
    }

    public virtual void initializeMain(GameObject positionObject)
    {
        var canvas = UIHelper.instance.canvasGroup.getCanvas((int)eUIRestaurantLayer.Main).canvas;
        base.initialize(positionObject, canvas, Camera.main);
    }

    public void initializeTutorialNavigation(eUIRestaurantLayer layer, GameObject positionObject)
    {
        var canvas = UIHelper.instance.canvasGroup.getCanvas((int)layer).canvas;
        base.initialize(positionObject, canvas, Camera.main);
    }


    public void refresh(GameObject positionObject)
    {
        var canvas = UIHelper.instance.canvasGroup.getFirstCanvas().canvas;
        startSyncPosition(positionObject, canvas, Camera.main, false);
    }

    public void setAutoRectSizeDependingOnCameraOrthographicSize(ref RectTransform rect)//, ref RectTransform notificationRect)
    {
        var firstStageSize = 1.0f;
        var currentStageSize = 0.0f;
        foreach (var cameraSetting in GameSettings.instance.cameraTransformSettings)
        {
            if ((cameraSetting.stageId / 100) == 1)
            {
                firstStageSize = cameraSetting.size;
            }
        }


        var cameraSizeRatio = firstStageSize / currentStageSize;

        Vector2 rtSizeDelta = rect.sizeDelta;
        rtSizeDelta.x = rect.sizeDelta.x;
        rtSizeDelta.y = rect.sizeDelta.y * cameraSizeRatio;
        rect.sizeDelta = rtSizeDelta;
    }

    public void setAutoRectPositionDependingScreen(ref RectTransform rect)
    {
        var curScreenRatio = (float)Screen.height / (float)Screen.width;
        var oriScreenRatio = 16.0f / 9.0f;
        var screenRatio = curScreenRatio / oriScreenRatio;
        var screenHeightSubValue = rect.sizeDelta.y * (1.0f - screenRatio);

        if (screenHeightSubValue <= 0)
        {
            rect.anchoredPosition =
            new Vector2(rect.anchoredPosition.x
                      , rect.anchoredPosition.y - screenHeightSubValue);
        }
        else
        {
            rect.anchoredPosition =
            new Vector2(rect.anchoredPosition.x
                      , rect.anchoredPosition.y + screenHeightSubValue);
        }
    }
}
