using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class UIGameScene : UIScene
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override void back()
    {
        base.back();

        GameUIHelper.getInstance().backWindow();
    }
}
