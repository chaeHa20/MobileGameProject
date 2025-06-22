using UnityHelper;

public class AbilityEntityData : EntityData
{
    public LocalAbilities abilities;
    // public LocalAbilityStatusEffect abilityStatusEffect;
}

public class AbilityEntity : Entity
{
    private long m_battlePower = 0;
    private LocalAbilities m_abilities = null;
    // private LocalAbilityStatusEffect m_abilityStatusEffect = null;

    protected LocalAbilities abilities => m_abilities;
    public long battlePower => m_battlePower;

    public override void initialize(EntityData entityData)
    {
        var d = entityData as AbilityEntityData;

        base.initialize(entityData);
        setAbilities(d.abilities);
    }

    public virtual void setAbilities(LocalAbilities abilities)
    {
        m_abilities = abilities;
    }

    protected int getAbilityValueInt(eAbility abilityType)
    {
        return m_abilities.getIntValue(abilityType);
    }

    public float getAbilityValueFloat(eAbility abilityType)
    {
        return m_abilities.getFloatValue(abilityType);
    }

    public string getAbilityValue(eAbility abilityType)
    {
        return m_abilities.getValue(abilityType);
    }

    protected bool isExistAbility(eAbility abilityType)
    {
        return m_abilities.isExistAbility(abilityType);
    }

    //private bool isAbilityStatusEffectCoolTime()
    //{
    //    if (null == m_abilityStatusEffect)
    //        return false;

    //    return TimeHelper.isCoolTime(m_abilityStatusEffect.coolTime);
    //}

    //private int getAbilityStatusEffectValue(eAbility abilityType)
    //{
    //    if (null == m_abilityStatusEffect)
    //        return 0;

    //    return m_abilityStatusEffect.getValue(abilityType);
    //}

    //protected BigMoney getAbilityBigMoney(eAbility abilityType, bool nagativeAbilityStatusEffectIsBuff)
    //{
    //    var bigMoney = new BigMoney(getAbilityValue(abilityType));

    //    if (isAbilityStatusEffectCoolTime())
    //    {
    //        var abilityStatusEffectValue = getAbilityStatusEffectValue(abilityType);
    //        if (0 != abilityStatusEffectValue)
    //        {
    //            if (nagativeAbilityStatusEffectIsBuff)
    //                abilityStatusEffectValue *= -1;

    //            bigMoney.value += (bigMoney.value * abilityStatusEffectValue) / 100;
    //        }
    //    }

    //    return bigMoney;
    //}

    protected float applyAbilityStatusEffect(float abilityValue, eAbility abilityType, bool nagativeAbilityStatusEffectIsBuff)
    {
        if (true)// isAbilityStatusEffectCoolTime())
        {
            var abilityStatusEffectValue = 1.0f;// getAbilityStatusEffectValue(abilityType);
            if (0 != abilityStatusEffectValue)
            {
                if (nagativeAbilityStatusEffectIsBuff)
                    abilityStatusEffectValue *= -1;

                abilityValue += (abilityValue * abilityStatusEffectValue) / 100.0f;
            }
        }

        return abilityValue;
    }
}
