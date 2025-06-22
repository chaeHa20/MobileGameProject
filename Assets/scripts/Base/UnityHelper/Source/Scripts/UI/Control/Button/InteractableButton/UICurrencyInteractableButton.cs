using UnityEngine;
using UnityEngine.UI;

namespace UnityHelper
{
    public class UICurrencyInteractableButton : UIInteractableButton
    {
        private bool m_isAvaliable = false;

        public void setCurrencyAvaliable(bool isAvaliable, bool isOnlyGraphicModify = false)
        {
            base.setInteractable(isAvaliable, isOnlyGraphicModify);

            m_isAvaliable = isAvaliable;
        }

        public void setButtonActiveOnlyCurrencyIsGold(/*LocalBigMoneyItem gold*/)
        {
            setInteractable(true);
        }

        public bool isLackCurrency()
        {
            return !m_isAvaliable;
        }

        public void setEmptyNeedGold()
        {
            setText(0, 0.ToString());
        }

        public void setNeedCurrency(string currencyValue)
        {
            setText(0, currencyValue);
        }

        public void checkEnoughCurrency(BigMoney curCurrency, BigMoney needCurrency)
        {
            setCurrencyAvaliable(curCurrency.value >= needCurrency.value);

            if (curCurrency.value == needCurrency.value && needCurrency.value >= 0)
                setOriginalTextColor();
        }

        public void setUpgradeLevel(string levelValue)
        {
            setText(1, levelValue);
        }
    }
}