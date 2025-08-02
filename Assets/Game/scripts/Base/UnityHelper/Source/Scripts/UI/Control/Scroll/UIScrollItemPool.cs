using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public class UIScrollItemPool : IDisposable
    {
        private Transform m_poolParent = null;
        private Stack<UIScrollItem> m_scrollItems = new Stack<UIScrollItem>();

        public void push(UIScrollView scrollView, UIScrollItem scrollItem)
        {
            if (null == m_poolParent)
                createPoolParent(scrollView);

            scrollItem.gameObject.SetActive(false);
            scrollItem.transform.SetParent(m_poolParent);
            
            m_scrollItems.Push(scrollItem);
        }

        public T pop<T>() where T : UIScrollItem
        {
            if (0 == m_scrollItems.Count)
                return null;

            var item = m_scrollItems.Pop() as T;
            item.gameObject.SetActive(true);

            return item;
        }

        private void createPoolParent(UIScrollView scrollView)
        {
            GameObject poolParent = new GameObject("ScrollItemPool");
            poolParent.transform.SetParent(scrollView.transform.parent);
            m_poolParent = poolParent.transform;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach(var item in m_scrollItems)
                {
                    item.Dispose();
                }
                m_scrollItems.Clear();
            }
        }
    }
}