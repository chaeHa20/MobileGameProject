using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using System;
using Unity.Notifications;

namespace UnityHelper
{
    public class NotificationHelper : MonoSingleton<NotificationHelper>
    {
        [SerializeField] string m_smallIconId = "icon_small";
        [SerializeField] string m_largeIconId = "icon_large";

        private string m_channelId = "channel_id";
        private bool m_isInitializedNotificationCenter = false;
        private PlatformNotification m_platform = null;

        protected override void Awake()
        {
            base.Awake();

            createPlatform();
        }

        private void createPlatform()
        {
#if UNITY_ANDROID
            m_platform = new AndroidPlatformNotification();
#elif UNITY_IOS
            m_platform = new IosPlatformNotification();
#else
            m_platform = new AndroidPlatformNotification();
#endif      
        }

        protected virtual void initialize()
        {            
            cancelNotifications();            
            m_platform.initialize(m_channelId, m_smallIconId, m_largeIconId);
        }

        private void initializeNotificationCenter()
        {
            var args = NotificationCenterArgs.Default;
            args.AndroidChannelId = m_channelId;
            args.AndroidChannelName = "Notifications";
            args.AndroidChannelDescription = "Main notifications";
            NotificationCenter.Initialize(args);

            m_isInitializedNotificationCenter = true;
        }
        
        public void requestPermission(Action callback)
        {
            initializeNotificationCenter();

            m_platform.requestPermission(this, () =>
            {
                if (Logx.isActive)
                    Logx.trace("-----------------> init Notification");

                initialize();
                callback();
            });
        }

        void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                if (Logx.isActive)
                    Logx.trace("-----------------> pause setNotification");

                sendNotifications();
            }
            else
            {
                if (Logx.isActive)
                    Logx.trace("-----------------> unpause setNotification");
                cancelNotifications();
            }
        }

        protected virtual void sendNotifications()
        {
            //if (Debugx.isActive)
            //    sendTestNotifications();
        }

        private void sendTestNotifications()
        {
            sendNotification("Test Title", "Test Msg", 5, false);
            sendNotification("Test Schedule Title", "Test Schedule Msg", 10, true);
        }

        protected void sendNotification(string title, string msg, long delay, bool isRepeat)
        {
            m_platform.sendNotification(title, msg, delay, isRepeat);
        }

        private void cancelNotifications()
        {
            if (!m_isInitializedNotificationCenter)
                return;

            m_platform.cancelNotifications();
        }

        protected int getRemainTime(int hour, int minute)
        {
            DateTime now = DateTime.Now;
            DateTime notiDate = new DateTime(now.Year, now.Month, now.Day, hour, minute, 0);
            if (now > notiDate)
            {
                notiDate = notiDate.AddDays(1);
            }

            int remainTime = (int)TimeHelper.getRemainSecond(notiDate.ToString(CultureInfo.InvariantCulture));

            if (Logx.isActive)
                Logx.trace("setNotification remainTime {0}, notiDate {1}", remainTime, notiDate.ToString(CultureInfo.InvariantCulture));

            return remainTime;
        }
    }
}