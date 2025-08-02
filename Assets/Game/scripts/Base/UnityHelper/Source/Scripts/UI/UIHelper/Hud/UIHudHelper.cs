using System.Collections.Generic;
using System;

namespace UnityHelper
{
    public class UIHudHelper : IDisposable
    {
        private Dictionary<string, UIHudContainer> m_containers = new Dictionary<string, UIHudContainer>();

        private void regist(string alias, UIHud hud)
        {
            if (Logx.isActive)
            {
                Logx.assert(!string.IsNullOrEmpty(alias), "Invalid alias");
                Logx.assert(null != hud, "hud is null");
            }

            var container = find(alias);
            if (null == container)
            {
                container = new UIHudContainer();
                m_containers.Add(alias, container);
            }

            container.regist(hud);
        }

        public void regist(UIHud hud)
        {
            if (Logx.isActive)
                Logx.assert(null != hud, "hud is null");

            regist(hud.GetType().Name, hud);
        }

        private void unregist(string alias, int instanceId, bool isDestroy)
        {
            if (Logx.isActive)
                Logx.assert(!string.IsNullOrEmpty(alias), "Invalid alias");

            var container = find(alias);
            if (null == container)
            {
                if (Logx.isActive)
                    Logx.error("Failed find alias {0}", alias);

                return;
            }

            container.unregist(instanceId, isDestroy);
        }

        public void unregist(UIHud hud, bool isDestroy = true)
        {
            if (Logx.isActive)
                Logx.assert(null != hud, "hud is null");

            unregist(hud.GetType().Name, hud.GetInstanceID(), isDestroy);
        }

        public UIHudContainer find<T>() where T : UIHud
        {
            return find(typeof(T).FullName);
        }

        public UIHudContainer find(string alias)
        {
            if (Logx.isActive)
                Logx.assert(!string.IsNullOrEmpty(alias), "Invalid alias");

            if (m_containers.TryGetValue(alias, out UIHudContainer container))
                return container;

            return null;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach(var pair in m_containers)
                {
                    pair.Value.Dispose();
                }
                m_containers.Clear();
            }
        }
    }
}