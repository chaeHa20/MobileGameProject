using System.Collections.Generic;
using System.Linq;
using UnityHelper;

public class TutorialRow : TableRow
{
    private int m_nextId;
    private int m_spawnMapId;
    private int m_spawnStageId;
    private eTutorialOpen m_openType;
    private int m_openTypeArg;
    private int m_argValue;
    private bool m_isFocus;
    private bool m_isArrow;
    private bool m_isDescription;
    private int m_descId;

    public int nextId => m_nextId;
    public int spawnMapId => m_spawnMapId;
    public int spawnStageId => m_spawnStageId;
    public eTutorialOpen openType => m_openType;
    public int openTypeArg => m_openTypeArg;
    public int argValue => m_argValue;
    public bool isFocus => m_isFocus;
    public bool isArrow => m_isArrow;
    public bool isDescription => m_isDescription;
    public int descId => m_descId;

    public override void parse(List<string> cells, List<string> dataTypes, ref int i)
    {
        base.parse(cells, dataTypes, ref i);
        m_nextId = toInt(cells, ref i);

        m_spawnMapId = toInt(cells, ref i);
        m_spawnStageId = toInt(cells, ref i);
        m_openType = (eTutorialOpen)toInt(cells, ref i);
        m_openTypeArg = toInt(cells, ref i);
        m_argValue = toInt(cells, ref i);
        m_isFocus = toBool(cells, dataTypes, ref i);
        m_isArrow = toBool(cells, dataTypes, ref i); ;
        m_isDescription = toBool(cells, dataTypes, ref i); ;
        m_descId = toInt(cells, ref i);
    }
}

public class TutorialTable : Table<TutorialRow>
{
    public List<TutorialRow> findRowsInStage(int mapId, int stageId)
    {
        var rows = findRowsLinq<TutorialRow>((r) =>
        {
            return r.spawnMapId == mapId && r.spawnStageId == stageId;
        });

        return rows;
    }
}
