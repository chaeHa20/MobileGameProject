using System.Collections.Generic;
using UnityHelper;

public class SoundRow : TableRow
{
    private int m_resourceId;
    private eSound m_type;
    private bool m_isLoop;
    private float m_volume;

    public int resourceId => m_resourceId;
    public eSound type => m_type;
    public bool isLoop => m_isLoop;
    public float volume => m_volume;

    public override void parse(List<string> cells, List<string> dataTypes, ref int i)
    {
        base.parse(cells, dataTypes, ref i);

        m_resourceId = toInt(cells, ref i);
        m_type = (eSound)toInt(cells, ref i);
        m_isLoop = toBool(cells, dataTypes, ref i);
        m_volume = toFloat(cells, dataTypes, ref i);
    }
}

public class SoundTable : Table<SoundRow>
{
    public List<SoundRow> findRowsBySoundType(eSound type)
    {
        var rows = findRowsLinq<SoundRow>((r) =>
        {
            return r.type == type;
        });

        return rows;
    }
}
