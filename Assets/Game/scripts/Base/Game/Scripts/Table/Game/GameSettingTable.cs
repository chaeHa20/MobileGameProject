using System.Collections.Generic;
using System.Numerics;
using UnityHelper;

public class GameSettingRow : TableRow
{
    public enum eType
    {
        None,
        ResetTime, 
    }

    private eType m_type;
    private string m_value;

    public eType type => m_type;
    public string value => m_value;

    public override void parse(List<string> cells, List<string> dataTypes, ref int i)
    {
        base.parse(cells, dataTypes, ref i);

        m_type = (eType)toInt(cells, ref i);
        m_value = toString(cells, ref i);
    }
}

public class GameSettingTable : Table<GameSettingRow>
{
    private Dictionary<GameSettingRow.eType, GameSettingRow> m_values = new Dictionary<GameSettingRow.eType, GameSettingRow>();

    public GameSettingRow findRow(GameSettingRow.eType type)
    {
        var row = findRowLinq<GameSettingRow>((r) =>
        {
            return r.type == type;
        });
        return row;
    }

    protected override void loadDone()
    {
        base.loadDone();

        setValues();
    }

    private void setValues()
    {
        var e = getEnumerator();
        while (e.MoveNext())
        {
            var row = e.Current.Value as GameSettingRow;
            if (m_values.ContainsKey(row.type))
            {
                if (Logx.isActive)
                    Logx.error($"Duplicated GameSettingTable Type {row.type}");

                continue;
            }

            m_values.Add(row.type, row);
        }
    }

    public int getValueInt(GameSettingRow.eType type)
    {
        if (m_values.TryGetValue(type, out GameSettingRow row))
        {
            return StringHelper.toInt32(row.value);
        }
        else
        {
            if (Logx.isActive)
                Logx.error($"Failed GameSettingRow getValueInt {type}");

            return 0;
        }
    }
    public long getValueLong(GameSettingRow.eType type)
    {
        if (m_values.TryGetValue(type, out GameSettingRow row))
        {
            return StringHelper.toInt64(row.value);
        }
        else
        {
            if (Logx.isActive)
                Logx.error($"Failed GameSettingRow getValueInt {type}");

            return 0;
        }
    }

    public float getValueFloat(GameSettingRow.eType type)
    {
        if (m_values.TryGetValue(type, out GameSettingRow row))
        {
            return StringHelper.toSingle(row.value);
        }
        else
        {
            if (Logx.isActive)
                Logx.error($"Failed GameSettingRow getValueFloat {type}");

            return 0;
        }
    }

    public BigInteger getValueBigInt(GameSettingRow.eType type)
    {
        if (m_values.TryGetValue(type, out GameSettingRow row))
        {
            if (BigInteger.TryParse(row.value, out BigInteger value))
            {
                return value;
            }
            else
            {
                if (Logx.isActive)
                    Logx.error($"Failed GameSettingRow getValueBigInt, invalid type {type}");

                return 0;
            }
        }
        else
        {
            if (Logx.isActive)
                Logx.error($"Failed GameSettingRow getValueBigInt, not found {type}");

            return 0;
        }
    }

    public string getValue(GameSettingRow.eType type)
    {
        if (m_values.TryGetValue(type, out GameSettingRow row))
        {
            return row.value;
        }
        else
        {
            if (Logx.isActive)
                Logx.error($"Failed GameSettingRow getValueInt {type}");

            return null;
        }
    }
}