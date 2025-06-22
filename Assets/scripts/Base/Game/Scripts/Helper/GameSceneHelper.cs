using UnityEngine.Playables;
using UnityHelper;

public class GameSceneHelper : SceneHelper
{
    public class LoadColonySceneData
    {
        public int nextMoveColonyId = 0;
        
        public int snapTransportHubId = 0;
        public bool isClickOpenLab = false;
        public string toastMsg = null;
    }

    public static GameSceneHelper getInstance()
    {
        return getInstance<GameSceneHelper>();
    }

    public void asyncLoad<T>(eScene scene, bool isImmediately, int defaultState = 0, params object[] defaultArgs) where T : SceneLoader, new()
    {
        var sceneLoadData = new GameSceneLoadData
        {
            sceneName = scene.ToString(),
            defaultState = defaultState,
            defaultArgs = defaultArgs,
        };

        asyncLoad<T>(sceneLoadData, isImmediately);
    }

    public void asyncLoad<T>(eScene scene, string additiveSceneName, bool isImmediately, int defaultState = 0, params object[] defaultArgs) where T : SceneLoader, new()
    {
        var sceneLoadData = new GameSceneLoadData
        {
            sceneName = scene.ToString(),
            additiveSceneName = additiveSceneName,
            defaultState = defaultState,
            defaultArgs = defaultArgs,
        };

        asyncLoad<T>(sceneLoadData, isImmediately);
    }

    public void asyncLoad<T>(string sceneName, string additiveSceneName, bool isImmediately, int defaultState = 0, params object[] defaultArgs) where T : SceneLoader, new()
    {
        var sceneLoadData = new GameSceneLoadData
        {
            sceneName = sceneName,
            additiveSceneName = additiveSceneName,
            defaultState = defaultState,
            defaultArgs = defaultArgs,
        };

        asyncLoad<T>(sceneLoadData, isImmediately);
    }

    public void load<T>(eScene scene, bool isImmediately, int defaultState = 0, params object[] defaultArgs) where T : SceneLoader, new()
    {
        var sceneLoadData = new GameSceneLoadData
        {
            sceneName = scene.ToString(),
            defaultState = defaultState,
            defaultArgs = defaultArgs,
        };

        load<T>(sceneLoadData, isImmediately);
    }

    public void load<T>(eScene scene, string additiveSceneName, bool isImmediately, int defaultState = 0, params object[] defaultArgs) where T : SceneLoader, new()
    {
        var sceneLoadData = new GameSceneLoadData
        {
            sceneName = scene.ToString(),
            additiveSceneName = additiveSceneName,
            defaultState = defaultState,
            defaultArgs = defaultArgs,
        };

        load<T>(sceneLoadData, isImmediately);
    }

    //public void loadInitializeAppScene()
    //{
    //    var sceneLoadData = new GameSceneLoadData
    //    {
    //        sceneName = eScene.InitializeApp.ToString(),
    //    };

    //    asyncLoad<EmptySceneLoader>(sceneLoadData, true);
    //}

    public void loadStartScene(bool isAppStart, bool isImmediately)
    {
        var sceneLoadData = new StartSceneLoadData
        {
            isAppStart = isAppStart,
            sceneName = eScene.StartScene.ToString(),
        };

        if (isAppStart)
        {
            asyncLoad<GameSceneLoader>(sceneLoadData, isImmediately);
        }
        else
        {
            asyncLoad<EmptySceneLoader>(sceneLoadData, isImmediately);
        }
    }

    //public void loadRestartScene(bool isLoadTable, bool isImmediately)
    //{
    //    var sceneLoadData = new RestartSceneLoadData
    //    {
    //        sceneName = eScene.Restart.ToString(),
    //        isLoadTable = isLoadTable,
    //    };

    //    load<EmptySceneLoader>(sceneLoadData, isImmediately);
    //}

    public void loadMainScene()
    {
        // GameLocalDataHelper.getInstance().refreshSimulationGolds();

        var sceneLoadData = new GameSceneLoadData
        {
            sceneName = eScene.Main.ToString(),
        };

        asyncLoad<StartSceneLoader>(sceneLoadData, false);
    }
}
