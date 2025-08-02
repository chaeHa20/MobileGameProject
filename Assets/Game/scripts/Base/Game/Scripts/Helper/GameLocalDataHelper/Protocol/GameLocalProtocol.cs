using System.Numerics;
using UnityEngine;
using UnityHelper;
public class GameLocalProtocol : LocalDataProtocol
{
    protected DATA getData<DATA>(eLocalData data) where DATA : LocalData
    {
        return GameLocalDataHelper.instance.getData<DATA>(data.ToString());
    }

    protected SUB_DATA getSubData<CONTAINER_DATA, SUB_DATA>(eLocalData data, int subId) where CONTAINER_DATA : LocalContainerData where SUB_DATA : LocalData
    {
        var containerData = getData<CONTAINER_DATA>(data);
        return containerData.getSubData<SUB_DATA>(subId);
    }

    protected B getTable<B>(eTable key) where B : class, ITable
    {
        return GameTableHelper.instance.getTable<B>((int)key);
    }

    protected R getRow<R>(eTable key, int row) where R : TableRow
    {
        return GameTableHelper.instance.getRow<R>((int)key, row);
    }

    protected LocalGameData getGameData()
    {
        return getData<LocalGameData>(eLocalData.Game);
    }

    protected bool tryGetValidUpgradeLevel(int curLevel, int maxLevel, ref int upgradeLevel)
    {
        if (curLevel >= maxLevel)
            return false;

        upgradeLevel = Mathf.Min(maxLevel, upgradeLevel);
        return true;
    }

    protected int getNotEnoughCurrencyError(eCurrency currencyType)
    {
        switch (currencyType)
        {
            case eCurrency.Gold: return (int)eLocalProtocolError.NotEnoughGold;
            case eCurrency.Gem: return (int)eLocalProtocolError.NotEnoughGem;
            default: return (int)eLocalProtocolError.None;
        }
    }

    protected bool tryUseCurrency(eCurrency currencyType, BigInteger value, out LocalBigMoneyItem currency)
    {
        var currencyData = getData<LocalCurrencyData>(eLocalData.Currency);
        return currencyData.tryUseCurrency(currencyType, value, out currency);
    }

    protected void addCurrency(eCurrency currencyType, BigInteger value, out LocalBigMoneyItem currency)
    {
        var currencyData = getData<LocalCurrencyData>(eLocalData.Currency);
        currency = currencyData.addCurrency(currencyType, value);
    }

    protected LocalBigMoneyItem getCurrency(eCurrency currencyType)
    {
        var currencyData = getData<LocalCurrencyData>(eLocalData.Currency);
        return currencyData.getCurrency(currencyType);
    }

    protected void addItem(int itemId, BigInteger value, bool isDouble, LocalAddItemResult result)
    {
        if (isDouble)
            value *= 2;

        //var itemRow = getRow<ItemRow>(eTable.Item, itemId);

        //if (eItem.Currency == itemRow.mainType)
        //{
        //    var currencyType = (eCurrency)itemRow.subType;
        //    addCurrency(currencyType, value, out LocalBigMoneyItem currency);
        //    result.addCurrency(currencyType, currency, value);

        //    GameLocalDataHelper.instance.saveUsedDatas(eLocalData.Currency);
        //}
        //else if (eItem.Equipment == itemRow.mainType)
        //{
        //    var itemData = getData<LocalItemData>(eLocalData.Item);


        //    if (eEquipment.Product == (eEquipment)itemRow.subType)
        //    {
        //        eGrade grade = eGrade.None;

        //        var addResult = itemData.addItem(itemRow, (long)value, grade);
        //        addResult.forEach(r =>
        //        {
        //            result.addItem(r.item, (int)r.addCount, r.isNew);
        //        });
        //    }
        //    else
        //    {
        //        eGrade grade = eGrade.None;

        //        var addResult = itemData.addItem(itemRow, (long)value, grade);
        //        addResult.forEach(r =>
        //        {
        //            result.addItem(r.item, (int)r.addCount, r.isNew);
        //        });
        //    }
        //}
        //else
        //{
        //    var itemData = getData<LocalItemData>(eLocalData.Item);
        //    var addResult = itemData.addItem(itemRow, (long)value);
        //    addResult.forEach(r =>
        //    {
        //        result.addItem(r.item, (int)r.addCount, r.isNew);
        //    });
        //}
    }

    protected void addItem(int itemId, string addValue, bool isDouble, LocalAddItemResult result)
    {
        var _addValue = new BigMoney(addValue);
        addItem(itemId, _addValue.value, isDouble, result);
    }

    public void checkQuest(eQuestType questType,int addValue, ref bool isUpdate)
    {
        var quest = getQuest(questType);
        if (null == quest)// TODO : 당일 퀘스트 목록에 있는지 확인
            isUpdate = false;
        else
            quest.check(questType, addValue, ref isUpdate);
    }

    public void isClearQuest(eQuestType questType, ref bool isClear)
    {
        var quest = getQuest(questType);
        if (null == quest)// TODO : 당일 퀘스트 목록에 있는지 확인
            isClear = false;
        else
            isClear = quest.isClear();
    }

    protected LocalQuest getQuest(eQuestType questType)
    {
        return getData<LocalQuestData>(eLocalData.Quest).fineQuest(questType);
    }
}
