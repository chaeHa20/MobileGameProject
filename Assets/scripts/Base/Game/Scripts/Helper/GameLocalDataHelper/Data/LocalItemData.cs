using System;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

[Serializable]
public class LocalItemData : LocalData
{
    [Serializable]
    public class _Dic
    {
        public int id;
        public int count;

        public _Dic(int _id, int _count)
        {
            id = _id;
            count = _count;
        }
    }

    [SerializeField] LocalItems<LocalItem> m_items = new LocalItems<LocalItem>();

    public LocalItems<LocalItem> items => m_items;


    public override void initialize(string _name, int _id)
    {
        base.initialize(_name, _id);

        setDefaultItems();
    }

    public override void checkValid()
    {
        base.checkValid();
    }

    private void setDefaultItems()
    {
    }

    public bool isEnoughIEquipment(int id, int needCount)
    {
        var items = m_items.findList(id);

        if (0 == items.Count)
            return false;
        else
        {
            var totalCount = 0;
            foreach (var item in items)
            {
                totalCount += (int)item.count;
            }
            return totalCount >= needCount;
        }
    }

    public LocalItems<LocalItem>.AddResult addItem(ItemRow itemRow, long itemCount, eGrade equipmetnGrade = eGrade.None)
    {
        if (eItem.Currency == itemRow.mainType)
        {
            if (Logx.isActive)
                Logx.error("재화 아이템은 CurrencyData에 추가 되어야 됩니다.");

            return null;
        }
        else
        {
            return m_items.add(itemRow.id, itemRow.getType(), itemCount, itemRow.isStack, equipmetnGrade);
        }
    }

    public void useEquipment(int itemId)
    {
        var findItems = m_items.findList(itemId);

        foreach (var item in findItems)
        {
            if (item.id == itemId && (int)item.count > 0)
            {
                m_items.remove(item.uuid);
                return;
            }
        }
    }

    public void removeItme(string uuid)
    {
        if (null != m_items.find(uuid))
            m_items.remove(uuid);
    }

    public void checkLogin(bool isNextDayLogin)
    {
        if (isNextDayLogin)
        {

        }
    }

    public override void setDailyReset()
    {
        var e = m_items.getEnumerator();
        while (e.MoveNext())
        {
            var item = e.Current;

        }
    }
}