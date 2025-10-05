using UnityHelper;

public enum eTable
{
    None,
    
    Resource,
    Path,
    Pool,
    Sound,
    String,

    Item,
    Equipment,
    EquipmentAbilityUpgrade,
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

        // String
        load<StringTable>((int)eTable.String, "Table/StringTable", crypto);

        // Resource
        load<ResourceTable>((int)eTable.Resource, "Table/ResourceTable", crypto);
        load<PathTable>((int)eTable.Path, "Table/PathTable", crypto);
        load<PoolTable>((int)eTable.Pool, "Table/PoolTable", crypto);


        // Item
        load<ItemTable>((int)eTable.Item, "Table/ItemTable", crypto);
        load<EquipmentTable>((int)eTable.Equipment, "Table/EquipmentTable", crypto);
        load<EquipmentAbilityUpgradeTable>((int)eTable.EquipmentAbilityUpgrade, "Table/EquipmentUpgradeAbilityTable", crypto);

        // Shop



        // Sound

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

    public string getResourcePath(int resourceId, int index)
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
            path = FileHelper.combine(pathRow.path, string.Format(resourceRow.filename, index));
        }
        else
        {
            path = string.Format(resourceRow.filename, index);
        }

        return path;
    }

    public string getIntroSpritePath(int resourceId, int index)
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

        var realPath = string.Format(path, index);

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

    //public int getGameSettingValueInt(GameSettingRow.eType type)
    //{
    //    var gameSettingTable = getTable<GameSettingTable>((int)eTable.GameSetting);
    //    return gameSettingTable.getValueInt(type);
    //}

    //public float getGameSettingValueFloat(GameSettingRow.eType type)
    //{
    //    var gameSettingTable = getTable<GameSettingTable>((int)eTable.GameSetting);
    //    return gameSettingTable.getValueFloat(type);
    //}
}
