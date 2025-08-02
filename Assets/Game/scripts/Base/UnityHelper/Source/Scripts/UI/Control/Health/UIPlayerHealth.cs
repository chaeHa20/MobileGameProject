using System.Collections.Generic;
using UnityEngine;


namespace UnityHelper
{
    public class UIPlayerHealth : MonoBehaviour
    {
        [SerializeField] GameObject m_itemParent = null;

        private List<UIPlayerHealthItem> m_items = new List<UIPlayerHealthItem>();

        private int m_hp = 0;

        public void initiailzie()
        {
            PlayerController.instance.setPlayerHealthUI(this);
        }
        public void setHealthItems(int hp)
        {
            m_hp = hp;
            for (int index = 0; index < hp; index++)
            {
                UIPlayerHealthItem.pop(m_itemParent, index, (item) =>
                {
                    m_items.Add(item);
                });
            }
        }

        private void addExtraHealth(int extraValue)
        {
            for (int index = m_items.Count; index < extraValue; index++)
            {
                UIPlayerHealthItem.pop(m_itemParent, index, (item) =>
                {
                    item.restoreItem();
                    m_items.Add(item);
                });
            }
        }

        public void updateHealth(int hp)
        {
            var oldHp = m_hp;
            var newHp = hp;
            if (oldHp < newHp)
            {
                for (int index = oldHp; index < newHp; index++)
                {
                    if (m_items.Count <= index)
                    {
                        addExtraHealth(newHp);
                        break;
                    }

                    m_items[index].restoreItem();
                }
            }
            else if (newHp < oldHp)
            {
                for (int index = oldHp - 1; newHp <= index; index--)
                {
                    if (index < 0)
                    {
                        if (Logx.isActive)
                            Logx.error("Player Health is Negative");
                        break;
                    }

                    m_items[index].setDamageItem();
                }
            }

            if (0 <= newHp)
                m_hp = newHp;
            else
                m_hp = 0;
        }

        public void clearItems()
        {
            foreach (var item in m_items)
                item.Dispose();

            m_items.Clear();
        }

    }
}
