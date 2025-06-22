using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class PathRow : TableRow
{
    private string m_path;

    public string path { get { return m_path; } }

    public override void parse(List<string> cells, List<string> dataTypes, ref int i)
    {
        base.parse(cells, dataTypes, ref i);

        m_path = toString(cells, ref i);
    }
}

public class PathTable : Table<PathRow>
{

}