using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;
using System;

namespace UnityHelper
{
    public partial class PlayerPrefsKey
    {
        public const string CustomId = "CustomId";
        public const string CustomPassword = "CustomPassword";
    }
}

public class GamePlayerPrefsHelper : PlayerPrefsHelper
{
    [Serializable]
    private class CloudData : BaseCloudData
    {

    }

    public static GamePlayerPrefsHelper getInstance()
    {
        return getInstance<GamePlayerPrefsHelper>();
    }

    public override string getCloudData(Crypto crypto)
    {
        CloudData cloudData = new CloudData();
        getCloudData(cloudData);

        return JsonHelper.toJson(cloudData, crypto);
    }

    public override void setCloudData(string data, Crypto crypto)
    {
        CloudData cloudData = JsonHelper.fromJson<CloudData>(data, crypto);

        setCloudData(cloudData);
    }
}
