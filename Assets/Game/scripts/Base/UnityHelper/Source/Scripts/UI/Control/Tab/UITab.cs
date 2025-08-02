using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

namespace UnityHelper
{
    public class UITab : UIComponent
    {
        public class BaseData { }

        /// <summary>
        /// tab id, is First
        /// </summary>
        [Serializable] class ClickEvent : UnityEvent<int, bool> { }

        [SerializeField] GameObject m_itemParent = null;
        [SerializeField] ClickEvent m_clickEvent = new ClickEvent();

        protected Dictionary<int, UITabItem> m_items = new Dictionary<int, UITabItem>();
        protected int m_selectTabItemId = -1;
        protected bool m_isFirstSetTab = true;

        public int selectTabItemId => m_selectTabItemId;

#if UNITY_EDITOR
        public GameObject itemParent { set => m_itemParent = value; }
#endif

        public virtual void initialize(BaseData baseData)
        {
            m_isFirstSetTab = true;

            initItems(baseData);
        }

        private void initItems(BaseData baseData)
        {
            m_items.Clear();

            UITabItem[] tabItems = null;
            
            if (null != m_itemParent)
                tabItems = m_itemParent.GetComponentsInChildren<UITabItem>(true);
            else
                tabItems = GetComponentsInChildren<UITabItem>(true);

            for (int i = 0; i < tabItems.Length; ++i)
            {
                tabItems[i].initialize(this, baseData);
                m_items.Add(tabItems[i].id, tabItems[i]);
            }

            if (Logx.isActive)
                Logx.assert(0 < m_items.Count, "tab item count is zero");
        }

        public virtual void resume(BaseData baseData)
        {
            foreach(var pair in m_items)
            {
                pair.Value.resume(m_selectTabItemId, baseData);
            }
        }

        public virtual void refresh(BaseData baseData)
        {
            foreach (var pair in m_items)
            {
                pair.Value.refresh(baseData);
            }
        }

        public virtual void refresh(LocalItem item)
        {
            foreach (var pair in m_items)
            {
                pair.Value.refresh(item);
            }
        }

        public virtual void change(LocalItem oldItem, LocalItem newItem)
        {
            foreach (var pair in m_items)
            {
                pair.Value.change(oldItem, newItem);
            }
        }

        protected Dictionary<int, UITabItem>.Enumerator getItemEnumerator()
        {
            return m_items.GetEnumerator();
        }

        public virtual void setTab(int tabItemId)
        {
            if (!selectTab(tabItemId))
                return;

            m_clickEvent.Invoke(tabItemId, m_isFirstSetTab);

            m_isFirstSetTab = false;
        }

        public bool selectTab(int tabItemId)
        {
            if (selectTabItemId == tabItemId) return false;

            if (Logx.isActive)
                Logx.assert(existTabItem(tabItemId), "{0} is not found", tabItemId);

            if (0 <= m_selectTabItemId)
            {
                UITabItem oldSelectTabItem = getTabItem<UITabItem>(m_selectTabItemId);
                oldSelectTabItem.unselect();
            }
            else
            {
                foreach (var pair in m_items)
                {
                    pair.Value.unselect();
                }
            }

            m_selectTabItemId = tabItemId;
            m_items[tabItemId].select();

            return true;
        }

        protected T getTabItem<T>(int tabItemId) where T : UITabItem
        {
            if (!m_items.TryGetValue(tabItemId, out UITabItem tabItem))
            {
                return null;
            }

            return tabItem as T;
        }

        private bool existTabItem(int tabItemId)
        {
            return m_items.ContainsKey(tabItemId);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach(var pair in m_items)
                {
                    pair.Value.Dispose();
                }
            }

            base.Dispose(disposing);
        }
    }
}