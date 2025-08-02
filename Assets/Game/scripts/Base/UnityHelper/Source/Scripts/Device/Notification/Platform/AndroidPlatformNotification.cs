using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Notifications;
using System.Globalization;

#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif

namespace UnityHelper
{
    public class AndroidPlatformNotification : PlatformNotification
    {
#if UNITY_ANDROID
        public override void initialize(string channelId, string smallIconId, string largeIconId)
        {
            base.initialize(channelId, smallIconId, largeIconId);

            registerNotificatonChannel();
        }

        private void registerNotificatonChannel()
        {
            var group = new AndroidNotificationChannelGroup()
            {
                Id = "Main",
                Name = "Main notifications",
            };
            AndroidNotificationCenter.RegisterNotificationChannelGroup(group);

            var channel = new AndroidNotificationChannel()
            {
                Id = m_channelId,
                Name = "Default Channel",
                Importance = Importance.Default,
                Description = "Generic notifications",
                Group = "Main",  // must be same as Id of previously registered group
            };
            AndroidNotificationCenter.RegisterNotificationChannel(channel);
        }

        public override void requestPermission(MonoBehaviour mono, Action callback)
        {
            mono.StartCoroutine(coRequestPermission(() =>
            {
                callback();
            }));
        }

        IEnumerator coRequestPermission(Action callback)
        {
            var request = NotificationCenter.RequestPermission();
            if (request.Status == NotificationsPermissionStatus.RequestPending)
                yield return request;

            if (Logx.isActive)
                Logx.trace("Permission result: " + request.Status);

            callback();
        }

        public override void sendNotification(string title, string msg, long delay, bool isRepeat)
        {
            var notification = new AndroidNotification();
            notification.Title = title;
            notification.Text = msg;
            notification.SmallIcon = m_smallIconId;
            notification.LargeIcon = m_largeIconId;
            notification.FireTime = DateTime.Now.AddSeconds(delay);
            if (isRepeat)
                notification.RepeatInterval = TimeSpan.FromSeconds(delay);
            AndroidNotificationCenter.SendNotification(notification, m_channelId);

            if (Logx.isActive)
                Logx.trace("setNotification {0}, {1}, {2} {3}", title, msg, delay, notification.FireTime.ToString());
        }

        public override void cancelNotifications()
        {
            AndroidNotificationCenter.CancelAllNotifications();
        }
#endif
    }
}