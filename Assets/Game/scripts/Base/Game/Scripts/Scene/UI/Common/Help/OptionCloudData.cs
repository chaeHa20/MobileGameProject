using System;
using UnityEngine;
using UnityHelper;
#if UNITY_ANDROID && _SOCIAL_
using System.Collections;
#endif

public class OptionCloudData
{
    public void load(Action callback)
    {
        loadCloudData(callback);
    }

    public void save(Action callback)
    {
        saveCloudData(callback);
    }

    private void checkExistSavedData(SocialCloud cloud, Action callback)
    {
        setActiveBlock(true);
        cloud.getSavedDataTime(savedDataTime =>
        {
            if (string.IsNullOrEmpty(savedDataTime))
            {
                openNoSavedDataMsgBox();
            }
            else
            {
                callback();
            }
        });
    }

    private void loadCloudData(Action callback)
    {
        initCloudData(cloud =>
        {
            checkExistSavedData(cloud, () =>
            {
                cloud.load(result =>
                {
                    if (SocialCloud.eResult.SUCCESS == result)
                    {
                        callback?.Invoke();
                    }
                    else
                    {
                        string title = StringHelper.get("load_data_title");
                        string errBody = StringHelper.get("load_data_fail");
                        showToastMessageWindow(title, errBody, callback);
                    }
                });
            });
        });
    }

    private void saveCloudData(Action callback)
    {
        initCloudData(cloud =>
        {
            if (Logx.isActive)
                Logx.trace("Try Cloud Save Firebase");

            cloud.save(result =>
            {
                if (SocialCloud.eResult.SUCCESS == result)
                {
                    if (Logx.isActive)
                        Logx.trace("Success Cloud Save Firebase");

                    callback?.Invoke();
                }
                else
                {
                    string title = StringHelper.get("data_save_title");
                    string errBody = StringHelper.get("system_error") + "\t" + StringHelper.get("no_saved_cloud_data");
                    showToastMessageWindow(title, errBody, callback);
                }
            });
        });
    }

    private void initCloudData(Action<SocialCloud> callback)
    {
        if (Logx.isActive)
            Logx.trace("initCloudData");

        var cloud = SocialCloud.create();
        cloud.initialize(GameSettings.instance.app.cloudFileName, AESSettings.instance.localData);

        // 클라우드 로그인 콜백은 첫 로그인 인증 할때만 호출 된다.
        // 따라서 여기서 로그인 안된 상태이면 에러 메시지를 출력하자.
        // https://issuetracker.unity3d.com/issues/ios-social-dot-localuser-dot-authenticate-is-not-called-when-user-is-not-authenticated-and-gamecenter-login-pop-up-is-dismissed
        if (cloud.isLoggedIn())
        {
            if (Logx.isActive)
                Logx.trace("isLoggedIn false");

            // setActiveBlock(true);
            cloud.login(false, (success, error) =>
            {
                if (Logx.isActive)
                    Logx.trace("cloud login {0}", success);

                if (success)
                    callback(cloud);
                else
                    openLoginFailMsgBox(error);
            });
        }
        else
        {
            openMustBeLoginSocial();
#if UNITY_ANDROID && _SOCIAL_
            CoroutineHelper.instance.start(coLoginSocial());
#endif
        }
    }

    private void openLoginFailMsgBox(string error)
    {
        var title = StringHelper.get("logIn_title");
        var body = StringHelper.get("social_login_fail", error);
        showToastMessageWindow(title, body, null);
    }

    private void openMustBeLoginSocial()
    {
        string stringCode = "";
#if UNITY_ANDROID
        stringCode = "must_be_logged_in_googleplay";
#elif UNITY_IOS
        stringCode = "must_be_logged_in_gamecenter";
#endif
        var title = StringHelper.get("logIn_title");
        var body = StringHelper.get(stringCode);
        showToastMessageWindow(title, body, null);
    }

    private void openNoSavedDataMsgBox()
    {
        string title = StringHelper.get("load_data_title");
        var body = StringHelper.get("no_saved_cloud_data");
        showToastMessageWindow(title, body, null);
    }

    private void setActiveBlock(bool isActive)
    {
        GameMsgHelper.instance.add(new SetActiveSceneBlockMsg(isActive));
    }

    private void showToastMessageWindow(string title, string msg, Action callback)
    {
        GameObject parent = UIHelper.instance.canvasGroup.getLastSafeArea();

        UIGameToastMsg.create(parent, title, msg, 1.0f, () =>
        {
            callback?.Invoke();
        });
    }

#if UNITY_ANDROID && _SOCIAL_
    private IEnumerator coLoginSocial()
    {
        if (Logx.isActive)
            Logx.trace("coLoginSocial");

        bool isLogged = false;
        eSocialMedia socialMedia = SocialHelper.instance.mediaType;
        if (SocialHelper.instance.isLoggedIn(socialMedia))
        {
            if (Logx.isActive)
                Logx.trace("Gpg isLoggedIn");

            isLogged = true;
        }
        else
        {
            SocialHelper.instance.login(socialMedia, false, (success, error) =>
            {
                if (Logx.isActive)
                    Logx.trace("Gpg login success {0}, error {1}", success, error);

                isLogged = true;
            });
        }

        yield return new WaitUntil(() => isLogged);


    }
#endif
}
