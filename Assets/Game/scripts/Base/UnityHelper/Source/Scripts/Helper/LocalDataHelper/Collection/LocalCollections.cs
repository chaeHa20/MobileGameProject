using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityHelper
{
    [Serializable]
    public class LocalCollections<T> where T : LocalCollection, new()
    {
        public class AddResult
        {
            public class Result
            {
                public T item;
                public long addCount;
                public bool isNew;
            }

            public List<Result> results = new List<Result>();

            public void add(T item, long addCount, bool isNew)
            {
                var result = new Result
                {
                    item = item,
                    addCount = addCount,
                    isNew = isNew,
                };

                results.Add(result);
            }

            public void forEach(Action<Result> callback)
            {
                if (null == callback)
                    return;

                foreach (var result in results)
                {
                    callback(result);
                }
            }
        }

        [SerializeField] List<T> m_collections = new List<T>();

        public int count => m_collections.Count;


        public LocalCollections()
        {

        }

        public LocalCollections(List<T> collections)
        {
            m_collections = collections;
        }

        public AddResult add(int id, LocalCollection.CollectionType type, float exp)
        {
            return add(id, type, exp);
        }

        public AddResult add(int id, LocalCollection.CollectionType type, float exp, int level)
        {
            if (Logx.isActive)
            {
                Logx.assert(0 < id, "invalid item id {0}", id);
                Logx.assert(0 <= level, "invalid level {0}", level);
            }

            var addResult = new AddResult();
            var isNew = false;

            var collection = find(id);
            if (null == collection && ((int)eCollection.Weapon != type.main || (int)eCollection.Armor != type.main))
            {
                isNew = true;
                collection = LocalCollection.create<T>(id, type, level);
                collection.totalExp = collection.totalExp + exp;
                m_collections.Add(collection);
            }
            else if (null == collection && ((int)eCollection.Rigding == type.main || (int)eCollection.PlayerCharacter == type.main))
            {
                isNew = true;
                for (var i = 0; i < count; ++i)
                {
                    collection = LocalCollection.create<T>(id, type, level);
                    m_collections.Add(collection);
                }
            }
            else
            {
                collection.totalExp = collection.totalExp + exp;
            }

            addResult.add(collection, count, isNew);

            return addResult;
        }

        public void add(T item, bool isStack)
        {
            if (Logx.isActive)
                Logx.assert(null != item, "item is null");

            if (!isStack)
            {
                if (null != find(item.id))
                {
                    if (Logx.isActive)
                        Logx.error("Failed add item, Already exist item {0}", item.type.ToString());

                    return;
                }
            }

            m_collections.Add(item);
        }

        public T find(string uuid)
        {
            if (string.IsNullOrEmpty(uuid))
                return null;

            return find((item) => { return item.uuid == uuid; });
        }

        public T find(int collectionId)
        {
            return find((collection) => { return collection.id == collectionId; });
        }

        private T find(Func<T, bool> compareFunc)
        {
            T item = (from i in m_collections
                      where compareFunc(i)
                      select i).FirstOrDefault();

            return item;
        }

        public LocalCollections<T> finds(LocalCollection.CollectionType type)
        {
            List<T> list = findList(type);
            return new LocalCollections<T>(list);
        }

        public List<T> findList(LocalCollection.CollectionType type)
        {
            List<T> list = (from i in m_collections
                            where i.type.isEqual(type)
                            select i).ToList();

            return list;
        }

        public List<T> findList(eCollection type)
        {
            List<T> list = (from i in m_collections
                            where i.type.isEqual((int)type)
                            select i).ToList();

            return list;
        }

        public List<T> findList(int collectionId)
        {
            List<T> list = (from i in m_collections
                            where i.id == collectionId
                            select i).ToList();

            return list;
        }

        public List<T> findList(Func<T, bool> compareFunc)
        {
            List<T> list = (from i in m_collections
                            where compareFunc(i)
                            select i).ToList();

            return list;
        }

        public void forEach(Action<T> callback)
        {
            if (Logx.isActive)
                Logx.assert(null != callback, "callback is null");

            foreach (var collection in m_collections)
            {
                callback(collection);
            }
        }

        public List<T>.Enumerator getEnumerator()
        {
            return m_collections.GetEnumerator();
        }

        public void remove(string uuid)
        {
            T collection = find(uuid);
            if (null == collection)
                return;

            m_collections.Remove(collection);
        }

        public void removeAll(LocalCollection.CollectionType type)
        {
            for (int i = m_collections.Count - 1; i >= 0; --i)
            {
                if (m_collections[i].type.isEqual(type))
                {
                    m_collections.RemoveAt(i);
                }
            }
        }

        public bool isFirst(string uuid)
        {
            return isEqual(uuid, 0);
        }

        public bool isLast(string uuid)
        {
            return isEqual(uuid, count - 1);
        }

        private bool isEqual(string uuid, int index)
        {
            if (0 == m_collections.Count)
                return false;

            return m_collections[index].uuid == uuid;
        }

        public T getNext(string uuid)
        {
            int index = m_collections.FindIndex(x => x.uuid == uuid);
            if (0 > index)
                return null;

            int nextIndex = index + 1;
            if (m_collections.Count <= nextIndex)
                return null;

            return m_collections[nextIndex];
        }

        public T getPrev(string uuid)
        {
            int index = m_collections.FindIndex(x => x.uuid == uuid);
            if (0 > index)
                return null;

            int prevIndex = index - 1;
            if (0 > prevIndex)
                return null;

            return m_collections[prevIndex];
        }

        public void sortById()
        {
            m_collections.Sort((v1, v2) =>
            {
                if (v1.id > v2.id) return 1;
                else if (v1.id < v2.id) return -1;
                else return 0;
            });
        }

        public LocalCollections<T> clone()
        {
            var c = new LocalCollections<T>();

            foreach (var i in m_collections)
            {
                c.add(i.clone<T>(), false);
            }

            return c;
        }
    }
}