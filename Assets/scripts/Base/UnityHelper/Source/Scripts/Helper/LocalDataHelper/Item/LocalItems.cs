using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityHelper
{
    [Serializable]
    public class LocalItems<T> where T : LocalItem, new()
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

        [SerializeField] List<T> m_items = new List<T>();

        public int count => m_items.Count;

        public LocalItems()
        {

        }

        public LocalItems(List<T> items)
        {
            m_items = items;
        }

        public AddResult add(int id, LocalItem.Type type, long count, bool isStack, eGrade equipmetnGrade)
        {
            return add(id, type, count, 1, 1, isStack, equipmetnGrade);
        }

        public AddResult add(int id, LocalItem.Type type, long count, int level, int durability, bool isStack, eGrade equipmetnGrade)
        {
            if (Logx.isActive)
            {
                Logx.assert(0 < id, "invalid item id {0}", id);
                Logx.assert(0 <= level, "invalid level {0}", level);
                Logx.assert(0 <= durability, "invalid durability {0}", durability);
            }

            var addResult = new AddResult();
            var isNew = false;
            if (isStack)
            {
                var item = find(id);
                if (null == item && (int)eItem.Equipment != type.main)
                {
                    isNew = true;
                    item = LocalItem.create<T>(id, type, count, level, durability);
                    m_items.Add(item);
                }
                else if ((int)eItem.Equipment == type.main)
                {
                    isNew = true;
                    for (var i = 0; i < count; ++i)
                    {
                        item = LocalItem.create<T>(id, type, 1, level, durability);
                        item.grade = (int)equipmetnGrade;
                        m_items.Add(item);
                    }
                }
                else
                {
                    item.count += count;
                }

                addResult.add(item, count, isNew);
            }
            else
            {
                for (var i = 0; i < count; ++i)
                {
                    var item = LocalItem.create<T>(id, type, 1, level, durability);
                    m_items.Add(item);

                    // isStack이 아닐 때에도 isNew 체크를 해야되는지 고민이다. 우선은 항상 false로 하자.
                    addResult.add(item, 1, isNew);
                }
            }

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

            m_items.Add(item);
        }

        public T repair(int id, LocalItem.Type type, int level, int durability)
        {
            T item = find(type);
            if (null == item)
            {
                item = LocalItem.create<T>(id, type, 1, level, durability);
                m_items.Add(item);
            }
            else
            {
                if (0 > item.curDurability)
                    item.curDurability = durability;
                else
                    item.curDurability += durability;
            }

            return item;
        }

        public T find(string uuid)
        {
            if (string.IsNullOrEmpty(uuid))
                return null;

            return find((item) => { return item.uuid == uuid; });
        }

        public T find(LocalItem.Type type)
        {
            return find((item) => { return item.type.isEqual(type); });
        }

        public T find(int itemId)
        {
            return find((item) => { return item.id == itemId; });
        }

        private T find(Func<T, bool> compareFunc)
        {
            T item = (from i in m_items
                      where compareFunc(i)
                      select i).FirstOrDefault();

            return item;
        }

        public LocalItems<T> finds(LocalItem.Type type)
        {
            List<T> list = findList(type);
            return new LocalItems<T>(list);
        }

        public List<T> findList(LocalItem.Type type)
        {
            List<T> list = (from i in m_items
                            where i.type.isEqual(type)
                            select i).ToList();

            return list;
        }

        public List<T> findList(int itemId)
        {
            List<T> list = (from i in m_items
                            where i.id == itemId
                            select i).ToList();

            return list;
        }

        public List<T> findList(Func<T, bool> compareFunc)
        {
            List<T> list = (from i in m_items
                            where compareFunc(i)
                            select i).ToList();

            return list;
        }

        public void forEach(Action<T> callback)
        {
            if (Logx.isActive)
                Logx.assert(null != callback, "callback is null");

            foreach (var item in m_items)
            {
                callback(item);
            }
        }

        public List<T>.Enumerator getEnumerator()
        {
            return m_items.GetEnumerator();
        }

        public void remove(LocalItem.Type type)
        {
            T item = find(type);
            if (null == item)
                return;

            m_items.Remove(item);
        }

        public void remove(string uuid)
        {
            T item = find(uuid);
            if (null == item)
                return;

            m_items.Remove(item);
        }

        public void removeAll(LocalItem.Type type)
        {
            for (int i = m_items.Count - 1; i >= 0; --i)
            {
                if (m_items[i].type.isEqual(type))
                {
                    m_items.RemoveAt(i);
                }
            }
        }

        public T use(LocalItem.Type type, long useCount)
        {
            if (Logx.isActive)
            {
                Logx.assert(0 < useCount, "invalid item useCount {0}", useCount);
            }

            T item = find(type);
            if (null == item)
                return null;

            if (item.use(useCount))
                return item;

            return null;
        }

        public T use(int itemId, long useCount)
        {
            if (Logx.isActive)
            {
                Logx.assert(0 < useCount, "invalid item useCount {0}", useCount);
            }

            T item = find(itemId);
            if (null == item)
                return null;

            if (item.use(useCount))
                return item;

            return null;
        }

        public T use(string uuid, long useCount)
        {
            if (Logx.isActive)
            {
                Logx.assert(0 < useCount, "invalid item useCount {0}", useCount);
            }

            T item = find(uuid);
            if (null == item)
                return null;

            if (item.use(useCount))
                return item;

            return null;
        }

        public bool tryUse(LocalItem.Type type, long useCount, out T item)
        {
            if (Logx.isActive)
            {
                Logx.assert(0 < useCount, "invalid item useCount {0}", useCount);
            }

            item = find(type);
            if (null == item)
                return false;

            if (item.use(useCount))
                return true;

            return false;
        }

        public bool tryUse(int itemId, long useCount, out T item)
        {
            if (Logx.isActive)
            {
                Logx.assert(0 < useCount, "invalid item useCount {0}", useCount);
            }

            item = find(itemId);
            if (null == item)
                return false;

            if (item.use(useCount))
                return true;

            return false;
        }

        public bool tryUseAndRemove(int itemId, long useCount, out T item)
        {
            var isUsed = tryUse(itemId, useCount, out item);

            if (isUsed)
            {
                if (0 == item.count)
                    remove(item.uuid);
            }

            return isUsed;
        }

        /// <summary>
        /// 이걸 쓸때는 조심하자, type이 같은 아이템이 여러개 있을 수 있다.
        /// </summary>
        public long getCount(LocalItem.Type type)
        {
            T item = find(type);
            if (null == item)
                return 0;

            return item.count;
        }

        public long getCount(int itemId)
        {
            T item = find(itemId);
            if (null == item)
                return 0;

            return item.count;
        }

        public long getCount(string uuid)
        {
            T item = find(uuid);
            if (null == item)
                return 0;

            return item.count;
        }

        public long getAllCount(LocalItem.Type type)
        {
            long count = (from i in m_items
                          where i.type.isEqual(type)
                          select i.count).Sum();

            return count;
        }

        public bool isEnough(LocalItem.Type type, long needCount)
        {
            if (Logx.isActive)
            {
                Logx.assert(0 <= needCount, "invalid item need count {0}", needCount);
            }

            T item = find(type);
            if (null == item)
                return false;

            return item.isEnough(needCount);
        }

        public bool isEnough(int itemId, long needCount)
        {
            if (Logx.isActive)
            {
                Logx.assert(0 <= needCount, "invalid item need count {0}", needCount);
            }

            T item = find(itemId);
            if (null == item)
                return false;

            return item.isEnough(needCount);
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
            if (0 == m_items.Count)
                return false;

            return m_items[index].uuid == uuid;
        }

        public T getNext(string uuid)
        {
            int index = m_items.FindIndex(x => x.uuid == uuid);
            if (0 > index)
                return null;

            int nextIndex = index + 1;
            if (m_items.Count <= nextIndex)
                return null;

            return m_items[nextIndex];
        }

        public T getPrev(string uuid)
        {
            int index = m_items.FindIndex(x => x.uuid == uuid);
            if (0 > index)
                return null;

            int prevIndex = index - 1;
            if (0 > prevIndex)
                return null;

            return m_items[prevIndex];
        }

        public void sortById()
        {
            m_items.Sort((v1, v2) =>
            {
                if (v1.id > v2.id) return 1;
                else if (v1.id < v2.id) return -1;
                else return 0;
            });
        }

        public LocalItems<T> clone()
        {
            var c = new LocalItems<T>();

            foreach (var i in m_items)
            {
                c.add(i.clone<T>(), false);
            }

            return c;
        }
    }
}