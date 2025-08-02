using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityHelper;

[Serializable]
public class LocalCurrencyData : LocalData
{
    [SerializeField] List<LocalBigMoneyItem> m_currencies = new List<LocalBigMoneyItem>();

    public override void initialize(string _name, int _id)
    {
        base.initialize(_name, _id);

        createCurrencies();
        setDefaultCurrencies();
    }

    private void createCurrencies()
    {
        //var itemTable = GameTableHelper.instance.getTable<ItemTable>((int)eTable.Item);

        //SystemHelper.forEachEnum<eCurrency>((e) =>
        //{
        //    if (eCurrency.None == e)
        //        return;

        //    var currencyRow = itemTable.findRow(eItem.Currency, (int)e);
        //    var currency = LocalBigMoneyItem.create(currencyRow.id, currencyRow.getType(), 0);
        //    m_currencies.Add(currency);
        //});
    }

    public LocalBigMoneyItem addCurrency(eCurrency currencyType, BigInteger value)
    {
        var currency = getCurrency(currencyType);
        if (null == currency)
            return null;

        currency.count += value;
        return currency;
    }

    public LocalBigMoneyItem getCurrency(eCurrency currencyType)
    {
        var currency = (from c in m_currencies
                        where c.type.sub == (int)currencyType
                        select c).FirstOrDefault();

        return currency;
    }

    private void setDefaultCurrencies()
    {
        //var gameSettingTable = GameTableHelper.instance.getTable<GameSettingTable>((int)eTable.GameSetting);

        //SystemHelper.forEachEnum<eCurrency>((e) =>
        //{
        //    if (eCurrency.None == e)
        //        return;

        //    addCurrency(e, 0);
        //});
    }

    public bool tryUseCurrency(eCurrency currencyType, BigInteger value, out LocalBigMoneyItem currency)
    {
        currency = getCurrency(currencyType);
        if (null == currency)
            return false;

        if (currency.count.value < value)
            return false;

        currency.count -= value;
        return true;
    }

    public override void deserialize()
    {
        foreach (var currency in m_currencies)
        {
            currency.deserialize();
        }
    }

    public override void serialize()
    {
        foreach (var currency in m_currencies)
        {
            currency.serialize();
        }
    }

    public void resetGold()
    {
        var gold = getCurrency(eCurrency.Gold);
        gold.count.value = 0;
    }
}