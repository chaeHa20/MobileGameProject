using System.Collections.Generic;
using UnityHelper;

public class CollectionRow : TableRow
{
    private eCollection m_mainType;
    private int m_nameId;
    private eGrade m_grade;
    private int m_spriteId;
    private int m_modelId;
    
    public int nameId => m_nameId;
    public eCollection mainType => m_mainType;
    public eGrade grade => m_grade;
    public int spriteId => m_spriteId;
    public int modelId => m_modelId;
    public LocalCollection.CollectionType getType() => new LocalCollection.CollectionType((int)m_mainType, m_grade);

    public override void parse(List<string> cells, List<string> dataTypes, ref int i)
    {
        base.parse(cells, dataTypes, ref i);

        m_mainType = (eCollection)toInt(cells, ref i);
        m_nameId = toInt(cells, ref i);
        m_grade = (eGrade)toInt(cells, ref i);
        m_spriteId = toInt(cells, ref i);
        m_modelId = toInt(cells, ref i);
    }

    public bool isType(eCollection mainType, eGrade grade)
    {
        return m_mainType == mainType && m_grade == grade;
    }

    public bool isType(eCollection mainType)
    {
        return m_mainType == mainType;
    }
}

public class CollectionTable : Table<CollectionRow>
{
    public List<CollectionRow> findRows(LocalCollection.CollectionType itemType)
    {
        var rows = findRowsLinq<CollectionRow>((row) =>
        {
            return (int)row.mainType == itemType.main && row.grade == itemType.grade;
        });

        return rows;
    }

    public List<CollectionRow> findRows(eCollection mainType, eGrade grade)
    {
        var rows = findRowsLinq<CollectionRow>((row) =>
        {
            return row.mainType == mainType && row.grade == grade;
        });

        return rows;
    }
}
