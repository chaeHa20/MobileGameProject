#if _ONE_ADMAX_
using ONEAdMax;
#endif
using System;
using UnityHelper;

public class GameAdHelper : AdHelper
{
    public static GameAdHelper getInstance()
    {
        return getInstance<GameAdHelper>();
    }

    public override void initialize(BaseAdSettings settings, Action callback)
    {
#if _APPLOVIN_
        base.initialize(settings, callback);
#endif
    }

    /// <param name="callback">isSuccess</param>
    public void showReward(Action<bool> callback)
    {
        show(eAdFormat.Reward, (isSuccess) =>
        {
            callback(isSuccess);
        });
    }

    /// <param name="callback">isSuccess</param>
    public void showInterstitial(Action<bool> callback)
    {
        show(eAdFormat.Interstitial, callback);
    }

    /// <param name="callback">isSuccess</param>
    private void show(eAdFormat adFormat, Action<bool> callback)
    {
#if _INAPP_
        if (GameInAppHelper.getInstance().isNoAds)
        {
            if (Logx.isActive)
                Logx.trace("ad show is no ads");

            callback(true);
            return;
        }
#endif

#if UNITY_EDITOR
        callback(true);
#else

#if _TEST_BUILD_
                    callback(true);
#endif
        GameHelper.ignoreApplicationPauseAction(true);

        // setActiveBlock(true);
        show(adFormat, (adResults) =>
        {
            bool isSuccess = adResults.isSuccess();

#if _ONE_ADMAX_
            if (eAdFormat.Reward == adFormat && isSuccess)
                ONEAdMaxClient.SetUserId(SystemInfo.deviceUniqueIdentifier);

            if (Logx.isActive)
                Logx.trace("ad Show In One Ad Max");
#elif _APPLOVIN_
            if (Logx.isActive)
                Logx.trace("ad Show In AppLovin");
#endif

            if (null != callback)
                callback(isSuccess);

            if (!isSuccess)
            {
                // 백그라운드로 가지 않으므로 직접 false를 설정하자.
                GameHelper.ignoreApplicationPauseAction(false);
            }
        });

#endif
    }

    public void forceRequest()
    {
        forceRequest(eAdFormat.Interstitial);
        forceRequest(eAdFormat.Reward);
    }
}
