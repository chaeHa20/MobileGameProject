using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public class SceneLoaderItem
    {
        private string m_name = null;

        public string name { get { return m_name; } }

        public SceneLoaderItem(string _name)
        {
            m_name = _name;
        } 

        public virtual void run(Action<float> callback)
        {

        }
    }

    public class SceneLoaderLoadSceneItem : SceneLoaderItem
    {
        private SceneLoadData m_loadData = null;

        public SceneLoaderLoadSceneItem(string name, SceneLoadData loadData) : base(name)
        {
            m_loadData = loadData;
        }

        public override void run(Action<float> callback)
        {
            SceneHelper.instance.load(m_loadData, callback);
        }
    }

    public class SceneLoaderAsyncLoadSceneItem : SceneLoaderItem
    {
        private SceneLoadData m_loadData = null;

        public SceneLoaderAsyncLoadSceneItem(string name, SceneLoadData loadData) : base(name)
        {
            m_loadData = loadData;
        }

        public override void run(Action<float> callback)
        {
            SceneHelper.instance.asyncLoad(m_loadData, callback);
        }
    }
}