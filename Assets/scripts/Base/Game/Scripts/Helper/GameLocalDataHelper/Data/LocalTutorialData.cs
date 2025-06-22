using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;
using static GameSettings;

[Serializable]
public class LocalTutorialData : LocalData
{
    [SerializeField] List<LocalTutorial> m_tutorials = new List<LocalTutorial>();

    public List<LocalTutorial> tutorials => m_tutorials;

    public override void initialize(string _name, int _id)
    {
        base.initialize(_name, _id);

        initTutorials();
    }

    public override void checkValid()
    {
        base.checkValid();
        updateTutorials();
    }

    public bool isCompleteTutorials(eTutorialId tutorialId)
    {
        foreach(var tutorial in m_tutorials)
        {
            if (tutorial.tutorialType == tutorialId)
                return tutorial.isComplete;
        }

        return false;
    }

    public LocalTutorial getTutorial(eTutorialId tutorialId)
    {
        foreach (var tutorial in m_tutorials)
        {
            if (tutorial.tutorialType == tutorialId)
                return tutorial;
        }

        return null;
    }

    //public List<LocalTutorial> findTutorialsInStage(int mapId, int stageId)
    //{
    //    List<LocalTutorial> res = new List<LocalTutorial>();
    //    foreach (var tutorial in m_tutorials)
    //    {
    //        if (tutorial.targetMapId == mapId && tutorial.targetStageId == stageId && !tutorial.isComplete)
    //            res.Add(tutorial);
    //    }

    //    return res;
    //}

    private void initTutorials()
    {
        var table = GameTableHelper.instance.getTable<TutorialTable>((int)eTable.Tutorial);
        var ids = table.getIds();
        foreach(var id in ids)
        {
            addTutorail(id);
        }
    }

    private void updateTutorials()
    {
        var table = GameTableHelper.instance.getTable<TutorialTable>((int)eTable.Tutorial);
        var ids = table.getIds();
        if(ids.Length <= m_tutorials.Count)
            return;

        foreach (var id in ids)
        {
            if(!checkExistLocalTutorial(id))
            {
                addTutorail(id);
            }
        }
    }

    private bool checkExistLocalTutorial(int targetId)
    {
        foreach(var tutorial in m_tutorials)
        {
            if(tutorial.id == targetId)
                return true;
        }

        return false;
    }

    private void addTutorail(int tutorialId)
    {
        LocalTutorial tutorial = new LocalTutorial();
        var row = GameTableHelper.instance.getRow<TutorialRow>((int)eTable.Tutorial, tutorialId);
        tutorial.initialize(row);
        m_tutorials.Add(tutorial);
    }

}
