using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityHelper;
using UnityEngine.UI;

public class UICurrencyBigValue : UIBigValue
{
    [SerializeField] eCurrency m_currencyType = eCurrency.Gold;
    [SerializeField] eCurrencyValueId m_id = eCurrencyValueId.None;


    public eCurrency currencyType { get { return m_currencyType; } }
    public eCurrencyValueId id { get { return m_id; } }

    protected override void Awake()
    {
        base.Awake();

        // setCurrencyIcon(m_currencyType);
        
        CurrencyValueBroadcaster.instance.regist(this);
    }

    void OnDestroy()
    {
        CurrencyValueBroadcaster.instance.unregist(this);
    }
}
