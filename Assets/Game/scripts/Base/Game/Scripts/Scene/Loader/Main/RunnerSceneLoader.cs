using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class RunnerSceneLoader : SceneLoader
{
    private UIRunnerSceneLoader m_runnerSceneLoader = null;

    protected override UISceneLoader createUILoader()
    {
        m_runnerSceneLoader = UISceneLoader.create("UI/Prefab/UIRunnerSceneLoader") as UIRunnerSceneLoader;
        return m_runnerSceneLoader;
    }
}
