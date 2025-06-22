using System.Collections.Generic;
using UnityHelper;

public class QuestRow : TableRow
{
    private int m_nameId;
    private int m_iconId;
    private eQuest m_resetType;
    private bool m_isFixed;
    private int m_value;
    private int m_rewardItemId;
    private int m_rewardValue;


    public int nameId => m_nameId;
    public int iconId=> m_iconId;
    public eQuest resetType => m_resetType;
    public bool isFixed => m_isFixed;
    public int value => m_value;
    public int rewardItemId => m_rewardItemId;  
    public int rewardValue => m_rewardValue;

    public override void parse(List<string> cells, List<string> dataTypes, ref int i)
    {
        base.parse(cells, dataTypes, ref i);

        m_nameId= toInt(cells, ref i);
        m_iconId = toInt(cells, ref i);
        m_resetType = (eQuest)toInt(cells, ref i);
        m_isFixed = toBool(cells, dataTypes, ref i);
        m_value = toInt(cells, ref i);
        m_rewardItemId = toInt(cells, ref i);
        m_rewardValue = toInt(cells, ref i);
    }
}

public class QuestTable : Table<QuestRow>
{
    public Dictionary<int, QuestRow> findDailyQuestRows(int questCount)
    {
        var selector = new ProbabilityHelper.Selector();
        
        Dictionary<int, QuestRow> results = new Dictionary<int, QuestRow>();
        List<QuestRow> rows = toList();

        foreach (var row in rows)
        {
            if (row.isFixed)
                results.Add(row.id, row);
            else
                selector.add(row.id, 1.0f);
        }

        for (int index = 0; index < questCount;)
        {
            var rowId = selector.selectId();
            var row = getRow(rowId) as QuestRow;
            if (!results.ContainsKey(row.id))
            {
                results.Add(row.id, row);
                index++;
            }
        }

        return results;
    }
}
