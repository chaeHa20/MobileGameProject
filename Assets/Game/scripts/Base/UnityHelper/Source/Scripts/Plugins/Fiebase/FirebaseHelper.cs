
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if _FIREBASE_
using Firebase.Extensions;
using Firebase;
#endif

namespace UnityHelper
{
    public class FirebaseHelper : NonMonoSingleton<FirebaseHelper>
    {
        private bool m_isInitialized = false;

        public bool isInitialized => m_isInitialized;

#if _FIREBASE_
        private FirebaseApp m_app = null;
        
        public void initialize()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
                var dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    m_app = FirebaseApp.DefaultInstance;

                    if (Logx.isActive)
                        Logx.trace("Success firebase dependencies: {0}", dependencyStatus);
                }
                else
                {
                    if (Logx.isActive)
                        Logx.error("Could not resolve all Firebase dependencies: {0}", dependencyStatus);
                }

                m_isInitialized = true;
            });
        }

        public void logEvent(string name)
        {
            if (Logx.isActive)
                Logx.trace("<color=blue>------>Firebase logEvent({0})</color>", name);
                

            Firebase.Analytics.FirebaseAnalytics.LogEvent(name);
        }

        public void logEvent(string name, string parameterName, string parameterValue)
        {
            if (Logx.isActive)
                Logx.trace("<color=blue>------>Firebase logEvent({0}, {1}, {2})</color>", name, parameterName, parameterValue);
            

            Firebase.Analytics.FirebaseAnalytics.LogEvent(name, parameterName, parameterValue);
        }
#else
        public void initialize()
        {
            m_isInitialized = true;
        }

        public void logEvent(string name)
        {
            if (Logx.isActive)
                Logx.trace("<color=blue>------>Firebase logEvent({0})</color>", name);

           
        }

        public void logEvent(string name, string parameterName, string parameterValue)
        {
            if (Logx.isActive)
                Logx.trace("<color=blue>------>Firebase logEvent({0}, {1}, {2})</color>", name, parameterName, parameterValue);
            

        }
#endif
    }
}