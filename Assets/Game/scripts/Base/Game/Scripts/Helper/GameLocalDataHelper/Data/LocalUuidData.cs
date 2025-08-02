using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

[Serializable]
public class LocalUuidData : LocalData
{
    [SerializeField] UUID m_uuid = new UUID();

    public static LocalUuidData instance => GameLocalDataHelper.getInstance().getData<LocalUuidData>(eLocalData.Uuid.ToString());

    public override void initialize(string _name, int _id)
    {
        base.initialize(_name, _id);

        m_uuid.initialize();
    }

    public long makeUuid()
    {
        return m_uuid.make();
    }
}
