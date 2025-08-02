using System;
using UnityEngine;
using UnityHelper;

public class SetSceneStateMsg : Msg
{
    private int m_state;

    public SetSceneStateMsg(int state)
    {
        m_state = state;
    }

    public override void action()
    {
        var scene = WorldScene.instance<GameScene>();
        if (null == scene)
            return;
        if (scene.isCurStateMachineCase(m_state))
            return;

        scene.setStateMachineCase(m_state);
    }
}

public class SetAddItemResultCurrencyMsg : Msg
{
    private LocalAddItemResult m_addItemResult = null;
    private GameObject m_throwEventStartObject = null;
    private eCurrencyValueId m_currencyValueId = eCurrencyValueId.None;

    public SetAddItemResultCurrencyMsg(eCurrencyValueId currencyValueId, LocalAddItemResult addItemResult, GameObject throwEventStartObject)
    {
        m_addItemResult = addItemResult;
        m_throwEventStartObject = throwEventStartObject;
        m_currencyValueId = currencyValueId;
    }

    public override void action()
    {
        SystemHelper.forEachEnum<eCurrency>(currencyType =>
        {
            if (eCurrency.None == currencyType)
                return;

            setCurrencyItemMsg(currencyType);
        });
    }

    private void setCurrencyItemMsg(eCurrency currencyType)
    {
        if (m_addItemResult.isCurrency(currencyType))
        {
            GameMsgHelper.instance.add(new SetCurrencyItemMsg(m_currencyValueId, m_addItemResult.getCurrencyItem(currencyType), true, true, true, m_throwEventStartObject));
        }
    }
}

public class SetCurrencyItemMsg : Msg
{
    private eCurrencyValueId m_valueId;
    private LocalBigMoneyItem m_item;
    private bool m_isPlaySfx = false;
    private bool m_isAnimation = true;
    private bool m_isUpdateUICurrency = true;
    private bool m_isGetOneCurrency = false;
    private bool m_isBuyShop = false;
    private GameObject m_throwEventStartObject = null;

    public SetCurrencyItemMsg(eCurrencyValueId valueId,
                              LocalBigMoneyItem item,
                              bool isPlaySfx = true,
                              bool isAnimation = true,
                              bool isUpdateUICurrency = true,
                              GameObject throwEventStartObject = null,
                              bool isGetOneCurrency = false,
                              bool isBuyShop = false)
    {
        m_valueId = valueId;
        m_item = item;
        m_isPlaySfx = isPlaySfx;
        m_isAnimation = isAnimation;
        m_isUpdateUICurrency = isUpdateUICurrency;
        m_throwEventStartObject = throwEventStartObject;
        m_isGetOneCurrency = isGetOneCurrency;
        m_isBuyShop = isBuyShop;
    }

    public SetCurrencyItemMsg(eCurrencyValueId valueId,
                              LocalBigMoneyItem item,
                              GameObject throwEventStartObject)
    {
        m_valueId = valueId;
        m_item = item;
        m_isPlaySfx = true;
        m_isAnimation = true;
        m_isUpdateUICurrency = true;
        m_throwEventStartObject = throwEventStartObject;
        m_isGetOneCurrency = false;
        m_isBuyShop = false;
    }

    public override void action()
    {
        var currencyType = (eCurrency)m_item.type.sub;
        CurrencyValueBroadcaster.instance.startEvent(currencyType, m_valueId, m_item, m_isPlaySfx, m_isAnimation, m_throwEventStartObject, m_isGetOneCurrency, m_isBuyShop);

        if (m_isUpdateUICurrency)
            GameUIHelper.instance.sendMessage((int)eUIMessage.UpdateCurrency, currencyType, m_item);
    }
}

public class SetActiveSceneBlockMsg : Msg
{
    private bool m_isBlock;

    public SetActiveSceneBlockMsg(bool isBlock)
    {
        m_isBlock = isBlock;
    }

    public override void action()
    {
        UIScene.instance().setActiveBlock(m_isBlock);
    }

    public override string toString()
    {
        return string.Format("{0} isBlock {1}", ToString(), m_isBlock);
    }
}