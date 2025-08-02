using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;
using System;
using System.Numerics;

public class CurrencyValueBroadcaster : NonMonoSingleton<CurrencyValueBroadcaster>
{
    private class CurrencyValues
    {
        public Dictionary<eCurrencyValueId, UICurrencyBigValue> values = new Dictionary<eCurrencyValueId, UICurrencyBigValue>();
    }

    private Dictionary<eCurrency, CurrencyValues> m_currencies = new Dictionary<eCurrency, CurrencyValues>();

    public void regist(UICurrencyBigValue value)
    {
        if (Logx.isActive)
        {
            Logx.assert(null != value, "value is null");
            Logx.assert(eCurrencyValueId.None != value.id, "Invalid currency value id {0}", value.id);
        }

        CurrencyValues currencyValues;
        if (!m_currencies.TryGetValue(value.currencyType, out currencyValues))
        {
            currencyValues = new CurrencyValues();
            m_currencies.Add(value.currencyType, currencyValues);
        }

        if (currencyValues.values.ContainsKey(value.id))
        {
            if (Logx.isActive)
                Logx.warn("Already exist currency value id {0}, currencyType {1}", value.id, value.currencyType);
            return;
        }

        currencyValues.values.Add(value.id, value);
    }

    public void unregist(UICurrencyBigValue value)
    {
        if (Logx.isActive)
            Logx.assert(eCurrencyValueId.None != value.id, "Invalid currency value id {0}", value.id);

        CurrencyValues currencyValues;
        if (!m_currencies.TryGetValue(value.currencyType, out currencyValues))
        {
            if (Logx.isActive)
                Logx.warn("Not exist currency type {0}", value.currencyType);
            return;
        }

        if (!currencyValues.values.ContainsKey(value.id))
        {
            if (Logx.isActive)
                Logx.warn("Not exist currency value id {0}", value.id);
            return;
        }

        currencyValues.values.Remove(value.id);
    }

    public void startEvent(eCurrency currencyType, eCurrencyValueId valueId, LocalBigMoneyItem item, bool isPlaySfx, bool isAnimation, GameObject throwEventStartObject , bool isGetCountOne, bool isBuyShop)
    {
        CurrencyValues currencyValues;
        if (!m_currencies.TryGetValue(currencyType, out currencyValues))
        {
            if (Logx.isActive)
                Logx.warn("Not exist currency type {0}", (eCurrency)item.id);
            return;
        }

        bool isGain = false;

        foreach (var pair in currencyValues.values)
        {
            if (pair.Value.value.value <= item.count.value)
                isGain = true;

            if (pair.Key == valueId)
            {
                if (null != throwEventStartObject)
                {
                    if (pair.Value.value.value <= item.count.value)
                        runGetItemEvent(throwEventStartObject, pair.Value, item.id, item.count, isGetCountOne, isBuyShop);
                    else
                        pair.Value.setValue(item.count, isAnimation);
                }
                else
                {
                    pair.Value.setValue(item.count, isAnimation);
                }
            }
            else
            {
                pair.Value.setValue(item.count, false);
            }
        }

        if (isPlaySfx)
            playEventSfx(isGain, item.id);
    }

    private void playEventSfx(bool isGain, int itemId)
    {
        if (isGain)
        {

        }
    }

    private void runGetItemEvent(GameObject eventStartObject, UICurrencyBigValue value, int itemId, BigMoney itemCount, bool isGetOne, bool isBuyShop)
    {
        if (null == eventStartObject)
        {
            value.setValue(itemCount, true);
        }
        else if(isGetOne)
        {
        //    ItemRow itemRow = GameTableHelper.instance.getRow<ItemRow>((int)eTable.Item, itemId);
        //    runBasicThrowItemEvent(eventStartObject, value.icon.gameObject, itemRow.spriteId,
        //                           GameSettings.instance.gainItemEvent.baseCurrencyCount, (eventCount) =>
        //    {
        //        if (null != value)
        //        {
        //            value.setIconScaleEffect();
        //            // 첫 이벤트 오브젝트가 도착했을 때 숫자 올라가는 연출을 해준다.
        //            if (1 == eventCount)
        //                value.setValue(itemCount, true);
        //        }
        //    });
        //}
        //else
        //{
        //    ItemRow itemRow = GameTableHelper.instance.getRow<ItemRow>((int)eTable.Item, itemId);
        //    var GameSettingTable = GameTableHelper.instance.getTable<GameSettingTable>((int)eTable.GameSetting);

        //    int throwItemCount = GameSettings.instance.radialGainItemEvent.eventCount;
        //    runRadialThrowItemEvent(eventStartObject, value.icon.gameObject, itemRow.spriteId, throwItemCount, (eventCount) =>
        //    {
        //        if (null != value)
        //        {
        //            value.setIconScaleEffect();
        //            // 첫 이벤트 오브젝트가 도착했을 때 숫자 올라가는 연출을 해준다.
        //            if (1 == eventCount)
        //                value.setValue(itemCount, true);
        //        }
        //    });
        }
    }

    /// <param name="endCallback">eventCount</param>
    public void runRadialThrowItemEvent(GameObject eventStartObject, GameObject eventDestObject, int iconId, int throwItemCount, Action<int> endCallback)
    {
        var settings = GameSettings.instance.radialGainItemEvent;

        UIGameGetRadialThrowItemEvent itemEvent = new UIGameGetRadialThrowItemEvent();
        itemEvent.startObject = eventStartObject;
        itemEvent.destObject = eventDestObject;
        itemEvent.eventCount = throwItemCount;
        itemEvent.runDelay = settings.runDelay;
        itemEvent.completeCallback = null;
        itemEvent.oneEventEndCallback = endCallback;
        itemEvent.speed = settings.speed;
        itemEvent.radialLength = settings.radialLength;
        itemEvent.radialAxis = settings.radialAxis;
        itemEvent.radialSmoothTime = settings.radialSmoothTime;
        itemEvent.radialMoveEndWait = settings.radialMoveEndWait;
        itemEvent.iconId = iconId;
        itemEvent.run();
    }

    /// <param name="endCallback">eventCount</param>
    public void runBasicThrowItemEvent(GameObject eventStartObject, GameObject eventDestObject, int iconId, int throwItemCount,
        Action<int> endCallback, Action runDelayCallback = null)
    {
        var settings = GameSettings.instance.gainItemEvent;

        runBasicThrowItemEvent((itemEvent) =>
        {
            itemEvent.startObject = eventStartObject;
            itemEvent.destObject = eventDestObject;
            itemEvent.eventCount = throwItemCount;
            itemEvent.completeCallback = null;
            itemEvent.runDelay = UnityEngine.Vector2.zero;
            itemEvent.runDelayCallback = runDelayCallback;
            itemEvent.oneEventEndCallback = endCallback;
            itemEvent.iconId = iconId;
        });
    }

    public void runBasicThrowItemEvent(Action<UIGameGetBasicThrowItemEvent> customCallback)
    {
        var settings = GameSettings.instance.gainItemEvent;

        UIGameGetBasicThrowItemEvent itemEvent = new UIGameGetBasicThrowItemEvent();
        itemEvent.completeCallback = null;
        itemEvent.speed = settings.speed;
        itemEvent.randAngle = settings.randAngle;
        itemEvent.centerPositionRate = settings.centerPositionRate;
        customCallback?.Invoke(itemEvent);
        itemEvent.run();
    }
}