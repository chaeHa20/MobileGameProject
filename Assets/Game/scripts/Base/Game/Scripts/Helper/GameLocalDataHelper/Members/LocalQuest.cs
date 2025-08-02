using System;
using UnityEngine;

[Serializable]
public class LocalQuest
{
    [SerializeField] int m_questId;
    [SerializeField] eQuest m_resetType;
    [SerializeField] eQuestType m_questType;
    [SerializeField] int m_questValue;
    [SerializeField] int m_curValue;
    [SerializeField] bool m_isShowClearNotification;
    [SerializeField] bool m_isAvaliableQuest = false;
    [SerializeField] bool m_isGetReward = false;


    public int questId => m_questId;
    public eQuest resetType => m_resetType;
    public eQuestType questType => m_questType;
    public int questValue => m_questValue;
    public int curValue => m_curValue;
    public bool isShowClearNotification => m_isShowClearNotification;
    public bool isAvaliableQuest => m_isAvaliableQuest;
    public bool isGetReward => m_isGetReward;

    public void initialize(QuestRow questRow, int curValue)
    {
        m_questId = questRow.id;
        m_questType = (eQuestType)questRow.id;
        m_resetType = questRow.resetType;
        m_questValue = questRow.value;
        m_curValue = curValue;
        m_isShowClearNotification = true;
        m_isAvaliableQuest = true;
        m_isGetReward = false;
    }

    public void setEndQuiest(QuestRow questRow)
    {
        m_questId = questRow.id;
        m_questType = eQuestType.None;
        m_resetType = questRow.resetType;
        m_questValue = 0;
        m_curValue = 0;
        m_isShowClearNotification = false;
        m_isAvaliableQuest = false;
        m_isGetReward = true;
    }

    public void check(eQuestType questType, int addValue, ref bool isUpdate)
    {
        isUpdate = false;
        if (m_questType != questType)
            return;

        if (isClear())
            return;

        if (!m_isAvaliableQuest)
            return;

        m_curValue += addValue;
        isUpdate = true;

        if(m_curValue > m_questValue)
            m_curValue = m_questValue;
    }

    public bool isClear()
    {
        return m_curValue >= m_questValue;
    }

    public void setGetReward()
    {
        m_isGetReward = true;
    }

    public void hideClearNotification()
    {
        m_isShowClearNotification = false;
    }
}
