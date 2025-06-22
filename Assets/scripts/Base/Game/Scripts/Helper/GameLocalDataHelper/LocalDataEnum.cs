public enum eLocalData
{
    Uuid,
    User,
    Item,
    Currency,
    Game,
    Boost,
    Catalog,
    PackageShops,// TODO : 2024-07-04 by pms
    Ad,
    MapContainer,
    Collection,
    Player,
    RestaurantUpgrade,
    SafeAbility,
    GameSkill,
    Tutorial,
    Quest,
}

public enum eLocalProtocol
{
    Login,
    GetGameOption,
    SetGameOption,
    GetCurrency,
    UseCurrency,
    BuyCurrency,
    AddRewardItems,
    AddCurrency,
    GetAdData,
    GetAllItems,
    GetPlayerInfo,
    GetQuest,
    UpdateQuest,
    GetAllQuest,
    DailyReset,
    GetTutorial,
    CompleteTutorial,
    GetAllTutorial,
    ReceiveQuestReward,
    ReceiveAllQuestReward,
    GetNoAds,
    SetNoAds,
    SetSocialId,
}

public enum eLocalProtocolError
{
    None,
    FailedFindRow,
    NotEnoughItem,
    NotEnoughGold,
    NotEnoughGem,
    NotEnoughBuyCount,
    NotEnoughRecoveryDailyFreeCount,
    IsCoolTime,
    AlreadyGetReward,
    QuestWasNotCleared,
    NotExistItem,
    isFullShowDailyTicketChargeAd,
    NoMoreRewardToReceived,
}