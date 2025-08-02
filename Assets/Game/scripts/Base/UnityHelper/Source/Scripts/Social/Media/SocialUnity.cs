using System;
using UnityEngine;

#if _SOCIAL_ && UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

namespace UnityHelper
{
    public class SocialUnity : SocialMedia
    {
#if _SOCIAL_

        public override bool isLoggedIn => null == Social.localUser ? false : Social.localUser.authenticated;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback">success, error</param>
        public override void login(bool isDownloadProfile, Action<bool, string> callback)
        {
            if (Logx.isActive)
                Logx.trace("Social Authenticate");

            Social.localUser.Authenticate((isSuccess, error) =>
            {
                if (Logx.isActive)
                    Logx.trace("Social Authenticate success {0}, error {1}", isSuccess, error);

                if (isSuccess)
                {
                    if (isDownloadProfile)
                    {
                        StartCoroutine(GraphicHelper.coDownloadTexture(getProfileUrl(), (texture, t_error) =>
                        {
                            Texture2D t = texture ?? Social.localUser.image;
                            setUser(Social.localUser.id, Social.localUser.userName, t);
                        }));
                    }
                    else
                    {
                        setUser(Social.localUser.id, Social.localUser.userName, Social.localUser.image);
                    }

                    callback?.Invoke(isSuccess, error);
#if _FIREBASE_
#if UNITY_ANDROID
                    GameFirebaseHelper.instance.logGameEvent("Social_GooglePlay_LogIn");
#elif UNITY_IOS
                    GameFirebaseHelper.instance.logGameEvent("Social_GameCenter_LogIn");
#endif
#endif
                }
                else
                {
#if UNITY_ANDROID
                    // TODO : Social로 로그인 안되면 
                    PlayGamesPlatform.Activate();
                    PlayGamesPlatform.Instance.Authenticate((status) =>
                    {
                        if (SignInStatus.Success == status)
                        {
                            if (Logx.isActive)
                                Logx.trace("Forced PlayGamesPlatform Login success Social Error {0}", error);

                            callback?.Invoke(true, error);
#if _FIREBASE_
                            GameFirebaseHelper.instance.logGameEvent("Social_GooglePlay_LogIn");
#endif
                        }
                        else
                        {
                            if (Logx.isActive)
                                Logx.trace("Forced PlayGamesPlatform Login Fail Social Error {0}", error);

                            callback?.Invoke(false, error);
                        }
                    });
#else
                    if (Logx.isActive)
                        Logx.error("Social Authenticate Failed code : {0}", error);

                    callback?.Invoke(isSuccess, error);
#endif
                }
            });
    }

    public override void showLeaderboardUI()
    {
        Social.ShowLeaderboardUI();
    }

    public override void showAchievementsUI()
    {
        Social.ShowAchievementsUI();
    }

    public override void setAchievement(string achievementId, double progress, Action<bool> callback)
    {
        Social.ReportProgress(achievementId, progress, callback);
    }

    public override void setLeaderboard(long score, string board, Action<bool> callback)
    {
        Social.ReportScore(score, board, callback);
    }

#else
    public override bool isLoggedIn => true;
#endif
}
}