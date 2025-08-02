using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public class PlatformNotification
    {
        protected string m_smallIconId = "icon_small";
        protected string m_largeIconId = "icon_large";
        protected string m_channelId = "channel_id";

        public virtual void initialize(string channelId, string smallIconId, string largeIconId)
        {
            m_channelId = channelId;
            m_smallIconId = smallIconId;
            m_largeIconId = largeIconId;
        }

        public virtual void requestPermission(MonoBehaviour mono, Action callback)
        {

        }

        public virtual void sendNotification(string title, string msg, long delay, bool isRepeat)
        {

        }

        public virtual void cancelNotifications()
        {

        }
    }
}