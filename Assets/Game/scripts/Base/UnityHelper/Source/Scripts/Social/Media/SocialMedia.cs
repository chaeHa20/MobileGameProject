using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
#if _SOCIAL_ && UNITY_ANDROID
using Google;
#endif

namespace UnityHelper
{
    public class SocialMedia : MonoBehaviour
    {
        private SocialUser m_user = null;

        public virtual bool isLoggedIn { get { return false; } }
        public SocialUser user { get { return m_user; } }
        
        public virtual void initialize()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback">success, error</param>
        public virtual void login(bool isDownloadProfile, Action<bool, string> callback)
        {

        }

        public virtual void initLogInData()
        {
        }

        public virtual void tryLogIn()
        {

        }


        public virtual void logout()
        {

        }

        protected void setUser(string id, string name, Texture2D texture)
        {
            m_user = new SocialUser(id, name, texture);
        }

        protected virtual string getProfileUrl()
        {
            return "";
        }

        public virtual void showLeaderboardUI()
        {
        }

        public virtual void showAchievementsUI()
        {
        }

        public virtual void setAchievement(string achievementId, double progress, Action<bool> callback)
        {
        }

        public virtual void setLeaderboard(long score, string board, Action<bool> callback)
        {
        }
    }
}