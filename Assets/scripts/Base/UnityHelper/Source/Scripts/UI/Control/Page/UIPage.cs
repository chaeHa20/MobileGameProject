using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public class UIPage : UIComponent
    {
        private int m_index = 0;

        public int index => m_index;

        public virtual void initialize(int index)
        {
            m_index = index;
        }

        public virtual string getPageName()
        {
            return "";
        }

        public virtual void select()
        {
            gameObject.SetActive(true);
        }

        public virtual void unselect()
        {
            gameObject.SetActive(false);
        }
    }
}