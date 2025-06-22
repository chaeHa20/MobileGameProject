using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class UIAbilityWindowTab : UITab
{
    public class Data : BaseData
    {
        public int defaultTabId;
    }

    public override void initialize(BaseData baseData)
    {
        base.initialize(baseData);

        var data = baseData as Data;

        setTab(data.defaultTabId);
    }
}
