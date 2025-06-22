using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    [Serializable]
    public struct PlayerPrefsNode
    {
        public string key;
        public string value;
    }

    [Serializable]
    public class PlayerPrefsNodes
    {
        public List<PlayerPrefsNode> nodes = new List<PlayerPrefsNode>();

        public void add(string key, string value)
        {
            PlayerPrefsNode node = new PlayerPrefsNode
            {
                key = key,
                value = value
            };
            nodes.Add(node);
        }

        public void add(string key, object obj)
        {
            PlayerPrefsNode node = new PlayerPrefsNode
            {
                key = key,
                value = JsonHelper.toJson(obj, new Crypto())
            };
            nodes.Add(node);
        }
    }
}