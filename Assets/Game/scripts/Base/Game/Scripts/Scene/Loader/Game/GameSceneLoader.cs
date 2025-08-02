using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class GameSceneLoader : SceneLoader
{
    protected UIGameSceneLoader m_gameSceneLoader = null;

    protected override UISceneLoader createUILoader()
    {
        m_gameSceneLoader = UISceneLoader.create("UI/Prefab/UIGameSceneLoader") as UIGameSceneLoader;
        return m_gameSceneLoader;
    }
}
