using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace UnityHelper
{
    public class SceneHelper : MonoSingleton<SceneHelper>, IDisposable
    {
        private SceneLoadData m_sceneLoadData = null;
        private string m_oldSceneName = null;
        private string m_oldAdditiveSceneName = null;
        private string m_curSceneName = null;
        private string m_curAdditiveSceneName = null;

        public SceneLoadData sceneLoadData { get { return m_sceneLoadData; } }

        public virtual void initialize()
        {

        }

        public void load<T>(SceneLoadData sceneLoadData, bool isImmediately) where T : SceneLoader, new()
        {
            if (isImmediately)
            {
                load(sceneLoadData, null);
            }
            else
            {
                T sceneLoader = new T();
                sceneLoader.add(new SceneLoaderLoadSceneItem("Scene Load", sceneLoadData));
                sceneLoader.start();
            }
        }

        public void load(SceneLoadData sceneLoadData, Action<float> callback)
        {
            if (Logx.isActive)
            {
                Logx.assert(null != sceneLoadData, "_loadData is null");
                Logx.assert(!string.IsNullOrEmpty(sceneLoadData.sceneName), "sceneName is null or empty");
            }

            StartCoroutine(coLoad(sceneLoadData, callback));
        }

        IEnumerator coLoad(SceneLoadData sceneLoadData, Action<float> callback)
        {
            yield return StartCoroutine(coUnload(sceneLoadData));

            m_sceneLoadData = sceneLoadData;

            if (Logx.isActive)
                Logx.trace("LoadScene {0}", sceneLoadData.sceneName);
            
            SceneManager.LoadScene(sceneLoadData.sceneName);

            if (!string.IsNullOrEmpty(sceneLoadData.additiveSceneName))
            {
                if (Logx.isActive)
                    Logx.trace("Additive LoadScene {0}", sceneLoadData.additiveSceneName);

                SceneManager.LoadScene(sceneLoadData.additiveSceneName, LoadSceneMode.Additive);
            }

            setCurSceneName(sceneLoadData.sceneName, sceneLoadData.additiveSceneName);

            callback?.Invoke(1.0f);
        }

        public T asyncLoad<T>(SceneLoadData sceneLoadData, bool isImmediately) where T : SceneLoader, new()
        {
            if (isImmediately)
            {
                asyncLoad(sceneLoadData, null);
                return null;
            }
            else
            {
                T sceneLoader = new T();
                sceneLoader.add(new SceneLoaderAsyncLoadSceneItem("Scene Load", sceneLoadData));
                sceneLoader.start();
                return sceneLoader;
            }
        }

        public void asyncLoad(SceneLoadData _loadData, Action<float> callback)
        {
            if (Logx.isActive)
            {
                Logx.assert(null != _loadData, "Invalid SceneHelper.asyncLoad parameter, loadData is null");
                Logx.assert(null != _loadData.sceneName, "Invalid SceneHelper.asyncLoad parameter, loadData.sceneName is null");
            }

            StartCoroutine(coAsyncLoad(_loadData, callback));
        }

        IEnumerator coAsyncLoad(SceneLoadData sceneLoadData, Action<float> callback)
        {
            yield return StartCoroutine(coUnload(sceneLoadData));

            m_sceneLoadData = sceneLoadData;
            setCurSceneName(sceneLoadData.sceneName, sceneLoadData.additiveSceneName);

            bool isAdditiveScene = !string.IsNullOrEmpty(sceneLoadData.additiveSceneName);

            if (Logx.isActive)
                Logx.trace("LoadSceneAsync {0}", sceneLoadData.sceneName);

            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneLoadData.sceneName);
            while (!asyncOperation.isDone)
            {
                if (1.0f > asyncOperation.progress)
                {
                    float p = asyncOperation.progress;
                    if (isAdditiveScene)
                        p *= 0.5f;

                    callback?.Invoke(p);
                }

                yield return null;
            }

            //yield return null;

            if (isAdditiveScene)
            {
                if (Logx.isActive)
                    Logx.trace("Additive LoadSceneAsync {0}", sceneLoadData.additiveSceneName);

                asyncOperation = SceneManager.LoadSceneAsync(sceneLoadData.additiveSceneName, LoadSceneMode.Additive);
                while (!asyncOperation.isDone)
                {
                    if (1.0f > asyncOperation.progress)
                    {
                        callback?.Invoke(0.5f + asyncOperation.progress * 0.5f);
                    }

                    yield return null;
                }
            }

            //yield return new WaitForSeconds(0.1f);//

            callback?.Invoke(1.0f);
        }

        IEnumerator coUnload(SceneLoadData _loadData)
        {
            disposeScene();

            if (_loadData.config.isCollectGC)
                GC.Collect();
            if (_loadData.config.isUnloadUnUsedAssets)
                yield return StartCoroutine(coUnloadUnusedAssets());
        }

        IEnumerator coUnloadUnusedAssets()
        {
            AsyncOperation asyncOperation = Resources.UnloadUnusedAssets();
            while (!asyncOperation.isDone)
            {
                yield return null;
            }
        }

        private void disposeScene()
        {
            if (Logx.isActive)
                Logx.trace("DisposeScene {0} / Additive {1}", m_curSceneName, m_curAdditiveSceneName);

            WorldScene worldScene = WorldScene.instance<WorldScene>();
            if (null != worldScene)
                worldScene.Dispose();

            UIScene uiScene = UIScene.instance<UIScene>();
            if (null != uiScene)
                uiScene.Dispose();
        }

        private void setCurSceneName(string sceneName, string additiveSceneName)
        {
            m_oldSceneName = m_curSceneName;
            m_oldAdditiveSceneName = m_curAdditiveSceneName;

            m_curSceneName = sceneName;
            m_curAdditiveSceneName = additiveSceneName;
        }

        public bool isCurrentScene(string sceneName)
        {
            if (Logx.isActive)
                Logx.assert(!string.IsNullOrEmpty(sceneName), "Invalid scene name");

            if (string.Equals(sceneName, m_curSceneName))
                return true;
            if (string.Equals(sceneName, m_curAdditiveSceneName))
                return true;

            return false;
        }

        public bool isOldScene(string sceneName)
        {
            if (Logx.isActive)
                Logx.assert(!string.IsNullOrEmpty(sceneName), "Invalid scene name");

            if (string.Equals(sceneName, m_oldSceneName))
                return true;
            if (string.Equals(sceneName, m_oldAdditiveSceneName))
                return true;

            return false;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
        }
    }
}