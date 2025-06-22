using UnityHelper;

public class UICharacterSettingWindowTab : UITab
{
    public enum eTab
    {
        Item = 1,
        Collection = 2,
        Worker = 3,
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
