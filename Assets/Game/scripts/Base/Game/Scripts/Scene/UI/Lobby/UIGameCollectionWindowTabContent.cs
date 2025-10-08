using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class UIGameCollectionWindowTabContent : UITabContent
{
    [SerializeField] UIGameCollectionItemMode m_tabMode;

    public UIGameCollectionItemMode tabMode => m_tabMode;
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
