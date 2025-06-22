using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityHelper;
using System;
using System.Numerics;

public class UISynthesisReturnCurrencyToastMsg : UIGameToastMsg
{
    [Serializable]
    public class ReturnCurrency
    {
        public GameObject gameObject;
        public Image icon;
        public Text count;
    }

    [SerializeField] ReturnCurrency m_returnCurrency = new ReturnCurrency();

    public static void createSynthesisReturnToast(BigInteger returnCurrency, float showTime = 3.0f)
    {
        var msg = StringHelper.get("return_mercenary_synthesis_currency");
        GameObject parent = UIHelper.instance.canvasGroup.getLastSafeArea();

        GamePoolHelper.getInstance().pop<UISynthesisReturnCurrencyToastMsg>(eResource.UISynthesisReturnCurrencyToastMsg, (t) =>
        {
            if (null != t)
            {
                t.initialize(parent, null, msg, showTime, null);
                t.setReturnCurrency(returnCurrency);
            }
        });
    }

    private void setReturnCurrency(BigInteger returnCurrency)
    {
        m_returnCurrency.gameObject.SetActive(0 < returnCurrency);

        if (0 < returnCurrency)
        {
            m_returnCurrency.icon.sprite = GameResourceHelper.getInstance().getGemSprite();
            m_returnCurrency.count.text = StringHelper.toX(returnCurrency);
        }
    }
}
