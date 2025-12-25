using System.Collections.Generic;
using UnityHelper;

public class QuestRow : TableRow
{
    private int m_nameId;
    private int m_descId;
    private eQuestDifficulty m_difficultyType; // 퀘스트 난이도
    private eQuest m_resetType; // 초기화 유형
    private eQuestType m_questClearType;// 목표 유형
    private int m_value; // 클리어 필요 갯수
    private int m_rewardId;
    private int m_rewardValue;
    private int m_iconId;
   
    


    public int nameId => m_nameId;
    public int descId => m_descId;
    public eQuestDifficulty difficultyType => m_difficultyType;
    public eQuest resetType => m_resetType;
    public eQuestType questClearType => m_questClearType;
    public int clearCount => m_value;
    public int rewardItemId => m_rewardId;
    public int rewardValue => m_rewardValue;
    public int iconId=> m_iconId;
    
    public override void parse(List<string> cells, List<string> dataTypes, ref int i)
    {
        base.parse(cells, dataTypes, ref i);

        m_nameId = toInt(cells, ref i);
        m_descId = toInt(cells, ref i);
        m_difficultyType = (eQuestDifficulty)toInt(cells, ref i); // 퀘스트 난이도
        m_resetType = (eQuest)toInt(cells, ref i); // 초기화 유형
        m_questClearType = (eQuestType)toInt(cells, ref i);// 목표 유형
        m_value = toInt(cells, ref i); // 클리어 필요 갯수
        m_rewardId = toInt(cells, ref i);
        m_rewardValue = toInt(cells, ref i);
        m_iconId = toInt(cells, ref i);
    }
}

public class QuestTable : Table<QuestRow>
{
    public Dictionary<int, QuestRow> findLoopQuestRows(int questCount)
    {
        var selector = new ProbabilityHelper.Selector();

        Dictionary<int, QuestRow> results = new Dictionary<int, QuestRow>();
        List<QuestRow> rows = toList();

        foreach (var row in rows)
        {
            if (row.resetType == eQuest.Daily || row.resetType == eQuest.Weekly)
                selector.add(row.id, 1.0f);
            else
                results.Add(row.id, row);
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
