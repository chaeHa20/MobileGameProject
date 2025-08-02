using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace UnityHelper
{
    public class UIPanelHelper : IDisposable
    {
        private Dictionary<string, UIPanel> m_panels = new Dictionary<string, UIPanel>();
        private List<UIPanel> m_panelList = new List<UIPanel>();

        public T open<T>(UIPanelData data) where T : UIPanel
        {
            if (Logx.isActive)
                Logx.assert(null != data, "data is null");

            if (exist(data.name))
            {
                return get<T>(data.name);
            }
            else
            {
                T t = ResourceHelper.instance.instantiate<T>(data.resPath);
                UIHelper.instance.setParent(data.parent, t.gameObject, data.setParentOption);
                m_panels.Add(data.name, t);
                m_panelList.Add(t);

                t.name = data.name;
                t.initialize(data);
                t.open();
                return t;
            }
        }

        public void close(string name)
        {
            if (Logx.isActive)
                Logx.assert(!string.IsNullOrEmpty(name), "name is null or empty");

            if (m_panels.TryGetValue(name, out UIPanel panel))
            {
                panel.Dispose();
                m_panels.Remove(name);
                m_panelList.Remove(panel);
            }
        }

        public T get<T>(string name) where T : UIPanel
        {
            if (m_panels.TryGetValue(name, out UIPanel panel))
            {
                return panel as T;
            }

            return null;
        }

        public T getCurrent<T>() where T : UIPanel
        {
            if (0 == m_panelList.Count)
                return null;

            return m_panelList[m_panelList.Count - 1] as T;
        }

        public bool exist(string name)
        {
            return m_panels.ContainsKey(name);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_panels.Clear();
            }
        }
    }
}