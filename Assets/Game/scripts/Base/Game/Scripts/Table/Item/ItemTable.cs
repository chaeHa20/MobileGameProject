using System.Collections.Generic;
using UnityHelper;

public class ItemRow : TableRow
{
    private int m_nameId;
    private eItem m_mainType;
    private int m_subType;
    private bool m_isStack;
    private int m_spriteId;
    private int m_bigSpriteId;
    
    public int nameId => m_nameId;
    public eItem mainType => m_mainType;
    public int subType => m_subType;
    public bool isStack => m_isStack;
    public int spriteId => m_spriteId;
    public int bigSpriteId => (0 == m_bigSpriteId) ? m_spriteId : m_bigSpriteId;
    public LocalItem.Type getType() => new LocalItem.Type((int)m_mainType, m_subType);
    public bool isGold => eItem.Currency == m_mainType && eCurrency.Gold == (eCurrency)m_subType;
    public bool isGem => eItem.Currency == m_mainType && eCurrency.Gem == (eCurrency)m_subType;
    //
    public bool isCurrency => eItem.Currency == m_mainType;

    public override void parse(List<string> cells, List<string> dataTypes, ref int i)
    {
        base.parse(cells, dataTypes, ref i);

        m_nameId = toInt(cells, ref i);
        m_mainType = (eItem)toInt(cells, ref i);
        m_subType = toInt(cells, ref i);
        m_isStack = toBool(cells, dataTypes, ref i);
        m_spriteId = toInt(cells, ref i);
        m_bigSpriteId = toInt(cells, ref i);
    }

    public bool isType(eItem mainType, int subType)
    {
        return m_mainType == mainType && m_subType == subType;
    }

    public bool isType(eItem mainType)
    {
        return m_mainType == mainType;
    }
}

public class ItemTable : Table<ItemRow>
{
    public ItemRow findRow(LocalItem.Type itemType)
    {
        var row = findRowLinq<ItemRow>((row) =>
        {
            return (int)row.mainType == itemType.main && row.subType == itemType.sub;
        });

        return row;
    }

    public ItemRow findRow(eItem mainType, int subType)
    {
        var row = findRowLinq<ItemRow>((row) =>
        {
            return row.mainType == mainType && row.subType == subType;
        });

        return row;
    }
}
