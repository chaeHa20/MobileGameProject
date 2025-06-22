using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

namespace UnityHelper
{
    public class UIPages : MonoBehaviour
    {
        // index
        [Serializable] class SelectEvent : UnityEvent<int> { }

        [SerializeField] List<UIPage> m_pages = new List<UIPage>();
        [SerializeField] Text m_pageName = null;
        [SerializeField] UIInteractableButton m_prev = null;
        [SerializeField] UIInteractableButton m_next = null;
        [SerializeField] SelectEvent m_selectEvent = new SelectEvent();

        private int m_pageIndex = -1;

        private int pageCount => m_pages.Count;
        public int pageIndex => m_pageIndex;

        public virtual void initialize()
        {
            for (int i = 0; i < m_pages.Count; ++i)
            {
                m_pages[i].initialize(i);
            }
        }

        public void onPrev()
        {
            setPage(m_pageIndex - 1);
        }

        public void onNext()
        {
            setPage(m_pageIndex + 1);
        }

        public void setPage(int pageIndex)
        {
            if (0 <= m_pageIndex)
                m_pages[m_pageIndex].unselect();

            if (0 > pageIndex)
                pageIndex = 0;
            else if (pageCount <= pageIndex)
                pageIndex = pageCount - 1;

            if (1 >= pageCount)
            {
                m_prev.setInteractable(false);
                m_next.setInteractable(false);
            }
            else
            {
                if (0 == pageIndex)
                {
                    m_prev.setInteractable(false);
                    m_next.setInteractable(true);
                }
                else if (pageCount - 1 == pageIndex)
                {
                    m_prev.setInteractable(true);
                    m_next.setInteractable(false);
                }
                else
                {
                    m_prev.setInteractable(true);
                    m_next.setInteractable(true);
                }
            }

            m_pageIndex = pageIndex;
            m_pages[m_pageIndex].select();
            m_pageName.text = m_pages[m_pageIndex].getPageName();

            m_selectEvent?.Invoke(m_pageIndex);
        }
    }
}