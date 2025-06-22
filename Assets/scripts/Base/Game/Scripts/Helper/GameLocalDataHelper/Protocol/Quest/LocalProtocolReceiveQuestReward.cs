using System;
using UnityHelper;
// using Google.Protobuf.WellKnownTypes;
// using UnityEditor.SceneManagement;

public class LocalProtocolReceiveQuestReward : GameLocalProtocol
{
    public override void process(Req_LocalData _req, Action<Res_LocalData> callback)
    {
        var req = _req as Req_ReceiveQuestReward;

        var quest = getQuest(req.questType);
        if (null == quest || !quest.isClear() || quest.isGetReward)
        {
            callback(Res_LocalData.createError<Res_ReceiveQuestReward>((int)eLocalProtocolError.QuestWasNotCleared));
            return;
        }

        var result = new LocalAddItemResult();
        var currency = new LocalBigMoneyItem();
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

        var res = new Res_ReceiveQuestReward
        {
            quest = quest,
            result = result,
            currency = currency,
        };

        callback(res);
    }
}
