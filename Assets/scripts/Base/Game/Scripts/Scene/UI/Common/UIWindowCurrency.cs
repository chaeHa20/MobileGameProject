using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityHelper;

public class UIWindowCurrency : UIComponent
{
    [SerializeField] UICurrencyBigValue m_gold = null;
    [SerializeField] UICurrencyBigValue m_gem = null;
    // �Ϲ��༺ �������� ������ �� ���� ���� ������ ������ ��, �� �̻��ϴ�...
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
        // ���߿� �̰� �ΰ� ������ �̱��� ���� instance�� �Ŀ� ���ο� �� �ε� �ϴ� ������ �ٲ� �� Awake�� �Ű� ��� ��
    }

    private void setActive(bool isActive)
    {
        m_gold.gameObject.SetActive(isActive);
        m_gem.gameObject.SetActive(isActive);
    }
}
