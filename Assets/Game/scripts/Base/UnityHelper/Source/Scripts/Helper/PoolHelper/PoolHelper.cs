using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace UnityHelper
{
    public class PoolHelper : MonoSingleton<PoolHelper>
    {
        [Serializable]
        public class CreatePoolData
        {
            public string resPath;
            public int initCount;
            public int maxCount;
            public bool isUi;
            public bool isAsync;
            public int reallocateCount;
        }

        private Dictionary<string, Pool> m_pools = new Dictionary<string, Pool>();

        public virtual void initialize()
        {
        }

        public void createPool(CreatePoolData data, Action callback)
        {
            if (Logx.isActive)
                Logx.assert(null != data, "create pool data is null");

            createPool(data.resPath, data.initCount, data.maxCount, data.isUi, data.isAsync, data.reallocateCount, callback);
        }

        private void createPool(string resPath, int initCount, int maxCount, bool isUi, bool isAsync, int reallocateCount, Action callback = null)
        {
            if (Logx.isActive)
            {
                Logx.assert(!string.IsNullOrEmpty(resPath), "resPath is null or empty");
                Logx.assert(0 < initCount, "Invalid create pool initCount {0}", initCount);
            }

            GameObject obj = new GameObject();
            obj.transform.parent = transform;
            obj.transform.localPosition = Vector3.zero;
            obj.name = resPath;

            Pool pool = obj.AddComponent<Pool>();
            m_pools.Add(Path.GetFileName(resPath), pool);

            pool.initialize(resPath, initCount, maxCount, isUi, isAsync, reallocateCount, callback);            
        }

        private Pool getPool(string poolName)
        {
            if (!m_pools.TryGetValue(poolName, out Pool pool))
            {
                return null;
            }

            return pool;
        }

        public bool existPool(string poolName)
        {
            if (Logx.isActive)
                Logx.assert(!string.IsNullOrEmpty(poolName), "pool name is null or empty");

            return m_pools.ContainsKey(poolName);
        }

        public bool isAsyncLoading(string poolName)
        {
            if (Logx.isActive)
                Logx.assert(!string.IsNullOrEmpty(poolName), "pool name is null or empty");

            Pool pool = getPool(poolName);
            if (null == pool)
                return false;

            return pool.isAsyncLoading;
        }

        public bool isMaxCount(string poolName)
        {
            if (Logx.isActive)
                Logx.assert(!string.IsNullOrEmpty(poolName), "pool name is null or empty");

            Pool pool = getPool(poolName);
            if (null == pool)
                return true;

            return pool.isMaxCount();
        }

        public void pop<P>(string poolName, Action<P> callback) where P : PoolObject
        {
            if (Logx.isActive)
                Logx.assert(!string.IsNullOrEmpty(poolName), "pool name is null or empty");

            Pool pool = getPool(poolName);
            if (null == pool)
            {
                callback?.Invoke(default);
                return; // ;default;
            }

            pool.pop<P>(callback);
        }

        public void popParticle<P>(string poolName, Vector3 position, PoolParticle.ePlay playType, Action<P> callback) where P : PoolParticle
        {
            pop<P>(poolName, (p) =>
            {
                if (null != p)
                    p.play(position, playType);

                callback?.Invoke(p);
            });
            
            /*
            if (null == pool)
                return default;

            pool.play(position, playType);
            return pool;
            */
        }

        public void push(PoolObject poolObject)
        {
            if (Logx.isActive)
                Logx.assert(null != poolObject, "poolObject is null");

            Pool pool = getPool(poolObject.poolName);
            if (null == pool)
                return;

            pool.push(poolObject);
        }

        public void disposePool(string poolName)
        {
            Pool pool = getPool(poolName);
            if (null == pool)
                return;

            pool.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var pair in m_pools)
                {
                    pair.Value.Dispose();
                }
            }
        }
    }
}