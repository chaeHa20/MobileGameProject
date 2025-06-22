using UnityEngine;
using UnityEngine.UI;

namespace UnityHelper
{
    public class UITabItemGrade : UITabItem
    {
        [SerializeField] GameObject m_count = null;
        [SerializeField] Text m_countText = null;
        [SerializeField] eGrade m_grade = eGrade.None;

        private bool m_isSelected = false;

        public bool isSelected => m_isSelected;

        public override void initialize(UITab tab, UITab.BaseData baseData)
        {
            base.initialize(tab, baseData);
            unselect();

            m_count.SetActive(false);
            setInfo(unselectInfo);
        }

        public void setItemCount(int count)
        {
            m_countText.text = count.ToString();
            if(0 == count)
            {
                m_isSelected = false;
                setUnselected();
            }
        }

        public override void unselect()
        {
            if (null != m_content)
                m_content.unselect();
        }

        public override void select()
        {
            if (null != m_content)
                m_content.select();
        }

        public void onClickItem()
        {
            m_isSelected = !m_isSelected;
            
            if (m_isSelected)
            {
                setSelected();
            }
            else
            {
                setUnselected();
            }
        }

        private void setUnselected()
        {
            setActiveContent(m_isSelected);
            m_count.SetActive(m_isSelected);
            unselect();
            setInfo(unselectInfo);
        }

        private void setSelected()
        {
            setActiveContent(m_isSelected);
            m_count.SetActive(m_isSelected);
            select();
            setInfo(selectInfo);
        }
    }
}