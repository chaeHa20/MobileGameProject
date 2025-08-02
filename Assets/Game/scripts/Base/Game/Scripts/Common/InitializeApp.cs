using System;
using System.Collections;
using UnityEngine;
using UnityHelper;

public class InitializeApp : MonoBehaviour
{
    public static void create(GameObject parent, LocalePlugin localePlugin, Action callback)
    {
        var obj = new GameObject();
        obj.name = "InitializeApp";
        obj.transform.SetParent(parent.transform);
        obj.transform.localPosition = Vector3.zero;
        var initializeApp = obj.AddComponent<InitializeApp>();

        initializeApp.initialize(localePlugin, callback);
    }

    public void initialize(LocalePlugin localePlugin, Action callback)
    {
        StartCoroutine(coInitialize(localePlugin, callback));
    }

    IEnumerator coInitialize(LocalePlugin localePlugin, Action callback)
    {
#if !UNITY_EDITOR
        // Application.targetFrameRate = 30;
#endif
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        if (null != DebugSettings.instance)
            DebugSettings.instance.apply();

        yield return StartCoroutine(coInitializeLocalePlugin(localePlugin));
        yield return StartCoroutine(coInitHelpers());
        //yield return StartCoroutine(coNotificationRequestPermission());
        //yield return StartCoroutine(coCheckAndroidVersion());
        //yield return StartCoroutine(coInitializeFirebase());


        callback();
    }

    private void initHelpers()
    {
        GameTableHelper.instance.initialize(AESSettings.instance.table);
        GameLocalDataHelper.instance.initialize(AESSettings.instance.localData);
        // GameInAppHelper.instance.initialize();
        GameSceneHelper.instance.initialize();
        GamePoolHelper.instance.initialize();
#if _SOCIAL_
        GameSocialHelper.instance.initialize();
#endif
        GameSoundHelper.instance.initialize();
        GameVibrationHelper.instance.initialize();
        GameOptionHelper.instance.initialize();
    }

    IEnumerator coInitializeLocalePlugin(LocalePlugin localePlugin)
    {
        bool isInitialized = false;

        var language = (eLanguage)GamePlayerPrefsHelper.instance.getInt(PlayerPrefsKey.Language, (int)eLanguage.None);
        localePlugin.initialize(language, (newLanguage) =>
        {
            isInitialized = true;

            if (language != newLanguage)
            {
                GamePlayerPrefsHelper.instance.setInt(PlayerPrefsKey.Language, (int)newLanguage);
            }
        });
        yield return new WaitUntil(() => isInitialized);
    }

    IEnumerator coInitHelpers()
    {
        initHelpers();

        yield break;
    }

    IEnumerator coNotificationRequestPermission()
    {
        bool isDone = false;
        GameNotificationHelper.instance.requestPermission(() =>
        {
            isDone = true;
        });

        yield return new WaitUntil(() => isDone);
    }

    IEnumerator coCheckAndroidVersion()
    {
#if UNITY_ANDROID && _ANDROID_APP_UPDATE_
        bool isInitialized = false;
        GameAndroidAppUpdateHelper.check(this, () => isInitialized = true);
        yield return new WaitUntil(() => isInitialized);
#else
        yield break;
#endif
    }

    IEnumerator coLoginSocial()
    {
#if _SOCIAL_
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
      
#else
        yield return new WaitForSeconds(0.2f);
#endif
    }

    IEnumerator coInitializeFirebase()
    {
#if _FIREBASE_
        GameFirebaseHelper.instance.initialize();
        yield return new WaitUntil(() => GameFirebaseHelper.instance.isInitialized);
#else
        yield break;
#endif
    }
}
