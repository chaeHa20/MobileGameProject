using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityHelper;

namespace UnityHelper
{
    public class CloudDataParser
    {
        [Serializable]
        public class Data
        {
            public string version;
            public string localData;
            public string prefs;
        }

        public string getData(Crypto crypto)
        {
            Data data = new Data
            {
                version = Application.version,
                localData = LocalDataHelper.instance.getCloudData(crypto),
                prefs = PlayerPrefsHelper.instance.getCloudData(crypto)
            };

            return JsonHelper.toJson(data);
        }

        public void setData(string cryptData, Crypto crypto)
        {
            Data data = JsonHelper.fromJson<Data>(cryptData);

            LocalDataHelper.instance.setCloudData(data.localData, crypto);
            PlayerPrefsHelper.instance.setCloudData(data.prefs, crypto);
        }

        public byte[] getCloudByteData(Crypto crypto)
        {
            string json = getData(crypto);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);

            return bytes;
        }

        public void setCloudByteData(byte[] bytes, Crypto crypto)
        {
            if (null == bytes)
                return;

            string json = System.Text.Encoding.UTF8.GetString(bytes);
            setData(json, crypto);
        }
    }
}