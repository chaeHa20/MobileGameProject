using UnityHelper;

public enum eTable
{
    None,
    
    ShopCondition,
    Package,
    Boost,
    Help,
    Tutorial,
    PushNotification,

    Resource,
    Path,
    Pool,
    Sound,
    String,

    ChestShop,
    PackageShop,
    CurrencyShop,
    InAppProduct,
    GoldAndBoosterShop,
    GemShop,

    LoginGift,
    PlayTimeGift,// TODO : 2024-09-02 by pms

    BoosterShopProbability,// TODO : 2024-06-28 by pms
    ShopAdCoolTime, // TODO : 2024-07-23 by pms
    //
    // Character
    CharacterSetting,
    CollectionAbility,
    Collection,
    CustomerAbility,
    Customer,
    NPCAbility,
    NPC,
    WorkerAbility,
    Worker,

    // Food
    FoodSetting,
    FoodPackage,
    Food,
    FoodAbilityGroup,
    FoodMakeCount,
    FoodUpgradeReward,
    FoodAbility_1st,
    FoodAbility_2nd,
    FoodAbility_3rd,
    FoodAbility_4th,
    FoodAbility_5th,
    FoodAbility_6th,
    FoodAbility_7th,
    FoodAbility_8th,
    FoodAbility_9th,
    FoodAbility_10th,

    //  Game
    GameSetting,
    Map,
    Stage,
    RestaurantAbility,
    RestaurantUpgrade,
    Chest,
    ChestAbility,
    Quest,

    // Item
    Item,
    EquipmentItem,
    EquipmentBluePrint,
    EquipmentItemAbility,
    Deco,
    DecoAbility,
}

public class GameTableHelper : TableHelper<GameTableHelper>
{
    public override void initialize(Crypto crypto)
    {
        base.initialize(crypto);
    }

    protected override void load(Crypto crypto)
    {
        base.load(crypto);
        
        // Game
        load<GameSettingTable>((int)eTable.GameSetting, "Table/GameSettingTable", crypto);
        load<TutorialTable>((int)eTable.Tutorial, "Table/TutorialTable", crypto);
        load<QuestTable>((int)eTable.Quest, "Table/QuestTable", crypto);
        load<PushNotificationTable>((int)eTable.PushNotification, "Table/PushNotificationTable", crypto);

        // String
        load<StringTable>((int)eTable.String, "Table/StringTable", crypto);

        // Resource
        load<ResourceTable>((int)eTable.Resource, "Table/ResourceTable", crypto);
        load<PathTable>((int)eTable.Path, "Table/PathTable", crypto);
        load<PoolTable>((int)eTable.Pool, "Table/PoolTable", crypto);


        // Item
        load<ItemTable>((int)eTable.Item, "Table/ItemTable", crypto);
        
        // Shop
        load<InAppProductTable>((int)eTable.InAppProduct, "Table/InAppTable", crypto);
        

        // Sound
        load<SoundTable>((int)eTable.Sound, "Table/SoundTable", crypto);
    }

    public string getString(int id)
    {
        var stringRow = getRow<StringRow>((int)eTable.String, id);
        if (null == stringRow)
        {
            if (Logx.isActive)
                Logx.error("Failed getString, Invalid string id {0}", id);

            return "";
        }

        return stringRow.text;
    }

    public string getString(int id, params object[] args)
    {
        string str = getString(id);
        return string.Format(str, args);
    }

    public string getString(string code)
    {
        var stringTable = getTable<StringTable>((int)eTable.String);
        return stringTable.getString(code);
    }

    public string getString(string code, params object[] args)
    {
        string str = getString(code);
        return string.Format(str, args);
    }

    public string getResourceName(int resourceId)
    {
        var resourceRow = getRow<ResourceRow>((int)eTable.Resource, resourceId);
        return resourceRow.filename;
    }

    public string getResourcePath(int resourceId)
    {
        if (Logx.isActive)
            Logx.assert(0 < resourceId, "Invalid resource id {0}", resourceId);

        string path = "";
        ResourceRow resourceRow = getRow<ResourceRow>((int)eTable.Resource, resourceId);
        if (null == resourceRow)
            return null;

        if (0 < resourceRow.pathId)
        {
            PathRow pathRow = getRow<PathRow>((int)eTable.Path, resourceRow.pathId);
            path = FileHelper.combine(pathRow.path, resourceRow.filename);
        }
        else
        {
            path = resourceRow.filename;
        }

        return path;
    }

    public string getStagePrefabPath(int resourceId, string id)
    {
        if (Logx.isActive)
            Logx.assert(0 < resourceId, "Invalid resource id {0}", resourceId);

        string path = "";
        ResourceRow resourceRow = getRow<ResourceRow>((int)eTable.Resource, resourceId);
        if (null == resourceRow)
            return null;

        if (0 < resourceRow.pathId)
        {
            PathRow pathRow = getRow<PathRow>((int)eTable.Path, resourceRow.pathId);
            path = FileHelper.combine(pathRow.path, resourceRow.filename);
        }
        else
        {
            path = resourceRow.filename;
        }

        var realPath = string.Format(path, id);

        return realPath;
    }

    public string getEntityModelPath(eResource resourceId, int modelId)
    {
        string resPath = getResourcePath((int)resourceId);
        return string.Format(resPath, modelId);
    }

    public bool isPoolResource(eResource resourceId)
    {
        var resourceRow = getRow<ResourceRow>((int)eTable.Resource, (int)resourceId);
        return 0 < resourceRow.poolId;
    }

    public ItemRow getGoldItemRow()
    {
        return getRow<ItemRow>((int)eTable.Item, Define.GOLD_ITEM_ID);
    }

    public ItemRow getItemRow(eItem mainType, int subType)
    {
        var itemTable = getTable<ItemTable>((int)eTable.Item);
        return itemTable.findRow(mainType, subType);
    }

    public ItemRow findCurrencyRow(eCurrency currencyType)
    {
        var itemTable = getTable<ItemTable>((int)eTable.Item);
        return itemTable.findRow(eItem.Currency, (int)currencyType);
    }

    public int getGameSettingValueInt(GameSettingRow.eType type)
    {
        var gameSettingTable = getTable<GameSettingTable>((int)eTable.GameSetting);
        return gameSettingTable.getValueInt(type);
    }

    public float getGameSettingValueFloat(GameSettingRow.eType type)
    {
        var gameSettingTable = getTable<GameSettingTable>((int)eTable.GameSetting);
        return gameSettingTable.getValueFloat(type);
    }
}
