using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public class UIWindowHelper : IDisposable
    {
        private UIWindowNavigation m_navigation = new UIWindowNavigation();
        private UUID m_uuid = new UUID();
        private Action m_openCallback = null;
        private Action m_disposeCallback = null;

        public int instanceCount { get { return m_navigation.instanceCount; } }
        public int mainInstanceCount { get { return m_navigation.mainInstanceCount; } }
        public Action openCallback { get => m_openCallback; set => m_openCallback = value; }
        public Action disposeCallback { get => m_disposeCallback; set => m_disposeCallback = value; }

        public T open<T>(UIWindowData data, bool isMain) where T : UIWindow
        {
            if (Logx.isActive)
                Logx.assert(null != data, "data is null");

            setMsgBoxRename(data);

            if (UIWindowData.eAddPosition.First == data.addPosition || m_navigation.isEmptyShowData())
            {
                var currentWindow = getCurrent<UIWindow>();
                if (null != currentWindow)
                    currentWindow.suspend();

                m_navigation.addFirst(data);

                T t = m_navigation.findInstance<T>(data.name);
                if (null == t)
                {
                    t = ResourceHelper.instance.instantiate<T>(data.resPath);
                    UIHelper.instance.setParent(data.parent, t.gameObject, data.setParentOption);
                    m_navigation.addInstance(data.name, t, isMain);

                    t.name = data.name;
                    t.initialize(data);
                    t.open();                    
                }
                else
                {
                    t.resume(data);
                }

                m_openCallback?.Invoke();

                return t as T;
            }
            else
            {
                m_navigation.addLast(data);
                m_openCallback?.Invoke();

                return null;
            }
        }

        public W get<W>(string name) where W : UIWindow
        {
            return m_navigation.findInstance<W>(name);
        }

        public W getCurrent<W>() where W : UIWindow
        {
            string currentName = m_navigation.getCurrentName();
            if (string.IsNullOrEmpty(currentName))
                return null;

            return m_navigation.findInstance<W>(currentName);
        }

        public bool isCurrent(string name)
        {
            string currentName = m_navigation.getCurrentName();
            return string.Equals(currentName, name);
        }

        public bool isCurrentMsgBox()
        {
            return m_navigation.isCurrentMsgBox();
        }

        public bool isOpen(string name)
        {
            var win = get<UIWindow>(name);
            return null != win;
        }

        /// <summary>
        /// 메세지 박스는 중복되면 안되기 때문에 이름을 강제로 틀리게 해준다.
        /// </summary>
        /// <param name="data"></param>
        private void setMsgBoxRename(UIWindowData data)
        {
            if (data.isMsgBox)
                data.name = string.Format("{0}_{1}", data.name, m_uuid.make());
        }

        public void dispose(string name)
        {
            if (Logx.isActive)
                Logx.assert(!string.IsNullOrEmpty(name), "name is null");

            m_navigation.dispose(name);
            m_navigation.resume();

            m_disposeCallback?.Invoke();
        }

        public void disposeCurrent()
        {
            string currentName = m_navigation.getCurrentName();
            if (string.IsNullOrEmpty(currentName))
                return;

            dispose(currentName);
        }

        public void close(string name, float delay)
        {
            if (Logx.isActive)
                Logx.assert(!string.IsNullOrEmpty(name), "name is null");

            m_navigation.onClose(name, delay);
        }

        public void closeCurrent(float delay)
        {
            string currentName = m_navigation.getCurrentName();
            if (string.IsNullOrEmpty(currentName))
                return;

            close(currentName, delay);
        }

        public void closeUntilTarget(string name)
        {
            while (true)
            {
                string currentName = m_navigation.getCurrentName();
                if (string.IsNullOrEmpty(currentName))
                    break;

                if (name == currentName)
                    break;

                dispose(currentName);
            }
        }

        public void closeAll()
        {
            m_navigation.closeAll();
        }

        public bool back()
        {
            return m_navigation.back();
        }

        public bool exist(string name)
        {
            return m_navigation.isExistShowData(name);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_navigation.Dispose();
            }
        }
    }
}