using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public class UITabContent : UIComponent
    {
        private int m_tabId = 0;

        protected int tabId => m_tabId;

        public virtual void initialize(int tabId, UITab.BaseData baseData)
        {
            m_tabId = tabId;

            gameObject.SetActive(true);
        }

        public virtual void select()
        {

        }

        public virtual void unselect()
        {

        }

        public virtual void resume(int selectTabItemId, UITab.BaseData data)
        {

        }

        public virtual void refresh(UITab.BaseData data)
        {

        }

        public virtual void refresh(LocalItem item)
        {

        }

        public virtual void change(LocalItem oldItem, LocalItem newItem)
        {

        }
    }
}