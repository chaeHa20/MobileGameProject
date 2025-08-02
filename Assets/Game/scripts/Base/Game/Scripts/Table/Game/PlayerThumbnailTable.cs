using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class PlayerThumbnailRow : TableRow
{
    private int m_iconId;

    public int iconId => m_iconId;

    public override void parse(List<string> cells, List<string> dataTypes, ref int i)
    {
        base.parse(cells, dataTypes, ref i);

        m_iconId = toInt(cells, ref i);
    }
}

public class PlayerThumbnailTable : Table<PlayerThumbnailRow>
{

}
