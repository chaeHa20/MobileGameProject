using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class UIStartSceneLoader : UISceneLoader
{
    [SerializeField] GameObject m_debug = null;
    [SerializeField] GameObject m_biKr = null;
    [SerializeField] GameObject m_biEn = null;
    [SerializeField] GameObject m_biJp = null;

    protected virtual void Start()
    {
        initBi();
        if (null != m_debug)
            m_debug.SetActive(Debugx.isActive);
    }

    public void initBi()
    {
        var language = (eLanguage)GamePlayerPrefsHelper.instance.getInt(PlayerPrefsKey.Language, (int)eLanguage.Kr);
        if(eLanguage.Kr == language)
        {
            language = LanguageHelper.getDeviceLanguage();
        }

        if (eLanguage.Kr == language)
        {
            m_biKr.SetActive(true);
            m_biEn.SetActive(false);
            m_biJp.SetActive(false);
        }
        else if (eLanguage.Jp == language)
        {
            m_biKr.SetActive(false);
            m_biEn.SetActive(false);
            m_biJp.SetActive(true);
        }
        else
        {
            m_biKr.SetActive(false);
            m_biEn.SetActive(true);
            m_biJp.SetActive(false);
        }
    }

    protected override void destroy()
    {
        base.destroy();
        //StageScene world = StageScene.instance<StageScene>();
        //if (null != world)
        //    world.showStartWindows();
    }
}
