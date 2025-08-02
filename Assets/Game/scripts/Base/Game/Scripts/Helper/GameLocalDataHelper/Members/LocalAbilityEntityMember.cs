using System;
using UnityEngine;
using UnityHelper;

[Serializable]
public class LocalAbilityEntityMember : LocalEntityMember
{
    [SerializeField] LocalAbilities m_lastAbilities = new LocalAbilities(eAbilityOwner.None);

    public LocalAbilities lastAbilities { get => m_lastAbilities; protected set => m_lastAbilities = value; }
}
