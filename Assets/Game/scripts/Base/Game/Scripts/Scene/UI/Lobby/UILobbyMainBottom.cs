using System;
using UnityEngine;
using UnityEngine.UI;
using UnityHelper;

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
        // 8월 5주차 해당 프리팹 제작 예정
    }

    private void openMissionWindow()
    {
        // 8월 5주차 해당 프리팹 제작 예정
    }

    private void openCollectionWindow()
    {
        // 8월 5주차 해당 프리팹 제작 예정
    }

    private void openShopWindow()
    {
        // 8월 5주차 해당 프리팹 제작 예정
    }

    private void openGacha()
    {
        // 8월 5주차 해당 프리팹 제작 예정
    }
}
