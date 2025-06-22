using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public class TempInstanceBuffer
    {
        private Dictionary<string, GameObject> m_buffers = new Dictionary<string, GameObject>();

        public void add(string key, GameObject obj)
        {
            if (Logx.isActive)
            {
                Logx.assert(!string.IsNullOrEmpty(key), "Failed add temp instance buffer, key is null or empty");
                Logx.assert(null != obj, "Failed add temp instance buffer, obj is null, key {0}", key);
            }

            if (exist(key))
            {
                if (Logx.isActive)
                    Logx.warn("Failed add temp instance buffer, obj is exist, key {0}", key);
                return;
            }

            m_buffers.Add(key, obj);
        }

        public T get<T>(string key) where T : Component
        {
            if (Logx.isActive)
                Logx.assert(!string.IsNullOrEmpty(key), "Failed get temp instance buffer, key is null or empty");

            var obj = find(key);
            if (null == obj)
            {
                if (Logx.isActive)
                    Logx.warn("Failed get temp instance buffer, failed find object, key {0}", key);
                return null;
            }

            return obj.GetComponent<T>();
        }

        public void destroy(string key)
        {
            if (Logx.isActive)
                Logx.assert(!string.IsNullOrEmpty(key), "Failed destroy temp instance buffer, key is null or empty");

            var obj = find(key);
            if (null == obj)
            {
                if (Logx.isActive)
                    Logx.warn("Failed destroy temp instance buffer, failed find object, key {0}", key);
                return;
            }

            m_buffers.Remove(key);
            GameObject.Destroy(obj);
        }

        private GameObject find(string key)
        {
            if (m_buffers.TryGetValue(key, out GameObject obj))
                return obj;

            return null;
        }

        private bool exist(string key)
        {
            return m_buffers.ContainsKey(key);
        }

        public void active(string key, bool isActive)
        {
            if (Logx.isActive)
                Logx.assert(!string.IsNullOrEmpty(key), "key is null or empty");

            var obj = find(key);
            if (null == obj)
                return;

            obj.SetActive(isActive);
        }
    }
}
