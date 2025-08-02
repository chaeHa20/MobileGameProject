using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class ResourceRow : TableRow
{
    private string m_filename;
    private int m_pathId;
    private int m_poolId;

    public string filename { get { return m_filename; } }
    public int pathId { get { return m_pathId; } }
    public int poolId { get { return m_poolId; } }

    public override void parse(List<string> cells,List<string> dataTypes, ref int i)
    {
        base.parse(cells, dataTypes, ref i);

        m_filename = toString(cells, ref i);
        m_pathId = toInt(cells, ref i);
        m_poolId = toInt(cells, ref i);
    }
}

public class ResourceTable : Table<ResourceRow>
{

}