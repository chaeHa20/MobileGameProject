using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class EmptySceneLoader : SceneLoader
{
    private UIEmptySceneLoader m_emptySceneLoader = null;

    protected override UISceneLoader createUILoader()
    {
        m_emptySceneLoader = UISceneLoader.create("UI/Prefab/UIEmptySceneLoader") as UIEmptySceneLoader;
        return m_emptySceneLoader;
    }
}
