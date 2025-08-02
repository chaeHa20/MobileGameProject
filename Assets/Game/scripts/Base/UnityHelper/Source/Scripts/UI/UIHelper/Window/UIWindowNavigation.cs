using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace UnityHelper
{
    public sealed class UIWindowNavigation : IDisposable
    {
        /// <summary>
        ///  window name, UIWindow
        /// </summary>
        private Dictionary<string, UIWindow> m_instances = new Dictionary<string, UIWindow>();
        private Dictionary<string, UIGameMainWindow> m_mainInstances = new Dictionary<string, UIGameMainWindow>();
        private LinkedList<UIWindowData> m_showDatas = new LinkedList<UIWindowData>();
        private HashSet<string> m_dontDestroyes = new HashSet<string>();

        public int instanceCount => m_instances.Count;
        public int mainInstanceCount => m_mainInstances.Count;

        public void addFirst(UIWindowData data)
        {
            if (Logx.isActive)
            {
                Logx.assert(null != data, "data is null");
                //Logx.trace("addFirst showData {0}", data.name);
            }

            if (UIWindowData.eInactiveCurrent.Immediately == data.inactiveCurrent)
                setInactiveCurrent();

            m_showDatas.AddFirst(data);
            if (data.isDontDestroy)
                addDontDestroy(data.name);
        }

        public void addLast(UIWindowData data)
        {
            if (Logx.isActive)
            {
                Logx.assert(null != data, "data is null");
                Logx.trace("addLast showData {0}", data.name);
            }

            m_showDatas.AddLast(data);
            if (data.isDontDestroy)
                addDontDestroy(data.name);
        }

        private void addDontDestroy(string name)
        {
            if (isDontDestroy(name))
                return;

            m_dontDestroyes.Add(name);
        }

        private bool isDontDestroy(string name)
        {
            return m_dontDestroyes.Contains(name);
        }

        private void setInactiveCurrent()
        {
            if (isEmptyShowData())
                return;

            string currentName = m_showDatas.First.Value.name;
            UIWindow currentWindow = findInstance<UIWindow>(currentName);
            if (null == currentWindow)
            {
                if (Logx.isActive)
                    Logx.error("Failed setInactiveCurrent, currentName {0}", currentName);

                return;
            }
            // closing 일 때 inactive하면 애니메이션이 진행 안 될 수도 있기 때문에
            if (!currentWindow.isClosing)
                currentWindow.gameObject.SetActive(false);
        }

        public bool back()
        {
            if (isEmptyShowData())
                return false;

            string currentName = m_showDatas.First.Value.name;
            UIWindow currentWindow = findInstance<UIWindow>(currentName);
            if (null != currentWindow)
                currentWindow.back();

            return true;
        }

        public string getCurrentName()
        {
            if (isEmptyShowData())
                return null;

            return m_showDatas.First.Value.name;
        }

        public bool isCurrentMsgBox()
        {
            if (isEmptyShowData())
                return false;

            return m_showDatas.First.Value.isMsgBox;
        }

        public bool isEmptyShowData()
        {
            return (0 == m_showDatas.Count);
        }

        public T findInstance<T>(string name) where T : UIWindow
        {
            if (Logx.isActive)
                Logx.assert(!string.IsNullOrEmpty(name), "name is null or empty");

            if (m_instances.TryGetValue(name, out UIWindow window))
                return window as T;

            return null;
        }

        private bool existInstance(string name)
        {
            return m_instances.ContainsKey(name);
        }

        public void addInstance(string name, UIWindow window, bool isMain)
        {
            if (Logx.isActive)
            {
                Logx.assert(!string.IsNullOrEmpty(name), "name is null or empty");
                Logx.assert(null != window, "window is null");
            }

            if (existInstance(name))
            {
                if (Logx.isActive)
                    Logx.warn("Already exist instance window");
                return;
            }

            if (Logx.isActive)
                Logx.trace("addInstance instance {0}", name);

            m_instances.Add(name, window);

            if (isMain)
            {
                var mainWindow = window as UIGameMainWindow;
                m_mainInstances.Add(name, mainWindow);
            }
        }

        private UIWindowData findShowData(string name)
        {
            return m_showDatas.FirstOrDefault(x => x.name == name);
        }

        public bool isExistShowData(string name)
        {
            if (Logx.isActive)
                Logx.assert(!string.IsNullOrEmpty(name), "name is null or empty");

            return null != findShowData(name);
        }

        public void resume()
        {
            if (isEmptyShowData())
                return;

            UIWindowData currentData = m_showDatas.First.Value;
            UIWindow currentWindow = findInstance<UIWindow>(currentData.name);
            if (null == currentWindow)
            {
                if (Logx.isActive)
                    Logx.error("Failed window resume, not find {0}", currentData.name);
                return;
            }
            
            currentWindow.resume(currentData);
        }

        public void dispose(string name)
        {
            if (Logx.isActive)
                Logx.assert(!string.IsNullOrEmpty(name), "name is null or empty");

            var data = findShowData(name);
            if (null == data)
                return;
            m_showDatas.Remove(data);

            if (Logx.isActive)
                Logx.trace("dispose showData {0}", data.name);

            UIWindow window = findInstance<UIWindow>(name);
            if (null != window)
            {
                if (isExistShowData(name) || isDontDestroy(name))
                {
                    window.keep();
                }
                else
                {
                    if (Logx.isActive)
                        Logx.trace("dispose instances {0}", data.name);

                    m_instances.Remove(name);
                    if(m_mainInstances.ContainsKey(name))
                        m_mainInstances.Remove(name);

                    window.Dispose();
                }
            }
        }

        public void onClose(string name, float delay)
        {
            if (Logx.isActive)
                Logx.assert(!string.IsNullOrEmpty(name), "name is null or empty");

            UIWindow window = findInstance<UIWindow>(name);
            if (null != window)
            {
                window.onClose(delay);
            }
        }

        public void closeAll()
        {
            var windowNames = (from d in m_showDatas
                               select d.name).ToList();

            foreach(var windowName in windowNames)
            {
                dispose(windowName);
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
                m_instances.Clear();
                m_mainInstances.Clear();
                m_showDatas.Clear();
                m_dontDestroyes.Clear();
            }
        }
    }
}