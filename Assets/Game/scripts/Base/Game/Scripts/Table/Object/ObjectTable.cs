using System.Collections.Generic;
using UnityHelper;

public class ObjectRow : TableRow
{
    private int m_nameId;
    private int m_spriteId;
    private int m_modelId;
    private int m_deadModelId;

    public int nameId => m_nameId;
    public int spriteId => m_spriteId;
    public int modelId => m_modelId;
    public int deadModelId => m_deadModelId;

    public override void parse(List<string> cells, List<string> dataTypes, ref int i)
    {
        base.parse(cells, dataTypes, ref i);

        m_nameId = toInt(cells, ref i);
        m_spriteId = toInt(cells, ref i);
        m_modelId = toInt(cells, ref i);
        m_deadModelId = toInt(cells, ref i);
    }
}