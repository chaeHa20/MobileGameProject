using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityHelper;

public class UIWindowCurrency : UIComponent
{
    [SerializeField] UICurrencyBigValue m_gold = null;
    [SerializeField] UICurrencyBigValue m_gem = null;
    // 일반행성 씬에서는 닫히면 열 수가 없기 때문에 변수를 둠, 영 이상하다...
    [SerializeField] bool m_isEnableClose = true;
    
    private void Awake()
    {
        registMessage((int)eUIMessage.UpdateCurrency, updateCurrencyCallBack);
    }

    private void updateCurrencyCallBack(List<object> datas)
    {
        GameLocalDataHelper.getInstance().requestGetCurrency((res) =>
        {
            m_gold.setValue(res.gold.count, false);
            m_gem.setValue(res.gem.count, false);            
        });
    }

    public void setCurrencies()
    {
        GameLocalDataHelper.getInstance().requestGetCurrency((res) =>
        {
            m_gold.setValue(res.gold.count, false);
            m_gem.setValue(res.gem.count, false);
        });
        // registMessage((int)eUIMessage.UpdateCurrency, updateCurrencyCallBack);
        // 나중에 이거 로고 씬에서 싱글톤 따로 instance한 후에 새로운 씬 로드 하는 구조로 바꿀 때 Awake로 옮겨 줘야 함
    }

    private void setActive(bool isActive)
    {
        m_gold.gameObject.SetActive(isActive);
        m_gem.gameObject.SetActive(isActive);
    }
}
