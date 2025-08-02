using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace UnityHelper
{
    public class UITabItem : UIComponent
    {
        [Serializable]
        public class ItemInfo
        {
            public Color bgColor = Color.white;
            public Color iconColor = Color.white;
            public Color textColor = Color.white;
        }

        [SerializeField] int m_id = 0;
        [SerializeField] Image m_bg = null;
        [SerializeField] Image m_icon = null;
        [SerializeField] TextSelector m_text = new TextSelector();
        [SerializeField] protected UITabContent m_content = null;
        [SerializeField] UIInteractableButton m_button = null;
        [SerializeField] ItemInfo m_selectInfo = new ItemInfo();
        [SerializeField] ItemInfo m_unselectInfo = new ItemInfo();

        private UITab m_tab = null;

        public int id { get { return m_id; } }
        public UITab tab { get { return m_tab; } }
        protected string text { set { m_text.text = value; } }
        protected Color textColor { set { m_text.color = value; } }
        protected ItemInfo selectInfo => m_selectInfo;
        protected ItemInfo unselectInfo => m_unselectInfo;

#if UNITY_EDITOR
        public Image bg { set => m_bg = value; }
        public UIInteractableButton button { set => m_button = value; }
        public UITabContent content { set => m_content = value; }
#endif
        public virtual void initialize(UITab tab, UITab.BaseData baseData)
        {
            if (Logx.isActive)
                Logx.assert(null != tab, "tab is null");

            gameObject.SetActive(true);

            m_tab = tab;

            if (null != m_content)
                m_content.initialize(id, baseData);
        }

        public virtual void unselect()
        {
            setActiveContent(false);
            setInfo(m_unselectInfo);
            if (null != m_content)
                m_content.unselect();
        }

        public virtual void select()
        {
            setActiveContent(true);
            setInfo(m_selectInfo);
            if (null != m_content)
                m_content.select();
        }

        protected void setActiveContent(bool isActive)
        {
            if (null != m_content)
                m_content.gameObject.SetActive(isActive);
        }

        protected virtual void setInfo(ItemInfo info)
        {
            if (null != m_bg)
            {
                m_bg.color = info.bgColor;
            }

            if (null != m_icon)
            {
                m_icon.color = info.iconColor;
            }

            if (null != m_text)
            {
                m_text.color = info.textColor;
            }
        }

        public virtual void onClick()
        {
            if (Logx.isActive)
                Logx.assert(null != m_tab, "m_tab is null");

            m_tab.setTab(m_id);
        }

        public void setInteractable(bool isInteractable)
        {
            if (null == m_button)
                return;

            m_button.setInteractable(isInteractable);
        }

        public virtual void resume(int selectTabItemId, UITab.BaseData baseData)
        {
            if (null != m_content)
                m_content.resume(selectTabItemId, baseData);
        }

        public virtual void refresh(UITab.BaseData baseData)
        {
            if (null != m_content)
                m_content.refresh(baseData);
        }

        public virtual void refresh(LocalItem item)
        {
            if (null != m_content)
                m_content.refresh(item);
        }

        public virtual void change(LocalItem oldItem, LocalItem newItem)
        {
            if (null != m_content)
                m_content.change(oldItem, newItem);
        }

        public T getContent<T>() where T : UITabContent
        {
            return m_content.GetComponent<T>();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (null != m_content)
                    m_content.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}