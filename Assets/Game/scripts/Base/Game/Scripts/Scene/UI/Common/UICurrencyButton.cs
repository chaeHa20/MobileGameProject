using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityHelper;

public class UICurrencyButton : MonoBehaviour
{
    [SerializeField] UIInteractableButton m_button = null;
    [SerializeField] Image m_currencyIcon = null;
    [SerializeField] Text m_currencyValue = null;

    public void setCurrencyType(eCurrency currencyType)
    {
        // m_currencyIcon.sprite = GameResourceHelper.getInstance().getSprite(currencyType);
    }

    public void setCurrencyValue(LocalBigMoneyItem curValue, int needValue)
    {
        m_currencyValue.text = StringHelper.toX(needValue);
        setEnough(null == curValue ? false : curValue.count.value >= needValue);
    }

    public void setCurrencyValue(LocalBigMoneyItem curValue, BigMoney needValue)
    {
        m_currencyValue.text = needValue.toString();
        setEnough(null == curValue ? false : curValue.count.value >= needValue.value);
    }

    private void setEnough(bool isEnough)
    {
        m_button.setInteractable(isEnough);
    }
}
