using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public class UIHudContainer : IDisposable
    {
        private Dictionary<int, UIHud> m_hudes = new Dictionary<int, UIHud>();

        public Dictionary<int, UIHud>.Enumerator getEnumerator()
        {
            return m_hudes.GetEnumerator();
        }

        public void regist(UIHud hud)
        {
            if (Logx.isActive)
                Logx.assert(null != hud, "hud is null");

            if (null != find<UIHud>(hud.GetInstanceID()))
            {
                if (Logx.isActive)
                    Logx.error("Duplicated hud key {0}", hud.GetInstanceID());

                return;
            }

            m_hudes.Add(hud.GetInstanceID(), hud);
        }

        public void unregist(int instanceId, bool isDestroy)
        {
            var hud = find<UIHud>(instanceId);
            if (null == hud)
            {
                if (Logx.isActive)
                    Logx.error("Failed find hud key {0}", instanceId);

                return;
            }

            if (isDestroy)
            {
                GameObject.Destroy(hud.gameObject);
            }

            m_hudes.Remove(instanceId);
        }

        public T find<T>(int instanceId) where T : UIHud
        {
            if (m_hudes.TryGetValue(instanceId, out UIHud hud))
                return hud as T;

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
                foreach (var pair in m_hudes)
                {
                    if (null != pair.Value)
                        GameObject.Destroy(pair.Value.gameObject);
                }
                m_hudes.Clear();
            }
        }
    }
}