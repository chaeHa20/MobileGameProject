using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class UIDragHand : MonoBehaviour
{
    private Action m_callback = null;

    public void initialize(GameObject parent, Action callback)
    {
        UIHelper.instance.setParent(parent, gameObject, SetParentOption.notFullAndReset());

        m_callback = callback;
    }

    public void onEndAnimationEvent()
    {
        m_callback?.Invoke();

        GameObject.Destroy(gameObject);
    }
}
