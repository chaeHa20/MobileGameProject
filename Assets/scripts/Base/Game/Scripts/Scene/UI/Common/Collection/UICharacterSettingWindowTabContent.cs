using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class UICharacterSettingWindowTabContent : UITabContent
{
    public override void select()
    {
        base.select();
        gameObject.SetActive(true);
    }

    public override void unselect()
    {
        base.unselect();
        gameObject.SetActive(false);
    }
}
