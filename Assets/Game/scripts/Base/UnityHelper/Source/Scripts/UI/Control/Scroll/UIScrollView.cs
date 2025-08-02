using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UnityHelper
{
    public partial class UIScrollView : UIComponent
    {
        public class BaseData { }

        [SerializeField] float m_snapBounary = 0.002f;
        [SerializeField] GameObject m_content = null;
        [SerializeField] List<GameObject> m_itemPrefabs = new List<GameObject>();

        private ScrollRect m_scrollRect = null;
        private List<UIScrollItem> m_items = new List<UIScrollItem>();
        private UIScrollItemPool m_itemPool = new UIScrollItemPool();

        protected int itemCount { get { return m_items.Count; } }
        protected int itemPrefabCount { get { return m_itemPrefabs.Count; } }
        protected Vector3 contentLocalPosition { get { return m_content.transform.localPosition; } set { m_content.transform.localPosition = value; } }
        protected GameObject content => m_content;
        protected ScrollRect scrollRect => m_scrollRect;
        public bool isEmpty => 0 == itemCount;

        protected virtual void Awake()
        {
            m_scrollRect = GetComponent<ScrollRect>();
        }

        public virtual void initialize(BaseData baseData)
        {
        }

        public List<UIScrollItem>.Enumerator getItemEnumerator()
        {
            return m_items.GetEnumerator();
        }

        protected void resizeItem<T>(int size, int itemPrefabIndex = 0) where T : UIScrollItem
        {
            if (size < m_items.Count)
            {
                int removeCount = m_items.Count - size;
                while (0 < removeCount)
                {
                    int itemIndex = m_items.Count - 1;
                    var item = m_items[itemIndex];
                    m_itemPool.push(this, item);
                    m_items.RemoveAt(itemIndex);

                    --removeCount;
                }
            }
            else if (size > m_items.Count)
            {
                int addCount = size - m_items.Count;
                while (0 < addCount)
                {
                    T t = m_itemPool.pop<T>();
                    if (null == t)
                        t = instantiateItem<T>(itemPrefabIndex);

                    UIHelper.instance.setParent(m_content, t.gameObject, SetParentOption.notFullAndReset());
                    m_items.Add(t);

                    --addCount;
                }
            }
        }

        private T instantiateItem<T>(int itemPrefabIndex) where T : UIScrollItem
        {
            GameObject inst = GameObject.Instantiate(m_itemPrefabs[itemPrefabIndex]);
            T t = inst.GetComponent<T>();
            return t;
        }

        protected T addItem<T>(UIScrollItem.BaseData data) where T : UIScrollItem
        {
            if (Logx.isActive)
                Logx.assert(data.itemPrefabIndex < m_itemPrefabs.Count, "ItemPrefabIndex {0}", data.itemPrefabIndex);

            T t = instantiateItem<T>(data.itemPrefabIndex);
            addItem(t, data);

            return t;
        }

        protected T insertItem<T>(UIScrollItem.BaseData data, int itemIndex) where T : UIScrollItem
        {
            if (Logx.isActive)
                Logx.assert(data.itemPrefabIndex < m_itemPrefabs.Count, "ItemPrefabIndex {0}", data.itemPrefabIndex);

            T t = instantiateItem<T>(data.itemPrefabIndex);
            insertItem(t, data, itemIndex);

            return t;
        }

        protected void replaceItem<T>(UIScrollItem.BaseData data, int itemIndex) where T : UIScrollItem
        {
            if (Logx.isActive)
                Logx.assert(data.itemPrefabIndex < m_itemPrefabs.Count, "ItemPrefabIndex {0}", data.itemPrefabIndex);

            if (m_items.Count >= itemIndex)
            {
                var curItem = m_items[itemIndex];
                curItem.Dispose();
            }

            T t = instantiateItem<T>(data.itemPrefabIndex);
            setItem(t, data, itemIndex);
        }

        protected void addItem(UIScrollItem item, UIScrollItem.BaseData data)
        {
            setParentAndInit(item, data, m_items.Count);

            m_items.Add(item);
        }

        protected void insertItem(UIScrollItem item, UIScrollItem.BaseData data, int itemIndex)
        {
            setParentAndInit(item, data, itemIndex);
            item.transform.SetSiblingIndex(itemIndex);

            m_items.Insert(itemIndex, item);
        }

        protected void setItem(UIScrollItem item, UIScrollItem.BaseData data, int itemIndex)
        {
            setParentAndInit(item, data, itemIndex);
            item.transform.SetSiblingIndex(itemIndex);

            m_items[itemIndex] = item;
        }

        private void setParentAndInit(UIScrollItem item, UIScrollItem.BaseData data, int itemIndex)
        {
            var parent = m_content;
            if (null != data && null != data.parent)
                parent = data.parent;

            UIHelper.instance.setParent(parent, item.gameObject, SetParentOption.notFullAndReset());
            item.initialize(this, data, itemIndex);
        }

        protected void addFirstItem(UIScrollItem item, UIScrollItem.BaseData data)
        {
            UIHelper.instance.setParent(m_content, item.gameObject, SetParentOption.notFullAndNotReset());
            item.initialize(this, data, m_items.Count);

            m_items.Insert(0, item);
        }

        protected void addLastItem(UIScrollItem item, UIScrollItem.BaseData data)
        {
            UIHelper.instance.setParent(m_content, item.gameObject, SetParentOption.notFullAndNotReset());
            item.initialize(this, data, m_items.Count);

            m_items.Add(item);
        }

        protected void reset()
        {
            Dispose();

            var childs = m_content.GetComponentsInChildren<UIScrollItem>();
            m_items = new List<UIScrollItem>(childs);
        }

        public bool disposeItem(UIScrollItem item)
        {
            if (null == item)
                return false;

            if (m_items.Remove(item))
            {
                item.Dispose();
                return true;
            }

            return false;
        }

        protected UIScrollItem popItem(int itemIndex)
        {
            var item = m_items[itemIndex];
            m_items.RemoveAt(itemIndex);

            return item;
        }

        protected void removeItem(UIScrollItem item)
        {
            m_items.Remove(item);
        }

        public Vector2 getItemSizeDelta(int itemPrefabIndex)
        {
            if (Logx.isActive)
                Logx.assert(0 <= itemPrefabIndex, "Invalid item prefab index {0}", itemPrefabIndex);

            if (itemPrefabIndex <= m_itemPrefabs.Count) return Vector2.zero;
            return m_itemPrefabs[itemPrefabIndex].GetComponent<RectTransform>().sizeDelta;
        }

        protected int getItemIndex(UIScrollItem item)
        {
            if (Logx.isActive)
                Logx.assert(null != item, "item is null");

            return m_items.IndexOf(item);
        }

        public T getItem<T>(int itemIndex) where T : UIScrollItem
        {
            if (Logx.isActive)
            {
                Logx.assert(0 <= itemIndex, "Invalid item index {0}", itemIndex);
            }

            if (m_items.Count <= itemIndex)
                return null;

            return m_items[itemIndex] as T;
        }

        public T getLastItem<T>() where T : UIScrollItem
        {
            int lastItemIndex = itemCount - 1;
            if (0 > lastItemIndex)
                return null;

            return m_items[lastItemIndex] as T;
        }

        protected T findItem<T>(Func<T, bool> condition) where T : UIScrollItem
        {
            var t = (from i in m_items
                     where condition(i as T)
                     select i).FirstOrDefault();

            return t as T;
        }

        protected List<T> findItems<T>(Func<T, bool> condition) where T : UIScrollItem
        {
            var items = (from i in m_items
                         where condition(i as T)
                         select i as T).ToList();

            return items;
        }

        protected void clickItem(int itemIndex)
        {
            if (Logx.isActive)
            {
                Logx.assert(0 <= itemIndex, "Invalid item index {0}", itemIndex);
                Logx.assert(m_items.Count > itemIndex, "Invalid item index {0}", itemIndex);
            }

            var item = getItem<UIScrollItem>(itemIndex);
            item.onClick();
        }

        protected Vector2 getScrolledContentSizeDelta()
        {
            var content = m_scrollRect.content;
            var contentSizeDelta = content.sizeDelta;
            var viewPortRect = getViewPortRect();

            // content의 실제 스크롤되는 영역을 계산하자
            float viewWidth = viewPortRect.width;
            contentSizeDelta.x -= viewWidth * content.anchorMin.x;
            contentSizeDelta.x -= viewWidth * (1.0f - content.anchorMax.x);

            // y는 테스트 안 해봄
            float viewHeight = viewPortRect.height;
            contentSizeDelta.y -= viewHeight * content.anchorMin.y;
            contentSizeDelta.y -= viewHeight * (1.0f - content.anchorMax.y);

            return contentSizeDelta;
        }

        protected UIScrollItem getItem(int itemIndex)
        {
            if (Logx.isActive)
            {
                Logx.assert(0 <= itemIndex, "Invalid item index {0}", itemIndex);
                Logx.assert(m_items.Count > itemIndex, "Invalid item index {0}", itemIndex);
            }

            return m_items[itemIndex];
        }

        protected Rect getViewPortRect()
        {
            return m_scrollRect.viewport.rect;
        }

        /// <summary>
        /// 그리드 레이아웃인 경우에 가로 아이템 개수를 구한다.
        /// 정확한가?
        /// </summary>
        /// <returns></returns>
        public int getGridLayoutItemWidthCount()
        {
            ScrollRect scrollRect = GetComponent<ScrollRect>();
            GridLayoutGroup gridLayout = scrollRect.content.GetComponent<GridLayoutGroup>();
            if (null == gridLayout)
                return 0;

            float contentWidth = scrollRect.content.rect.width;
            float itemWidth = gridLayout.cellSize.x + gridLayout.spacing.x;
            int widthItemCount = (int)(contentWidth / itemWidth);

            return widthItemCount;
        }

        protected R find<R>(Func<R, bool> callback) where R : UIScrollItem
        {
            return m_items.FirstOrDefault(x => callback(x as R)) as R;
        }

        public virtual void refresh()
        {
            for (int i = 0; i < m_items.Count; i++)
                m_items[i].refresh();
        }

        public virtual void refresh(BaseData baseData)
        {
            for (int i = 0; i < m_items.Count; i++)
                m_items[i].refresh(baseData);
        }

        //protected virtual void stopSnap()
        //{
        //    if (null != m_coHorizontalNormalize)
        //    {
        //        StopCoroutine(m_coHorizontalNormalize);
        //        m_coHorizontalNormalize = null;
        //    }

        //    if (null != m_coVerticalNormalize)
        //    {
        //        StopCoroutine(m_coVerticalNormalize);
        //        m_coVerticalNormalize = null;
        //    }

        //    m_coSyncHorizontalScrollValueAndTabItem = null;

        //    StopAllCoroutines();
        //}

        //protected bool isSnapping()
        //{
        //    if (null != m_coHorizontalNormalize || null != m_coVerticalNormalize)
        //        return true;

        //    return false;
        //}

        void Update()
        {
            //if (Input.GetMouseButtonDown(0))
            //{
            //    if (isSnapping())
            //        stopSnap();
            //}
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                m_items.ForEach(x => x.Dispose());
                m_items.Clear();

                m_itemPool.Dispose();
            }
        }
    }
}