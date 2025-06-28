using System;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityHelper;

public struct Damage
{
    public float value;
    public float oriValue;  // 스킬에서 쓰인다.
    public bool isCritical;
    public int intValue => (int)value;

    public Damage(float _value, float _oriValue, bool _isCritical)
    {
        value = _value;
        oriValue = _oriValue;
        isCritical = _isCritical;
    }

    public Damage(in Damage damage)
    {
        value = damage.value;
        oriValue = damage.oriValue;
        isCritical = damage.isCritical;
    }

    public void divide(int d)
    {
        if (1 == d)
            return;

        value /= d;
        oriValue /= d;
    }
}