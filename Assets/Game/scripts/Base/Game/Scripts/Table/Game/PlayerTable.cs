using System.Collections.Generic;
using UnityHelper;

public class PlayerRow : TableRow
{
    private float m_hp;
    private float m_power;
    private float m_speed;
    private int m_needExp;
    private float m_hpLevelWeight;
    private float m_speedLevelWeight;
    private float m_needExpLevelWeight;

    public float baseHp => m_hp;
    public float basePower => m_power;
    public float baseSpeed => m_speed;
    public int baseNeedExp => m_needExp;
    public float hpWeight => m_hpLevelWeight;
    public float speedWeight => m_speedLevelWeight;
    public float needExpWeight => m_needExpLevelWeight;

    public override void parse(List<string> cells, List<string> dataTypes, ref int i)
    {
        base.parse(cells, dataTypes, ref i);
        
        m_hp = toFloat(cells, dataTypes, ref i);
        m_power = toFloat(cells, dataTypes, ref i);
        m_speed = toFloat(cells, dataTypes, ref i);
        m_needExp = toInt(cells, ref i);
        m_hpLevelWeight = toFloat(cells, dataTypes, ref i);
        m_speedLevelWeight = toFloat(cells, dataTypes, ref i);
        m_needExpLevelWeight = toFloat(cells, dataTypes, ref i);
    }

}

public class PlayerTable : Table<PlayerRow>
{

}
