using UnityEngine;
using UnityEngine.UI;
using UnityHelper;

public class UIGameOptionWindow : UIGameMainWindow
{
    [SerializeField] UIToggleButtonGroup m_graphicQuality = null;
    [SerializeField] UIToggleButton m_viveration = null;
    [SerializeField] UIToggleButton m_bgm = null;
    [SerializeField] UIToggleButton m_sfx = null;
    [SerializeField] Text m_language = null;
    [SerializeField] Text m_version = null;
    [SerializeField] Button m_umpPrivacyOptionsForm = null;
    [SerializeField] GameObject m_restorePurchase = null;
    [SerializeField] UIDropDown m_dropDown = null;

    protected LocalGameOption m_localGameOption = null;
    private eGraphicQuality m_oriGraphicQuality = eGraphicQuality.Medium;


    public override void initialize(UIWidgetData data)
    {
        base.initialize(data);

        checkActiveUmpPrivacyOptionsForm();
        requestGetGameOption();

#if UNITY_ANDROID
        m_restorePurchase.SetActive(false);
#elif UNITY_IOS
        m_restorePurchase.SetActive(true);
#endif
    }

    private void requestGetGameOption()
    {
        var req = new Req_GetGameOption();
        GameLocalDataHelper.getInstance().requestGetGameOption(localGameOption =>
        {
            setOption(localGameOption);
        });
    }

    protected void requestSetGameOption()
    {
        var req = new Req_SetGameOption { localGameOption = m_localGameOption };
        GameLocalDataHelper.instance.request<Res_SetGameOption>(req, (res) =>
        {
            if (res.isSuccess)
            {
                m_localGameOption = res.localGameOption;
            }
        });
    }

    protected virtual void setOption(LocalGameOption localGameOption)
    {
        m_localGameOption = localGameOption;

        m_bgm.initialize(localGameOption.isBgmOn);
        m_sfx.initialize(localGameOption.isSfxOn);
        m_viveration.initialize(localGameOption.isViverationOn);

        m_oriGraphicQuality = localGameOption.graphicQuality;
        m_graphicQuality.initialize((int)localGameOption.graphicQuality);
        m_language.text = StringHelper.get(LanguageHelper.language);
        m_version.text = StringHelper.getVersion();
    }

    public void onGraphicQuality(int index)
    {
        var oldGraphicOptionIndex = m_localGameOption.graphicQuality;
        m_localGameOption.graphicQuality = (eGraphicQuality)index;

        QualitySettings.SetQualityLevel(index);
        requestSetGameOption();

        GameSceneHelper.getInstance().loadStartScene(false, false);
    }

    public void onLanguage(int language)
    {
        if ((eLanguage)language != LanguageHelper.language)
        {
            LanguageHelper.language = (eLanguage)language;
            m_language.text = StringHelper.get(LanguageHelper.language);
            GamePlayerPrefsHelper.instance.setInt(PlayerPrefsKey.Language, (int)language);
            GameTableHelper.instance.initialize(AESSettings.instance.table);
            updateLanguages();
            sendMessage((int)eUIMessage.UpdateLanguage);
            m_dropDown.setLanguage();
        }
        else
            m_language.text = StringHelper.get(LanguageHelper.language);
    }

    public void onBgm()
    {
        m_localGameOption.isBgmOn = m_bgm.isOn;

        var volume = m_bgm.isOn ? 1.0f : 0.0f;
        GameSoundHelper.getInstance().setBgmVolume(volume);

        requestSetGameOption();
    }

    public void onSfx()
    {
        m_localGameOption.isSfxOn = m_sfx.isOn;

        var volume = m_sfx.isOn ? 1.0f : 0.0f;
        GameSoundHelper.getInstance().setSfxVolume(volume);

        requestSetGameOption();
    }

    public void onViveration()
    {
        m_localGameOption.isViverationOn = m_viveration.isOn;

        requestSetGameOption();

        GameOptionHelper.instance.setGameOption(m_localGameOption);
    }

    public void onRestorePurchase()
    {
#if UNITY_IOS
        var cloud = new IosCloud();

        if (cloud.loadRestoreAllData())
        {
            openChanageGraphicQualityNoticeWindow(eOption.RestorePurchase, (isRestart) =>
{

    var req = new Req_SetOfflineTime();
    GameLocalDataHelper.instance.request<Res_SetOfflineTime>(req, (res) =>
            {
            if (res.isSuccess)
            {
                GameSceneHelper.getInstance().loadStartScene(false, false);
            }
        });
});
        }
        else
        {
            var msg = StringHelper.get("no_restore_purchase_data");
            UIGameToastMsg.create(null, msg);
        }
#endif
    }

    private void checkActiveUmpPrivacyOptionsForm()
    {
#if _APPLOVIN_
        var consentGeography = MaxSdk.GetSdkConfiguration().ConsentFlowUserGeography;

        if (consentGeography == MaxSdkBase.ConsentFlowUserGeography.Gdpr)
            m_umpPrivacyOptionsForm.gameObject.SetActive(true);
        else
            m_umpPrivacyOptionsForm.gameObject.SetActive(false);
#else
        m_umpPrivacyOptionsForm.gameObject.SetActive(false);
#endif
    }

    private void updateLanguages()
    {
        var texts = GetComponentsInChildren<UIApplyDBString>();
        foreach (var DBString in texts)
        {
            DBString.updateLanguage();
        }

        m_language.text = StringHelper.get(LanguageHelper.language);
    }

    public void onDebugMax()
    {
#if _APPLOVIN_
        MaxSdk.ShowMediationDebugger();
#endif
    }

    public void onShowUmpPrivacyOptionsForm()
    {
#if _ADMOB_
        UmpUtility.ShowPrivacyOption((isShow) =>
        {
            // AdMob에서 알아서 하므로 건들게 없다.
        });
#elif _APPLOVIN_
        var cmpService = MaxSdk.CmpService;

        cmpService.ShowCmpForExistingUser(error =>
        {
            if (null == error)
            {
                // The CMP alert was shown successfully.
            }
        });
#endif
    }
}
