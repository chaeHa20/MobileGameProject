using System;
using System.Collections.Generic;
using UnityHelper;

public class LocalProtocolReceiveAllQuestReward : GameLocalProtocol
{
    public override void process(Req_LocalData _req, Action<Res_LocalData> callback)
    {
        var req = _req as Req_ReceiveAllQuestReward;

        var result = new LocalAddItemResult();
        var currency = new LocalBigMoneyItem();
        List<LocalQuest> quests = new List<LocalQuest>();

        foreach (var questType in req.questTypes)
        {
            var quest = getQuest(questType);
            if (null == quest || !quest.isClear() || quest.isGetReward)
            {
                callback(Res_LocalData.createError<Res_ReceiveQuestReward>((int)eLocalProtocolError.QuestWasNotCleared));
                return;
            }

            var questRow = getRow<QuestRow>(eTable.Quest, quest.questId);

            BigMoney rewardValue = new BigMoney(questRow.rewardValue);

            var itemRow = getRow<ItemRow>(eTable.Item, questRow.rewardItemId);
            if (eItem.Currency == itemRow.mainType)
            {
                var currencyType = (eCurrency)itemRow.subType;
                addCurrency(currencyType, rewardValue.value, out currency);
                result.addCurrency(currencyType, currency, rewardValue.value);
            }
            else
                addItem(questRow.rewardItemId, rewardValue.value, false, result);

            quest.setGetReward();
            quests.Add(quest);
        }

        var res = new Res_ReceiveAllQuestReward
        {
            quests = quests,
            result = result,
            currency = currency,
        };

        callback(res);
    }
}
