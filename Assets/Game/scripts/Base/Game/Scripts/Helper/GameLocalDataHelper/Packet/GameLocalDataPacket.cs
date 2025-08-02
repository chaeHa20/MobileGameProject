using System.Collections.Generic;
using System.Numerics;
using UnityHelper;

public class Res_GameData : Res_LocalData
{
    public bool isUpdateQuest;
    public bool isClearQuest;
}

public class Req_Login : Req_LocalData
{
    public Req_Login()
    {
        m_pid = (int)eLocalProtocol.Login;
        m_dataType = eLocalData.User;
    }
}

public class Res_Login : Res_GameData
{
    public bool isFirstAppStart;
    public bool isDailyLogin;
}

public class Req_GetGameOption : Req_LocalData
{
    public Req_GetGameOption()
    {
        m_pid = (int)eLocalProtocol.GetGameOption;
        m_dataType = eLocalData.Game;
    }
}

public class Res_GetGameOption : Res_GameData
{
    public LocalGameOption localGameOption;
}

public class Req_SetGameOption : Req_LocalData
{
    public LocalGameOption localGameOption;

    public Req_SetGameOption()
    {
        m_pid = (int)eLocalProtocol.SetGameOption;
        m_dataType = eLocalData.Game;
    }
}

public class Res_SetGameOption : Res_GameData
{
    public LocalGameOption localGameOption;
}

public class Req_GetCurrency : Req_LocalData
{
    public Req_GetCurrency()
    {
        m_pid = (int)eLocalProtocol.GetCurrency;
        m_dataType = eLocalData.Currency;
    }
}

public class Res_GetCurrency : Res_GameData
{
    public LocalBigMoneyItem gold;
    public LocalBigMoneyItem gem;
}

public class Req_UseCurrency : Req_LocalData
{
    public eCurrency currencyType;
    public BigInteger useValue;

    public Req_UseCurrency()
    {
        m_pid = (int)eLocalProtocol.UseCurrency;
        m_dataType = eLocalData.Currency;
    }
}

public class Res_UseCurrency : Res_GameData
{
    public LocalBigMoneyItem currency;
}

public class Req_BuyCurrency : Req_LocalData
{
    public int shopId;
    public eTable shopType;
    public eBuyType buyType;
    public eCurrency currencyType;
    public BigInteger useValue;

    public Req_BuyCurrency()
    {
        m_pid = (int)eLocalProtocol.BuyCurrency;
        m_dataType = eLocalData.Currency;
    }
}

public class Res_BuyCurrency : Res_GameData
{
    public LocalBigMoneyItem useCurrency;
    public LocalAddItemResult rewardCurrency;
}

public class Req_AddRewardItems : Req_LocalData
{
    public List<int> rewardItems;
    public bool isGetUsingRewardChest;
    public bool isShowAd;

    public Req_AddRewardItems()
    {
        m_pid = (int)eLocalProtocol.AddRewardItems;
        m_dataType = eLocalData.Item;
    }
}

public class Res_AddRewardItems : Res_GameData
{
    public LocalAddItemResult addItemResult;
}

public class Req_AddCurrency : Req_LocalData
{
    public eCurrency currencyType;
    public BigInteger addValue;

    public Req_AddCurrency()
    {
        m_pid = (int)eLocalProtocol.AddCurrency;
        m_dataType = eLocalData.Currency;
    }
}

public class Res_AddCurrency : Res_GameData
{
    public LocalBigMoneyItem currency;
}

public class Req_GetLocalAd : Req_LocalData
{
    public Req_GetLocalAd()
    {
        m_pid = (int)eLocalProtocol.GetAdData;
        m_dataType = eLocalData.Ad;
    }
}


public class Res_GetLocalAd : Res_GameData
{
    public LocalAdData adData;
}


public class Req_GetAllItems : Req_LocalData
{
    public Req_GetAllItems()
    {
        m_pid = (int)eLocalProtocol.GetAllItems;
        m_dataType = eLocalData.Item;
    }
}

public class Res_GetAllIems : Res_GameData
{
    public LocalBigMoneyItem gold;
    public LocalBigMoneyItem gem;
    public LocalItems<LocalItem> items;
}

public class Req_GetPlayerInfo : Req_LocalData
{
    public Req_GetPlayerInfo()
    {
        m_pid = (int)eLocalProtocol.GetPlayerInfo;
        m_dataType = eLocalData.Player;
    }
}

public class Res_GetPlayerInfo : Res_GameData
{
    public int playerThumbnailId;
    public LocalPlayerData playerData;
}

public class Req_GetQuest : Req_LocalData
{
    public eQuestType type;
    public Req_GetQuest()
    {
        m_pid = (int)eLocalProtocol.GetQuest;
        m_dataType = eLocalData.Quest;
    }
}

public class Res_GetQuest : Res_LocalData
{
    public LocalQuest quest;
}

public class Req_UpdateQuest : Req_LocalData
{
    public eQuestType type;
    public int addValue;

    public Req_UpdateQuest()
    {
        m_pid = (int)eLocalProtocol.UpdateQuest;
        m_dataType = eLocalData.Quest;
    }
}

public class Res_UpdateQuest : Res_GameData
{
}


public class Req_GetAllQuests : Req_LocalData
{
    public Req_GetAllQuests()
    {
        m_pid = (int)eLocalProtocol.GetAllQuest;
        m_dataType = eLocalData.Quest;
    }
}

public class Res_GetAllQuests : Res_LocalData
{
    public List<LocalQuest> quests;
    public bool isOpenQuest;
}

public class Req_DailyReset : Req_LocalData
{
    public Req_DailyReset()
    {
        m_pid = (int)eLocalProtocol.DailyReset;
        m_dataType = eLocalData.User;
    }
}

public class Res_DailyReset : Res_LocalData
{
}

public class Req_GetTutorial : Req_LocalData
{
    public eTutorialId tutorialType;

    public Req_GetTutorial()
    {
        m_pid = (int)eLocalProtocol.GetTutorial;
        m_dataType = eLocalData.Tutorial;
    }
}

public class Res_GetTutorial : Res_LocalData
{
    public LocalTutorial tutorial;
}

public class Req_CompleteTutorial : Req_LocalData
{
    public eTutorialId tutorialType;

    public Req_CompleteTutorial()
    {
        m_pid = (int)eLocalProtocol.CompleteTutorial;
        m_dataType = eLocalData.Tutorial;
    }
}

public class Res_CompleteTutorial : Res_LocalData
{
}

public class Req_GetAllTutorials : Req_LocalData
{
    public Req_GetAllTutorials()
    {
        m_pid = (int)eLocalProtocol.GetAllTutorial;
        m_dataType = eLocalData.Tutorial;
    }
}

public class Res_GetAllTutorials : Res_LocalData
{
    public List<LocalTutorial> tutorials;
}

public class Req_ReceiveQuestReward : Req_LocalData
{
    public eQuestType questType;

    public Req_ReceiveQuestReward()
    {
        m_pid = (int)eLocalProtocol.ReceiveQuestReward;
        m_dataType = eLocalData.Quest;
    }
}

public class Res_ReceiveQuestReward : Res_GameData
{
    public LocalQuest quest;
    public LocalAddItemResult result;
    public LocalBigMoneyItem currency;
}

public class Req_ReceiveAllQuestReward : Req_LocalData
{
    public List<eQuestType> questTypes;

    public Req_ReceiveAllQuestReward()
    {
        m_pid = (int)eLocalProtocol.ReceiveAllQuestReward;
        m_dataType = eLocalData.Quest;
    }
}


public class Res_ReceiveAllQuestReward : Res_LocalData
{
    public List<LocalQuest> quests;
    public LocalAddItemResult result;
    public LocalBigMoneyItem currency;
}


public class Req_GetNoAds : Req_LocalData
{
    public Req_GetNoAds()
    {
        m_pid = (int)eLocalProtocol.GetNoAds;
        m_dataType = eLocalData.Ad;
    }
}

public class Res_GetNoAds : Res_LocalData
{
    public bool isNoAds;
}

public class Req_SetNoAds : Req_LocalData
{
    public Req_SetNoAds()
    {
        m_pid = (int)eLocalProtocol.SetNoAds;
        m_dataType = eLocalData.Ad;
    }
}

public class Res_SetNoAds : Res_LocalData
{
    
}

public class Req_SetSocialId : Req_LocalData
{
    public string id;

    public Req_SetSocialId()
    {
        m_pid = (int)eLocalProtocol.SetSocialId;
        m_dataType = eLocalData.Player;
    }
}

public class Res_SetSocialId : Res_LocalData
{
}