using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class PoolRow : TableRow
{
    private int m_initCount;
    private int m_maxCount;
    private bool m_isUI;
    private bool m_isAsync;
    private int m_reallocateCount;
    private bool m_isPreload;

    public int initCount => m_initCount;
    public int maxCount => m_maxCount;
    public bool isUI => m_isUI;
    public bool isAync => m_isAsync;
    public int reallocateCount => m_reallocateCount;
    public bool isPreload => m_isPreload;

    public override void parse(List<string> cells, List<string> dataTypes, ref int i)
    {
        base.parse(cells, dataTypes, ref i);

        m_initCount = toInt(cells, ref i);
        m_maxCount = toInt(cells, ref i);
        m_isUI = toBool(cells, dataTypes, ref i);
        m_isAsync = toBool(cells, dataTypes, ref i);
        m_reallocateCount = toInt(cells, ref i);
        m_isPreload = toBool(cells, dataTypes, ref i);
    }
}

public class PoolTable : Table<PoolRow>
{

}
