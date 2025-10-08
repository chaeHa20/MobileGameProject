using UnityHelper;

public class UIGameCollectionWindowTab : UITab
{
    public enum eTab
    {
        None = 0,
        BGTemplete = 1,
        Rigding = 2,
        Weapon = 3,
        Armor = 4,
        PlayerCharacter = 5,
    }

    public class Data : BaseData
    {
        public eTab tab;
    }

    public override void initialize(BaseData baseData)
    {
        base.initialize(baseData);

        var data = baseData as Data;

        setTab((int)data.tab);
    }
}
