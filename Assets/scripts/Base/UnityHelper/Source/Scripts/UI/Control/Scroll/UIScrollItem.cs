using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public class UIScrollItem : UIComponent, IDisposable
    {
        public class BaseData
        {
            public int itemPrefabIndex = 0;
            /// <summary>
            /// null이면 m_content이다.
            /// </summary>
            public GameObject parent = null;
        }

        protected UIScrollView m_scrollView = null;
        private int m_prefabIndex = 0;
        private int m_itemIndex = 0;

        public int prefabIndex { get { return m_prefabIndex; } }
        public int itemIndex { get { return m_itemIndex; } }
        protected UIScrollView scrollView { get { return m_scrollView; } }

        public virtual void initialize(UIScrollView scrollView, BaseData baseData, int itemIndex)
        {
            m_scrollView = scrollView;
            m_itemIndex = itemIndex;

            if (null != baseData)
            {
                m_prefabIndex = baseData.itemPrefabIndex;
            }
        }

        public virtual void onClick()
        {
        }

        public virtual void refresh()
        {
        }

        public virtual void refresh(UIScrollView.BaseData baseData)
        {

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                // UIScrollView에서 siblingindex 설정 할 때 안 꼬이도록
                transform.SetParent(null);
                GameObject.Destroy(gameObject);
            }
        }
    }
}