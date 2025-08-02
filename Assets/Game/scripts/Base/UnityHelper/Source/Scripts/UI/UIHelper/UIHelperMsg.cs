using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public class UIHelperMsg : IDisposable
    {
        private class Callback
        {
            public List<UIComponentCallback> list = new List<UIComponentCallback>();
        }

        private Dictionary<int, Callback> m_msgs = new Dictionary<int, Callback>();

        public void regist(int id, UIComponentCallback callback)
        {
            if (!m_msgs.TryGetValue(id, out Callback value))
            {
                value = new Callback();
                m_msgs.Add(id, value);
            }

            value.list.Add(callback);
        }

        public void unregist(int id, UIComponentCallback callback)
        {
            if (m_msgs.TryGetValue(id, out Callback value))
            {
                int index = value.list.IndexOf(callback);
                if (0 <= index)
                    value.list.RemoveAt(index);
            }
        }

        public void send(int messageId)
        {
            send(messageId, null);
        }

        public void send(int messageId, object data)
        {
            send(messageId, new List<object> { data });
        }

        public void send(int messageId, object data1, object data2)
        {
            send(messageId, new List<object> { data1, data2 });
        }

        public void send(int messageId, object data1, object data2, object data3)
        {
            send(messageId, new List<object> { data1, data2, data3 });
        }

        public void send(int messageId, object data1, object data2, object data3, object data4)
        {
            send(messageId, new List<object> { data1, data2, data3, data4 });
        }

        public void send(int id, List<object> datas)
        {
            try
            {
                if (m_msgs.TryGetValue(id, out Callback value))
                {
                    foreach (var callback in value.list)
                    {
                        callback?.Invoke(datas);
                    }
                }
            }
            catch(Exception e)
            {
                if (Logx.isActive)
                    Logx.exception(e);
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_msgs.Clear();
            }
        }
    }
}