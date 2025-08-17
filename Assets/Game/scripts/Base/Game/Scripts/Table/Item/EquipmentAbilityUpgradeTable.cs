using System.Collections.Generic;
using UnityHelper;

public class EquipmentAbilityUpgradeRow : TableRow
{
    private eEquipmentAbility m_abilityType;
    private int m_groupId;
    private int m_level;
    private int m_nextId;
    private int m_needExp;
    private float m_abilityWeight;

    public eEquipmentAbility abilityType => m_abilityType;
    public int groupId => m_groupId;
    public int level => m_level;
    public int nextId => m_nextId;
    public int needExp => m_needExp;
    public float abilityWeight => m_abilityWeight;

    public override void parse(List<string> cells, List<string> dataTypes, ref int i)
    {
        base.parse(cells, dataTypes, ref i);

        m_abilityType = (eEquipmentAbility)toInt(cells, ref i);
        m_groupId = toInt(cells, ref i);
        m_level = toInt(cells, ref i);
        m_nextId = toInt(cells, ref i);
        m_needExp = toInt(cells, ref i);
        m_abilityWeight = toFloat(cells, dataTypes, ref i);
    }
}

public class EquipmentAbilityUpgradeTable : Table<EquipmentAbilityUpgradeRow>
{
    public List<EquipmentAbilityUpgradeRow> findUpgradeRows(int groupId)
    {
        var rows = findRowsLinq<EquipmentAbilityUpgradeRow>((r) =>
        {
            return r.groupId == groupId;
        });

        return rows;
    }

    public EquipmentAbilityUpgradeRow findUpgradeRow(int groupId, int level)
    {
        var row = findRowLinq<EquipmentAbilityUpgradeRow>((r) =>
        {
            return r.groupId == groupId && r.level == level;
        });

        return row;
    }
}
