using System.Collections.Generic;
using UnityHelper;

public class EnemyRow : TableRow
{
    private int m_nameId;
    private eEnemyGrade m_grade;
    private float m_hp;
    private float m_power;
    private float m_speed;
    private int m_rewardExp;
    private int m_rewardScore;

    public int nameId => m_nameId;
    public eEnemyGrade grade => m_grade;
    public float baseHp => m_hp;
    public float basePower => m_power;
    public float baseSpeed => m_speed;
    public int exp => m_rewardExp;
    public int score => m_rewardScore;

    public override void parse(List<string> cells, List<string> dataTypes, ref int i)
    {
        base.parse(cells, dataTypes, ref i);

        m_nameId = toInt(cells, ref i);
        m_grade = (eEnemyGrade)toInt(cells, ref i);
        m_hp = toFloat(cells, dataTypes, ref i);
        m_power = toFloat(cells, dataTypes, ref i);
        m_speed = toFloat(cells, dataTypes, ref i);
        m_rewardExp = toInt(cells, ref i);
        m_rewardScore = toInt(cells, ref i);
    }

}

public class EnemyTable : Table<EnemyRow>
{
    public List<EnemyRow> findRows(eEnemyGrade grade)
    {
        var rows = findRowsLinq<EnemyRow>((row) =>
        {
            return row.grade== grade;
        });

        return rows;
    }// 같은 등급의 모든 Enemy 데이터 추출

    public EnemyRow findRow(string name)
    {
        var row = findRowLinq<EnemyRow>((row) =>
        {
            return StringHelper.get(row.nameId) == name;
        });

        return row;
    }// Enemy이름으로 찾기
}
