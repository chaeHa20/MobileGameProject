using System;

public enum eTag
{

}

public enum eScene
{
    LogoScene,
    StartScene,
    Lobby,
    Main,
}

public enum eUIMainLayer
{
    Canvas = 0,
    Bottom = 1,
    Main = 2,
    Currency = 3,
    OverCurrency = 4,
}

public enum eLayer
{
    Default = 0,
    UI = 5,
    Prop = 6,
    Allience = 10,
    Enemy = 11,
    UIObject = 12,
}

public enum eItem
{
    None = 0,
    Currency = 1,
    Equipment = 2,
    NoAds = 7,
}

public enum eEquip
{
    None = 0,
    Weapon = 1,
    Armor = 2,
}

public enum eEquipmentAbility
{
    None = 0,
    NormalRange = 1,
    WideRange = 2,
    ExtraWideRange = 3,
    ShotRange = 4,
    ExtraShotRange = 5,
    NormalAtkPower = 6,
    HighAtkPower = 7,
    LowAtkPower = 8,
    NormalAtkSpeed = 9,
    FastAtkSpeed = 10,
    ExtraFastAtkSpeed = 11,
    SlowAtkSpeed = 12,
    NormalStun = 13,
    StrongStun = 14,
    WeakStun = 15,
    ExtraWeakStun = 16,
    NormalPiercing = 17,
    StrongPiercing = 18,
    ExtraStrongPiercing = 19,
    WeakPiercing = 20,
}


public enum eNoAds
{
    None = 0,
    NoAds = 1,
}

public enum eCurrency
{
    None,
    Gold = 1,
    Gem = 2,
}


[Flags]
public enum eCurrencyFlag
{
    None = 0,
    Gold = 1 << 0,
    Gem = 1 << 1,
}

public enum eCurrencyValueId
{
    None,
    Main,
    InSide,
    InShop,
}
public enum eOption
{
    None = 0,
    Language = 1,
    GraphicQuiality = 2,
    SaveCloudData = 3,
}

public enum eCharacterAbility
{
    None = 0,
}

public enum eEquipmentAbility
{
    None = 0,
}


public enum eResource
{
    None,
    UIGameToastMsg = 1000,
    introToon = 1001,
    UISynthesisReturnCurrencyToastMsg = 1002,
    UIPlayerHealthItem = 1003,
    UIGameIntroToonWindow = 1005,
}

public enum eUIMessage
{
    None,

    // data : Gem
    RefreshGem,
    // TODO : 2024-01-31 update by pms data :
    CloseSubCurrencyUI,
    // TODO : 2024-01-31 update by pms data : eCurrencyFlag
    OpenSubCurrencyUI,
    // data
    OpenQuest,
    // data
    UpdateQuest,
    // data
    ClearQuest,
    // data : 
    RecieveQuestReward,
    // data : count
    SetRecoveryDailyFreeCount,
    // data : eCurrency, LocalBigMoneyItem
    UpdateCurrency,
    // data : 
    UpdateLanguage,
    // data : eTutorial
    UpdateTutorial,
}

public enum eObjectMessage
{

}

public enum eSound
{
    None = 0,
    Bgm = 1,
    Sfx = 2,
}

public enum eBgm
{
    None,
    Main = 101,
}

public enum eSfx
{
    None,
}

public enum eGraphicQuality
{
    Low,
    Medium,
    High,
}

public enum eVibration
{
    None = 0,
}

public enum eAttackType
{
    None = 0,
    Melee = 1,
    Ranged = 2,
}

public enum eFireMachineType
{
    None = 0,
    Bow = 1,
    CrossBow = 2,
    Pistol = 3,
    Rifle = 4,
}

public enum eBulletType
{
    None = 0,
    Arrow = 1,
    Bullet = 2,
}



public enum eTutorialOpen
{
    None = 0,
}

public enum ePackageItem
{
    None = 0,
    Currency = 1,
    Equipment = 2,
}

public enum eShopPackage
{
    None = 0,
    Shop = 1
}


public enum eAbility
{
    None = 0,
    MaxHealth = 1,
    MoveSpeed = 2,
    AttackSpeed = 3,
    AttackPower = 4,
    AttackCount = 5,
    AttackPerTime = 6,
    CriticalProbability = 7,
    CriticalDamagePercent = 8,
}

public enum eAbilityOwner
{
    None = 0,
    Player = 1,
    Character = 2,
}

public enum eEquipment
{
    None = 0,
    Product = 1,
    BluePrint = 2,
}


public enum eGrade
{
    None = 0,
    Normal = 1,
    Rare = 2,
    Epic = 3,
    Legendary = 4,
    Ultimate = 5,
    Mythic = 6,

    Unique = 10,
}

public enum eGoal
{
    None,
    Idle,
    PathMove,
    DirectMove,
    Waiting,
}

public enum eParts
{
    Head = 1,
    Eye = 2,
    Neck = 3,
    Back = 4,
    LHand = 5,
    RHand = 6,

    None = 100,
    All = 101,
    Pet = 102,
}

public enum eProp
{
    None = 0,
}


public enum eTeam
{
    None,
    _1 = 1,     // 아군
    _2 = 2,     // 적군
    _3 = 3,     // NPC
}

public enum eHudPosition
{
    None = 0,
    Left = 1,
    Middle = 2,
    Right = 3,
}

[Flags]
public enum eEntity
{
    None = 0,
    Player = 1 << 0,
    Forces = 1 << 1,
    Monster = 1 << 4,
    SubPlayer = 1 << 5,
    Prop = 1 << 7,
}

public enum eMotion
{
    None = 0,
    Idle = 1,
    Move = 2,
}

public enum eNotification
{
    AdShop,
    Quest,
}

public enum eTutorialCompleteType
{
    None,
    OpenMsgBox,
    ShowToast,
}

public enum eTutorialId
{
    None = 0,
}

public enum eQuest
{
    None,
    Daily = 1,
    Weekly = 2,
}

public enum eQuestType
{
    None,
}


public enum eBuyType
{
    None = 0,
    InApp = 1,
    Gem = 2,
    PreOrder = 3,
}


public enum eShop
{
    None = 0,
    Currency = 1,
    Package = 2,
}

public enum eNPC
{
    None = 0,
}

public enum eMonster
{
    None = 0,
}