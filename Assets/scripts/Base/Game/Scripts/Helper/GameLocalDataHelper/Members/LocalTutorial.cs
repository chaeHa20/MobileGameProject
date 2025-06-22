using System;
using UnityEngine;

[Serializable]
public class LocalTutorial
{
    [SerializeField] int m_id;
    [SerializeField] eTutorialId m_tutorialType;
    [SerializeField] eTutorialId m_nextTutorialType;
    [SerializeField] int m_targetMapId;
    [SerializeField] int m_targetStageId;
    [SerializeField] bool m_isComplete = false;

    public int id => m_id;
    public eTutorialId tutorialType => m_tutorialType;
    public eTutorialId nextTutorialType => m_nextTutorialType;

    public int targetMapId => m_targetMapId;
    public int targetStageId => m_targetStageId;
    public bool isComplete => m_isComplete;


    public void initialize(TutorialRow row)
    {
        m_id = row.id;
        m_tutorialType = (eTutorialId)row.id;
        m_nextTutorialType = (eTutorialId)row.nextId;
        m_targetMapId = row.spawnMapId;
        m_targetStageId = row.spawnStageId;
        m_isComplete = false;

#if UNITY_EDITOR
        if (DebugSettings.instance.isSkipTutorial)
            setCompleteTutorial();
#endif
    }

    public void setCompleteTutorial()
    {
        m_tutorialType = eTutorialId.None;
        m_nextTutorialType = eTutorialId.None;
        m_targetMapId = 0;
        m_targetStageId = 0;
        m_isComplete = true;
    }
}
