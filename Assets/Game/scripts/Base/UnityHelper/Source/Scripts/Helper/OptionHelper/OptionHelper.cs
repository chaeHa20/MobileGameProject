using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public class OptionHelper : NonMonoSingleton<OptionHelper>
    {
        private bool m_isBgmOn = true;
        private bool m_isSfxOn = true;
        private bool m_isVibrationOn = true;
        private int m_graphicQualityLevel = -1;

        public bool isBgmOn => m_isBgmOn;
        public bool isSfxOn => m_isSfxOn;
        public bool isVibrationOn => m_isVibrationOn;
        public int graphicQualityLevel => m_graphicQualityLevel;

        public virtual void initialize()
        {
            getPrefValues();

            SoundHelper.instance.setPauseBgm(!m_isBgmOn);
            SoundHelper.instance.setPauseSfx(!m_isSfxOn);
        }

        protected virtual void getPrefValues()
        {
            m_isBgmOn = getBool(PlayerPrefsKey.Bgm, true);
            m_isSfxOn = getBool(PlayerPrefsKey.Sfx, true);
            m_isVibrationOn = getBool(PlayerPrefsKey.Vibration, true);
            m_graphicQualityLevel = PlayerPrefsHelper.instance.getInt(PlayerPrefsKey.GrahpicQualityLevel, 0);
        }

        public void setBgmOn(bool on)
        {
            m_isBgmOn = on;
            setBool(PlayerPrefsKey.Bgm, m_isBgmOn);
            SoundHelper.instance.setPauseBgm(!on);
        }

        public void setSfxOn(bool on)
        {
            m_isSfxOn = on;
            setBool(PlayerPrefsKey.Sfx, m_isSfxOn);
            SoundHelper.instance.setPauseSfx(!on);
        }

        public void setVibrationOn(bool on)
        {
            m_isVibrationOn = on;
            setBool(PlayerPrefsKey.Vibration, m_isVibrationOn);
        }

        protected bool getBool(string key, bool defaultValue = false)
        {
            return PlayerPrefsHelper.instance.getBool(key, defaultValue);
        }

        protected void setBool(string key, bool value)
        {
            PlayerPrefsHelper.instance.setBool(key, value);
        }

        protected int getInt(string key, int defaultValue = 0)
        {
            return PlayerPrefsHelper.instance.getInt(key, defaultValue);
        }

        protected void setInt(string key, int value)
        {
            PlayerPrefsHelper.instance.setInt(key, value);
        }
    }
}