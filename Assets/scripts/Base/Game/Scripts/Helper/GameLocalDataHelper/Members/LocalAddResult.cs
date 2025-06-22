using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityHelper;

public class LocalAddResult
{
    public enum eType { None, Item, Mercenary };

    protected eType m_type = eType.None;
    public eType type => m_type;

    protected LocalAddResult(eType type)
    {
        m_type = type;
    }
}

public class LocalAddItemResult : LocalAddResult
{
    public class Item
    {
        public int id;
        public long addCount;
        public long totalCount;
        public bool isNew;
    }

    public class Currency
    {
        public LocalBigMoneyItem item;
        public BigInteger addCount;
    }

    private Dictionary<eCurrency, Currency> m_currencies = new Dictionary<eCurrency, Currency>();
    private List<Item> m_items = new List<Item>();

    public List<Item> items => m_items;

    public LocalAddItemResult() : base(eType.Item)
    {

    }

    public void addItem(LocalItem item, int value, bool isNew)
    {
        var findItem = m_items.Find(x => x.id == item.id);
        if (null == findItem)
        {
            findItem = new Item { id = item.id };
            m_items.Add(findItem);            
        }

        findItem.addCount += value;
        findItem.totalCount = item.count;
        findItem.isNew = isNew;
    }

    public void addCurrency(eCurrency currencyType, LocalBigMoneyItem item, BigInteger value)
    {
        var currency = getCurrency(currencyType);
        if (null == currency)
        {
            currency = new Currency();
            m_currencies.Add(currencyType, currency);
        }

        currency.item = item;
        currency.addCount += value;
    }

    public Currency getCurrency(eCurrency currencyType)
    {
        if (m_currencies.TryGetValue(currencyType, out Currency currency))
        {
            return currency;
        }

        return null;
    }

    public LocalBigMoneyItem getCurrencyItem(eCurrency currencyType)
    {
        if (m_currencies.TryGetValue(currencyType, out Currency currency))
        {
            return currency.item;
        }

        return null;
    }

    public bool isCurrency(eCurrency currencyType)
    {
        var currency = getCurrency(currencyType);
        if (null == currency)
            return false;

        return 0 < currency.addCount;
    }

    public List<Item>.Enumerator getItemEnumerator()
    {
        return m_items.GetEnumerator();
    }

    public Dictionary<eCurrency, Currency>.Enumerator getCurrencyEnumerator()
    {
        return m_currencies.GetEnumerator();
    }

    public bool isEmpty()
    {
        if (0 == m_currencies.Count && 0 == m_items.Count)
            return true;
        else
            return false;
    }
}
