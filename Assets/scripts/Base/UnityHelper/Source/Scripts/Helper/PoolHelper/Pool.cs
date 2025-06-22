using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace UnityHelper
{
    public class Pool : Disposable
    {
        private GameObject m_object = null;
        private Stack<PoolObject> m_instances = new Stack<PoolObject>();
        private Dictionary<long, PoolObject> m_pops = new Dictionary<long, PoolObject>();
        private UUID m_uuid = new UUID();
        private string m_poolName = "";
        private int m_maxCount = -1;
        private bool m_isUi = false;
        private ResourceRequest m_resourceRequest = null;
        private int m_reallocateCount = 2;

        public bool isAsyncLoading { get { return null != m_resourceRequest; } }

        public static bool isDebug = false;

        public void initialize(string resPath, int initCount, int maxCount, bool isUi, bool isAsync, int reallocateCount, Action callback)
        {
            if (Logx.isActive)
            {
                //Logx.trace("Initialize pool resPath {0}, initCount {1}, isAsync {2}", resPath, initCount, isAsync);
                Logx.assert(!string.IsNullOrEmpty(resPath), "resPath is null or empty");
                Logx.assert(0 <= initCount, "Invalid initCount {0}", initCount);
            }

            m_poolName = Path.GetFileName(resPath);
            m_maxCount = maxCount;
            m_isUi = isUi;
            m_reallocateCount = reallocateCount;

            if (isAsync)
            {
                loadResourceAsync(resPath);
                StartCoroutine(coInstantiatePoolAsync(initCount, callback));
            }
            else
            {
                loadResource(resPath);
                instantiatePool(initCount);
                callback?.Invoke();
            }
        }

        private void instantiatePool(int count)
        {
            if (null == m_object)
            {
                if (Logx.isActive)
                    Logx.warn("failed instantiatePool, m_object is null {0}", m_poolName);
                return;
            }

            for (int i = 0; i < count; ++i)
            {
                if (isMaxCount())
                {
                    if (Logx.isActive)
                        Logx.warn("pool is maxcount, pool name {0}, max count {1}", m_poolName, m_maxCount);
                    break;
                }

                GameObject obj = GameObject.Instantiate<GameObject>(m_object);
                obj.name = m_poolName;

                PoolObject poolObject = obj.GetComponent<PoolObject>();
                if (Logx.isActive)
                    Logx.assert(null != poolObject, "Failed find PoolObject Component in {0}", m_poolName);

                poolObject.initializePool(m_uuid.make(), m_poolName);
                push(poolObject);
            }
        }

        public bool isMaxCount()
        {
            if (0 > m_maxCount)
                return false;

            return m_maxCount <= m_instances.Count + m_pops.Count;
        }

        private void reallocatePool()
        {
            instantiatePool(m_reallocateCount);
        }

        private void loadResource(string resPath)
        {
            m_object = Resources.Load<GameObject>(resPath);
            if (Logx.isActive)
                Logx.assert(null != m_object, "Failed load resource {0}", resPath);
        }

        private void loadResourceAsync(string resPath)
        {
            m_resourceRequest = Resources.LoadAsync<GameObject>(resPath);
        }

        IEnumerator coInstantiatePoolAsync(int initCount, Action callback)
        {
            while (!m_resourceRequest.isDone)
            {
                yield return null;
            }

            m_object = m_resourceRequest.asset as GameObject;
            m_resourceRequest = null;

            instantiatePool(initCount);
            callback?.Invoke();
        }

        IEnumerator coPopAsync<T>(Action<T> callback) where T : PoolObject
        {
            while (isAsyncLoading)
            {
                yield return null;
            }

            pop<T>(callback);
        }

        public void pop<T>(Action<T> callback) where T : PoolObject
        {
            if (isAsyncLoading)
            {
                StartCoroutine(coPopAsync<T>(callback));
                return;
            }

            if (0 == m_instances.Count)
            {
                reallocatePool();
            }

            if (0 == m_instances.Count)
            {
                callback?.Invoke(default);
                return;
            }

            PoolObject obj = m_instances.Pop();
            obj.gameObject.SetActive(true);

            //if (Logx.isActive)
            //    Logx.trace("<=========== pop {0}, {1}, {2}", obj.poolUuid, m_poolName, obj.GetInstanceID());

            if (m_pops.ContainsKey(obj.poolUuid))
            {
                if (Logx.isActive)
                    Logx.warn("Failed add pop, already exist {0}, {1}", obj.poolName, obj.poolUuid);
            }
            else
            {
                m_pops.Add(obj.poolUuid, obj);
            }

            callback?.Invoke(obj.GetComponent<T>());

        }

        public void push(PoolObject poolObject)
        {
            if (Logx.isActive)
            {
                //Logx.trace("===========> push {0}, {1}, {2}", poolObject.poolUuid, m_poolName, poolObject.GetInstanceID());
                Logx.assert(null != poolObject, "poolObject is null");
            }

            try
            {
                /// 가끔 null 인경우가 있다. 흠,
                if (null == poolObject.transform)
                {
                    if (Logx.isActive)
                        Logx.error("Failed push, poolObject.transform is null : {0}", poolObject.poolName);
                }
                else
                {
                    if (m_isUi)
                        poolObject.transform.SetParent(transform);
                    else
                        poolObject.transform.parent = transform;
                    poolObject.transform.localPosition = Vector3.zero;
                    poolObject.transform.localScale = Vector3.one;
                    poolObject.pushRestore();
                    poolObject.StopAllCoroutines();
                    poolObject.gameObject.SetActive(false);

                    if (!isExistInstance(poolObject))
                    {
                        m_instances.Push(poolObject);
                    }

                    if (m_pops.ContainsKey(poolObject.poolUuid))
                    {
                        m_pops.Remove(poolObject.poolUuid);
                    }
                }
            }
            catch(Exception e)
            {
                if (Logx.isActive)
                {
                    Logx.error("Error Pool push {0}", m_poolName);
                    Logx.exception(e);                    
                }
            }
        }

        private bool isExistInstance(PoolObject poolObject)
        {
            var e = m_instances.GetEnumerator();
            while (e.MoveNext())
            {
                if (e.Current.poolUuid == poolObject.poolUuid)
                {
                    return true;
                }
            }

            return false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //
                //  pop to push
                //
                var e = m_pops.Keys;
                long[] keys = new long[e.Count];
                e.CopyTo(keys, 0);

                for (int i = 0; i < keys.Length; ++i)
                {
                    if (m_pops.TryGetValue(keys[i], out PoolObject poolObject))
                    {
                        if (null == poolObject)
                        {
                            if (Logx.isActive)
                            {
                                Logx.warn("Failed dispose push pool, {0} is null", m_poolName);
                            }

                            m_pops.Remove(keys[i]);
                        }
                        else
                        {
                            push(poolObject);
                        }
                    }
                }

                if (Logx.isActive)
                    Logx.assert(0 == m_pops.Count, "Invalid last pool count is {0}", m_pops.Count);
            }
        }
    }
}