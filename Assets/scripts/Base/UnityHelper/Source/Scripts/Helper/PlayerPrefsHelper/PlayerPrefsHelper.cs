using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public partial class PlayerPrefsKey
    {
        public const string Language = "Language";
        public const string Bgm = "Bgm";
        public const string Sfx = "sfx";
        public const string Vibration = "Vibration";
        public const string GrahpicQualityLevel = "GrahpicQualityLevel";
    }

    public class PlayerPrefsHelper : MonoSingleton<PlayerPrefsHelper>
    {
        [Serializable]
        protected class BaseCloudData
        {
            public bool AutoGpgGameCentreLogin;
            public bool Bgm;
            public bool Sfx;
            public bool Vibration;
            public bool Language;
            public int GrahpicQualityLevel;
        }

        public int getInt(int key, int defaultValue = 0)
        {
            return getInt(key.ToString(), defaultValue);
        }

        public bool getBool(int key, bool defaultValue = false)
        {
            return getBool(key.ToString(), defaultValue);
        }

        public float getFloat(int key, float defaultValue = 0.0f)
        {
            return getFloat(key.ToString(), defaultValue);
        }

        public string getString(int key, string defaultValue = null)
        {
            return getString(key.ToString(), defaultValue);
        }

        public void setInt(int key, int value)
        {
            setInt(key.ToString(), value);
        }

        public void setBool(int key, bool value)
        {
            setBool(key.ToString(), value);
        }

        public void setFloat(int key, float value)
        {
            setFloat(key.ToString(), value);
        }

        public void setString(int key, string value)
        {
            setString(key.ToString(), value);
        }

        public int getInt(string key, int defaultValue = 0)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        public bool getBool(string key, bool defaultValue = false)
        {
            int i_defaultValue = (defaultValue) ? 1 : 0;
            return (1 == PlayerPrefs.GetInt(key, i_defaultValue));
        }

        public float getFloat(string key, float defaultValue = 0.0f)
        {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }

        public string getString(string key, string defaultValue = null)
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }

        public Vector3 getVector3(string key)
        {
            string json = getString(key);
            if (string.IsNullOrEmpty(json))
                return Vector3.zero;

            return JsonHelper.fromJson<Vector3>(json);
        }

        public void setInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
            PlayerPrefs.Save();
        }

        public bool incInt(string key, int maxValue)
        {
            int value = getInt(key, 0);
            if (maxValue <= value)
                return false;

            setInt(key, value + 1);
            return true;
        }

        public void setBool(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
            PlayerPrefs.Save();
        }

        public void setFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
            PlayerPrefs.Save();
        }

        public void setString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
            PlayerPrefs.Save();
        }

        public void setVector3(string key, Vector3 value)
        {
            string json = JsonHelper.toJson(value);
            setString(key, json);
        }

        public bool hasKey(int key)
        {
            return PlayerPrefs.HasKey(key.ToString());
        }

        public bool hasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        public virtual void setCloudData(string data, Crypto crypto)
        {
            if (Logx.isActive)
                Logx.error("Need override setCloudData()");
        }

        protected void getCloudData(BaseCloudData cloudData)
        {
            cloudData.Bgm = getBool(PlayerPrefsKey.Bgm, true);
            cloudData.Sfx = getBool(PlayerPrefsKey.Sfx, true);
            cloudData.Vibration = getBool(PlayerPrefsKey.Vibration, true);
            cloudData.GrahpicQualityLevel = getInt(PlayerPrefsKey.GrahpicQualityLevel, 0);
        }

        public virtual string getCloudData(Crypto crypto)
        {
            if (Logx.isActive)
                Logx.error("Need override getCloudData()");

            return null;
        }

        protected void setCloudData(BaseCloudData cloudData)
        {
            setBool(PlayerPrefsKey.Bgm, cloudData.Bgm);
            setBool(PlayerPrefsKey.Sfx, cloudData.Sfx);
            setBool(PlayerPrefsKey.Vibration, cloudData.Vibration);
            setInt(PlayerPrefsKey.GrahpicQualityLevel, cloudData.GrahpicQualityLevel);
        }
    }
}