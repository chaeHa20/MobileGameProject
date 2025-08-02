using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public delegate void UIComponentCallback(List<object> datas);

    public class UIComponent : Disposable
    {
        private Dictionary<int, UIComponentCallback> m_callbacks = new Dictionary<int, UIComponentCallback>();

        public void registMessage(int messageId, UIComponentCallback callback)
        {
            if (UIHelper.isNullInstance())
                return;

            if (m_callbacks.ContainsKey(messageId))
            {
                if (Logx.isActive)
                    Logx.error("Duplicated regist message id {0}", messageId);
                return;
            }

            m_callbacks.Add(messageId, callback);

            UIHelper.instance.registMessage(messageId, callback);
        }

        public void sendMessage(int messageId)
        {
            UIHelper.instance.sendMessage(messageId);
        }

        public void sendMessage(int messageId, object data)
        {
            UIHelper.instance.sendMessage(messageId, data);
        }

        public void sendMessage(int messageId, object data1, object data2)
        {
            UIHelper.instance.sendMessage(messageId, data1, data2);
        }

        public void sendMessage(int messageId, object data1, object data2, object data3)
        {
            UIHelper.instance.sendMessage(messageId, data1, data2, data3);
        }

        public void sendMessage(int messageId, object data1, object data2, object data3, object data4)
        {
            UIHelper.instance.sendMessage(messageId, data1, data2, data3, data4);
        }

        public void sendMessage(int messageId, List<object> datas)
        {
            UIHelper.instance.sendMessage(messageId, datas);
        }

        public void setParent(GameObject parent, SetParentOption option)
        {
            UIHelper.instance.setParent(parent, gameObject, option);
        }

        public void setSafeAreaParent(int layer, SetParentOption option)
        {
            //var parent = UIHelper.instance.getMainSafeArea(safeAreaLayer);
            var parent = UIHelper.instance.canvasGroup.getSafeArea(layer);
            UIHelper.instance.setParent(parent, gameObject, option);
        }

        protected virtual void OnDestroy()
        {
            unregistMessages();
        }

        protected void unregistMessages()
        {
            if (UIHelper.isNullInstance())
                return;

            foreach(var pair in m_callbacks)
            {
                UIHelper.instance.unregistMessage(pair.Key, pair.Value);
            }

            m_callbacks.Clear();
        }
    }
}