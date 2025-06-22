using System;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class GameLocalDataHelper : LocalDataHelper
{
    [SerializeField] GameSimulator m_simulator = null;

    public static GameLocalDataHelper getInstance()
    {
        return getInstance<GameLocalDataHelper>();
    }

    public override void initialize(Crypto crypto)
    {
        base.initialize(crypto);

        m_simulator.initialize();
    }

    protected override void initDatas()
    {
        base.initDatas();

        addData<LocalUuidData>(eLocalData.Uuid.ToString(), 0);
        addData<LocalUserData>(eLocalData.User.ToString(), 0);
        addData<LocalItemData>(eLocalData.Item.ToString(), 0);

        addData<LocalGameData>(eLocalData.Game.ToString(), 0);
        addData<LocalQuestData>(eLocalData.Quest.ToString(), 0);
        addData<LocalTutorialData>(eLocalData.Tutorial.ToString(), 0);
        addData<LocalAdData>(eLocalData.Ad.ToString(), 0);

        addData<LocalPlayerData>(eLocalData.Player.ToString(), 0);
        addData<LocalCurrencyData>(eLocalData.Currency.ToString(), 0);
    }

    protected override void initProtocols()
    {
        base.initProtocols();

        addProtocol<LocalProtocolGetCurrency>((int)eLocalProtocol.GetCurrency, false);
        addProtocol<LocalProtocolAddCurrency>((int)eLocalProtocol.AddCurrency, true);
        addProtocol<LocalProtocolGetPlayerInfo>((int)eLocalProtocol.GetPlayerInfo, false);
        addProtocol<LocalProtocolGetGameOption>((int)eLocalProtocol.GetGameOption, false);
        addProtocol<LocalProtocolGetAllItems>((int)eLocalProtocol.GetAllItems, false);
        addProtocol<LocalProtocolGetTutorial>((int)eLocalProtocol.GetTutorial, false);
        addProtocol<LocalProtocolGetAllValidTutorial>((int)eLocalProtocol.GetAllTutorial, false);
        addProtocol<LocalProtocolGetQuest>((int)eLocalProtocol.GetQuest, false);
        addProtocol<LocalProtocolGetTodayQuests>((int)eLocalProtocol.GetAllQuest, false);



        // save LocalData
        addProtocol<ProtocolLogin>((int)eLocalProtocol.Login, true);
        addProtocol<LocalProtocolUseCurrency>((int)eLocalProtocol.UseCurrency, true);;
        addProtocol<LocalProtocolSetGameOption>((int)eLocalProtocol.SetGameOption, true);
        addProtocol<LocalProtocolAddRewardItems>((int)eLocalProtocol.AddRewardItems, true);
        addProtocol<LocalProtocolCompleteTutorial>((int)eLocalProtocol.CompleteTutorial, true);
        addProtocol<LocalProtocolSetSocialId>((int)eLocalProtocol.SetSocialId, true);
        addProtocol<LocalProtocolUpdateQuest>((int)eLocalProtocol.UpdateQuest, true);
        addProtocol<LocalProtocolReceiveQuestReward>((int)eLocalProtocol.ReceiveQuestReward, true);
        addProtocol<LocalProtocolReceiveAllQuestReward>((int)eLocalProtocol.ReceiveAllQuestReward, true);
        addProtocol<LocalProtocolDailyReset>((int)eLocalProtocol.DailyReset, true);
    }

    public override void request<U>(Req_LocalData req, Action<U> callback)
    {
        base.request<U>(req, (res) =>
        {
            var r = res as Res_GameData;
            if (null != r)
            {
                if (r.isSuccess && r.isUpdateQuest)
                {
                    GameUIHelper.getInstance().sendMessage((int)eUIMessage.UpdateQuest);
                    if (r.isClearQuest)
                        GameUIHelper.getInstance().sendMessage((int)eUIMessage.ClearQuest);
                }
            }

            callback?.Invoke(res);
        });
    }

    protected override string errorToText(int error)
    {
        var errorCode = (eLocalProtocolError)error;

        return StringHelper.get("system_protocol_error_occur", error);
    }

    protected override void openErrorMsgBox(int error, object[] errorArgs, string errorText)
    {
        var errorCode = (eLocalProtocolError)error;

        if (eLocalProtocolError.NotEnoughGem == errorCode)
        {
            UIGameToastMsg.createNotEnoughCurrency(eCurrency.Gem);
        }
        else if (eLocalProtocolError.NotEnoughGold == errorCode)
        {
            UIGameToastMsg.createNotEnoughCurrency(eCurrency.Gold);
        }
    }


    public void requestGetGameOption(Action<LocalGameOption> callback)
    {
        var req = new Req_GetGameOption();
        request<Res_GetGameOption>(req, (res) =>
        {
            if (res.isSuccess)
            {
                callback?.Invoke(res.localGameOption);
            }
        });
    }

    public void requestGetCurrency(Action<Res_GetCurrency> callback)
    {
        var req = new Req_GetCurrency();
        request<Res_GetCurrency>(req, (res) =>
        {
            if (res.isSuccess)
            {
                callback(res);
            }
        });
    }

    public void requestUpdateQuest(eQuestType questType, int addValue = 1)
    {
        var req = new Req_UpdateQuest
        {
            type = questType,
            addValue = addValue,
        };

        request<Res_UpdateQuest>(req, (res) =>
        {
            if (res.isSuccess && res.isUpdateQuest)
            {
                GameUIHelper.getInstance().sendMessage((int)eUIMessage.UpdateQuest);
                if(res.isClearQuest)
                    GameUIHelper.getInstance().sendMessage((int)eUIMessage.ClearQuest);
            }
        });
    }
}
