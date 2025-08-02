using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class StartSceneLoader : SceneLoader
{
    private UIStartSceneLoader m_startSceneLoader = null;

    protected override UISceneLoader createUILoader()
    {
        m_startSceneLoader = UISceneLoader.create("UI/Prefab/UIStartSceneLoader") as UIStartSceneLoader;
        return m_startSceneLoader;
    }
}
