using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityHelper;
using System.Collections.Generic;

public class UILobbyMainBottom : UIComponent
{
    [SerializeField] Image m_userThumnail;
    [SerializeField] Image m_dailyGachaIcon;
    [SerializeField] Text m_userName;
    [SerializeField] UICoolTime m_gachaCoolTime;

    public void onClickOpenUserProfileWindow()
    {
        openUserProfileWindow();
    }

    public void onClickOpenMissionWindow()
    {
        openMissionWindow();
    }
    public void onClickOpenCollectionWindow()
    {
        openCollectionWindow();
    }
    public void onClickOpenShopWindow()
    {
        openShopWindow();
    }

    public void onOpenDailyGacha()
    {
        openGacha();
    }

    public void onLoadRunnerScene()
    {
        loadMainScene();
    }

    private void startCoolTime(float duration)
    {

        var coolTime = DateTime.Now.AddSeconds(duration);

        m_gachaCoolTime.start(coolTime, null, StringHelper.getMsCoolTime);
    }

    private void resetGachaCoolTime()
    { 
    }

    private void openUserProfileWindow()
    {
        var datas = new UIGameWindowData
        {
            resourceId = eResource.UIGameUserProfileWindow,
            layer = (int)eUILobbyLayer.Main,
            inactiveCurrent = UIWindowData.eInactiveCurrent.None,
        };

        GameUIHelper.getInstance().openGameWindow<UIGameUserProfileWindow>(datas);

    }

    private void openMissionWindow()
    {
        var datas = new UIGameWindowData
        {
            resourceId = eResource.UIGameMissionWindow,
            layer = (int)eUILobbyLayer.Main,
            inactiveCurrent = UIWindowData.eInactiveCurrent.None,
        };

        GameUIHelper.getInstance().openGameWindow<UIGameMissionWindow>(datas);
    }

    private void openCollectionWindow()
    {
        var datas = new UIGameWindowData
        {
            resourceId = eResource.UIGameCollectionWindow,
            layer = (int)eUILobbyLayer.Main,
            inactiveCurrent = UIWindowData.eInactiveCurrent.None,
        };

        GameUIHelper.getInstance().openGameWindow<UIGameCollectionWindow>(datas);
    }

    private void openShopWindow()
    {
        var datas = new UIGameWindowData
        {
            resourceId = eResource.UIGameShopWindow,
            layer = (int)eUILobbyLayer.Main,
            inactiveCurrent = UIWindowData.eInactiveCurrent.None,
        };

        GameUIHelper.getInstance().openGameWindow<UIGameShopWindow>(datas);
    }

    private void openGacha()
    {
        // 8월 5주차 해당 프리팹 제작 예정
    }

    private void loadMainScene()
    {
        GameSceneHelper.getInstance().loadMainScene();
    }
}
