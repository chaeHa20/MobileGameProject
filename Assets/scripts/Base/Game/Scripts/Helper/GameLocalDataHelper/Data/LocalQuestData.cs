using System;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

[Serializable]
public class LocalQuestData : LocalData
{
    [SerializeField] List<LocalQuest> m_quests = new List<LocalQuest>();
    [SerializeField] bool m_isQuestOpen = false;

    public List<LocalQuest> quests => m_quests;
    public bool IsQuestOpen => m_isQuestOpen;

    public override void initialize(string _name, int _id)
    {
        base.initialize(_name, _id);

        m_isQuestOpen = false;
        initQuest();
    }

    public override void checkValid()
    {
        base.checkValid();
        initQuest();
    }

    private void initQuest()
    {
        var questTable = GameTableHelper.instance.getTable<QuestTable>((int)eTable.Quest);
        var ids = questTable.getIds();
        foreach (var id in ids)
        {
            var questRow = GameTableHelper.instance.getRow<QuestRow>((int)eTable.Quest, id);
            LocalQuest quest = new LocalQuest();
            quest.initialize(questRow, 0);
            if (!checkExistQuest(questRow.id))
                m_quests.Add(quest);
        }
    }

    public List<LocalQuest> getTodayQuests()
    {
        List<LocalQuest> todayQuests = new List<LocalQuest>();
        foreach (var quest in m_quests)
        {
            if (quest.isAvaliableQuest)
                todayQuests.Add(quest);
        }

        return todayQuests;
    }

    private bool checkExistQuest(int questId)
    {
        foreach (var quest in m_quests)
        {
            if (quest.questId == questId)
                return true;
        }

        return false;
    }

    public void openQuest()
    {
        if (!m_isQuestOpen)
        {
            m_isQuestOpen = true;
            setDailyReset();
        }
    }

    public LocalQuest fineQuest(eQuestType questType)
    {
        foreach (var quest in m_quests)
        {
            if (quest.questType == questType && quest.isAvaliableQuest)
                return quest;
        }

        return null;
    }

    public override void setDailyReset()
    {
        base.setDailyReset();

        var questTable = GameTableHelper.instance.getTable<QuestTable>((int)eTable.Quest);
        var gameSettingTable = GameTableHelper.instance.getTable<GameSettingTable>((int)eTable.GameSetting);

        foreach (var quest in m_quests)
        {
            if (eQuest.Daily == quest.resetType)
            {
                var row = GameTableHelper.instance.getRow<QuestRow>((int)eTable.Quest, quest.questId);
                quest.initialize(row, 0);
            }
        }
    }
}
