using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class UIEndcreditWindow : UIGameWindow
{
    [SerializeField] List<UIStaffRoll> m_staffRolls = new List<UIStaffRoll>();
    [SerializeField] List<float> m_staffShowTime = new List<float>();


    public override void initialize(UIWidgetData data)
    {
        base.initialize(data);
        loadStaffRoll(0);
    }

    private void loadStaffRoll(int index)
    {
        if (index < m_staffRolls.Count)
        {
            m_staffRolls[index].showStaffRoll(m_staffShowTime[index], () =>
            {
                loadStaffRoll(index + 1);
            });
        }
    }

}
