using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

// TODO : 2023-12-22 update by pms
public class GameFirebaseHelper : NonMonoSingleton<GameFirebaseHelper>
{
    public bool isInitialized => FirebaseHelper.instance.isInitialized;

    public void initialize()
    {
        FirebaseHelper.instance.initialize();
    }

    private void logEvent(string eventName)
    {
        FirebaseHelper.instance.logEvent(eventName);
    }

    private void logEvent(string EventName, string parameter_name, string parameter_value)
    {
        FirebaseHelper.instance.logEvent(EventName, parameter_name, parameter_value);
    }

    public void logGameEvent(string desc)
    {
        logEvent(desc);
    }

    public void logUseMainBooster(int stageId)
    {
        string desc = string.Format("MainBooster_AD_Count_StageId_{0}", stageId);
        logEvent(desc);
    }

    public void logUseMainBooster()
    {
        string desc = string.Format("MainBooster_AD_Count");
        logEvent(desc);
    }

    public void logUseBoostItem()
    {
        logEvent("BoosterItem_Use_Count");
    }
}
