using System.Collections.Generic;
using UnityHelper;

public class EquipmentRow : TableRow
{
    private eEquip m_equipType;
    private int m_abilityId;
    private List<int> m_abilityGroupIds;
    private List<int> m_ability_initValues;

    public eEquip equipType=> m_equipType;
    public int abilityId => m_abilityId;
    public List<int> abilityGroupIds=> m_abilityGroupIds;
    public List<int> ability_initValues => m_ability_initValues;

    public override void parse(List<string> cells, List<string> dataTypes, ref int i)
    {
        base.parse(cells, dataTypes, ref i);

        m_equipType = (eEquip)toInt(cells, ref i);
        m_abilityId = toInt(cells, ref i);
        m_abilityGroupIds = toList<int>(cells, ref i);
        m_ability_initValues = toList<int>(cells, ref i);
    }
}

public class EquipmentTable : Table<EquipmentRow>
{
    
}
