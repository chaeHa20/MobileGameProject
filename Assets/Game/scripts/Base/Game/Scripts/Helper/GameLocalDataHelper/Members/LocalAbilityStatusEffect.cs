using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable]
public class LocalAbilityStatusEffectValue
{
    [SerializeField] eAbility m_abilityType = eAbility.None;
    [SerializeField] int m_abilityValue = 0;

    public eAbility abilityType => m_abilityType;
    public int abilityValue => m_abilityValue;

    public void initialize(eAbility abilityType, int abilityValue)
    {
        m_abilityType = abilityType;
        m_abilityValue = abilityValue;
    }
}

[Serializable]
public class LocalAbilityStatusEffect
{
    [SerializeField] long m_coolTime = 0;
    [SerializeField] int m_itemId = 0;
    [SerializeField] List<LocalAbilityStatusEffectValue> m_values = new List<LocalAbilityStatusEffectValue>();

    public long coolTime => m_coolTime;
    public int itemId => m_itemId;

    public void initialize(int itemId, long coolTime)
    {
        m_itemId = itemId;
        m_coolTime = coolTime;
    }

    public void addValue(eAbility abilityType, int abilityValue)
    {
        var value = new LocalAbilityStatusEffectValue();
        value.initialize(abilityType, abilityValue);
        m_values.Add(value);
    }

    public int getValue(eAbility abilityType)
    {
        var value = (from v in m_values
                     where v.abilityType == abilityType
                     select v.abilityValue).FirstOrDefault();

        return value;
    }
}