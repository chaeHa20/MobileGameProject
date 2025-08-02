using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class PushNotificationRow : TableRow
{
    public enum eHourType
    {
        AbsoluteTime = 1,
        AfterTime = 2,
    }

    private eHourType m_hourType;
    private List<int> m_hours;
    private bool m_isRepeat;
    private int m_stringId;
    private int m_maxNotificationTime;

    public eHourType hourType => m_hourType;
    public bool isRepeat => m_isRepeat;
    public int stringId => m_stringId;
    public int maxNotificationTime => m_maxNotificationTime;

    public override void parse(List<string> cells, List<string> dataTypes, ref int i)
    {
        base.parse(cells, dataTypes, ref i);

        m_hourType = (eHourType)toInt(cells, ref i);
        m_hours = toList<int>(cells, ref i);
        m_isRepeat = toBool(cells, dataTypes, ref i);
        m_stringId = toInt(cells, ref i);
        m_maxNotificationTime = toInt(cells, ref i);
    }

    public void forEach(Action<int> callback)
    {
        foreach(var hour in m_hours)
        {
            callback(hour);
        }
    }
}

public class PushNotificationTable : Table<PushNotificationRow>
{

}
