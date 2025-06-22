using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public class SocialHelper : MonoSingleton<SocialHelper>
    {
        private Dictionary<eSocialMedia, SocialMedia> m_medias = new Dictionary<eSocialMedia, SocialMedia>();
#if UNITY_ANDROID && _SOCIAL_
        private string m_idToken = null;
        private string m_email = null;
        public string idToken { get {return m_idToken; } set { m_idToken = value; } }
        public string email { get { return m_email; } set { m_email = value; } }
#endif
        public eSocialMedia mediaType
        {
            get
            {
#if UNITY_EDITOR
                return eSocialMedia.Editor;
#else
#if UNITY_ANDROID
          return eSocialMedia.Google;
#else
                return eSocialMedia.Guest;
#endif
#endif
            }
        }

        public virtual void initialize()
        {
#if UNITY_EDITOR
            createMedia<SocialEditor>(eSocialMedia.Editor);
#else
#if UNITY_ANDROID
          createMedia<SocialGoogle>(eSocialMedia.Google);
#endif
      
#endif
        }

        protected void createMedia<M>(eSocialMedia mediaType) where M : SocialMedia
        {
            if (m_medias.ContainsKey(mediaType))
            {
                if (Logx.isActive)
                    Logx.error("Duplicated SocialMedia {0}", mediaType.ToString());
                return;
            }

            SocialMedia media = createMediaObject<M>();
            m_medias.Add(mediaType, media);

            media.initialize();
        }

        private M createMediaObject<M>() where M : SocialMedia
        {
            GameObject obj = new GameObject();
            obj.transform.parent = transform;
            obj.transform.localPosition = Vector3.zero;
            obj.name = typeof(M).Name;
            M m = obj.AddComponent<M>();

            return m;
        }

        protected SocialMedia getMedia(eSocialMedia mediaType)
        {
            if (m_medias.TryGetValue(mediaType, out SocialMedia media))
                return media;

            return null;
        }

        public SocialUser getUser(eSocialMedia mediaType)
        {
            SocialMedia media = getMedia(mediaType);
            if (null == media)
                return null;

            return media.user;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediaType"></param>
        /// <param name="isDownloadProfile"></param>
        /// <param name="callback">success, error</param>
        public virtual void login(eSocialMedia mediaType, bool isDownloadProfile, Action<bool, string> callback)
        {
            SocialMedia media = getMedia(mediaType);
            if (null == media)
            {
                if (Logx.isActive)
                    Logx.error("Failed find media {0}", mediaType);

                callback?.Invoke(false, string.Format("Failed find media {0}", mediaType));
                return;
            }

            UIHelper.instance.isNotShowPauseMsg = true;
            media.login(isDownloadProfile, (success, error) =>
            {
                callback?.Invoke(success, error);
            });
        }

        public virtual void logout(eSocialMedia mediaType)
        {
            SocialMedia media = getMedia(mediaType);
            if (null == media)
                return;

            media.logout();
        }

        public bool isLoggedIn(eSocialMedia mediaType)
        {
            SocialMedia media = getMedia(mediaType);
            if (null == media)
                return false;

            return media.isLoggedIn;
        }

        public void connectLogIn(eSocialMedia mediaType)
        {
            if (Logx.isActive)
                Logx.trace("try connect google signIn");

            SocialMedia media = getMedia(mediaType);
            if (null != media)
                media.tryLogIn();
        }

        public void showLeaderboardUI()
        {
            SocialMedia media = getMedia(mediaType);
            if (null == media)
                return;

            media.showLeaderboardUI();
        }

        public void showAchievementsUI()
        {
            SocialMedia media = getMedia(mediaType);
            if (null == media)
                return;

            media.showAchievementsUI();
        }
    }
}