using UnityEngine;
using UnityHelper;

public class UIGameSubCurrencyWindow : UIGameWindow
{
    [SerializeField] eCurrencyFlag m_currencyFlag = eCurrencyFlag.None;

    // TODO : 2024-02-01 update by pms
    private void OnEnable()
    {
        sendMessage((int)eUIMessage.OpenSubCurrencyUI, m_currencyFlag);
    }

    // TODO : 2024-02-01 update by pms
    public override void onClose()
    {
        sendMessage((int)eUIMessage.CloseSubCurrencyUI);
        base.onClose();
    }

    public override void resume(UIWindowData data)
    {
        base.resume(data);

        sendMessage((int)eUIMessage.OpenSubCurrencyUI, m_currencyFlag);
    }

    protected void onAskNeedGold(bool isToast = true)
    {
        askNeedCurrency(eCurrency.Gold, isToast);
    }

    protected void onAskNeedGem(bool isToast = false)
    {
        askNeedCurrency(eCurrency.Gem, isToast);
    }

    private void askNeedCurrency(eCurrency currencyType, bool isToast)
    {
        if (isToast)
            UIGameToastMsg.createNotEnoughCurrency(currencyType);
        else
            openNotEnoughCurrencyMsgBox(currencyType);
    }

    protected void openNotEnoughCurrencyMsgBox(eCurrency currencyType)
    {
        UIGameToastMsg.createNotEnoughCurrency(currencyType);
    }
}
