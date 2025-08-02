using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    [Serializable]
    public class LocalDatas
    {
        [Serializable]
        public struct LocalData
        {
            public string name;
            public string data;
        }

        private List<LocalData> datas = new List<LocalData>();

        public static void fromJson(string localData, Crypto crypto)
        {
            string jsonData = (crypto.isEmpty()) ? localData : AES.Decode(localData, crypto);
            LocalDatas datas = JsonHelper.fromJson<LocalDatas>(jsonData);
            datas.save();
        }

        public static string toJson(LocalDatas localDatas, Crypto crypto)
        {
            string jsonData = JsonHelper.toJson(localDatas);
            return (crypto.isEmpty()) ? jsonData : AES.Encode(jsonData, crypto);
        }

        private void save()
        {
            foreach (var data in datas)
            {
                PlayerPrefs.SetString(data.name, data.data);
            }
        }

        public void add(string name, object obj)
        {
            LocalData data = new LocalData
            {
                name = name,
                data = JsonHelper.toJson(obj, true)
            };
            datas.Add(data);
        }
    }

    public interface ILocalData
    {
        void setLocalDatas(string text, Crypto crypto);
        string getLocalDatas(Crypto crypto);
        void clearLocalDatas();
    }
}