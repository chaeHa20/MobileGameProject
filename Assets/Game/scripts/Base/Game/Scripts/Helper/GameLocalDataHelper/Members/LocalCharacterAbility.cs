using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using UnityEngine;
using UnityHelper;

[Serializable]
public class LocalCharacterAbility
{
    [SerializeField] eCharacterAbility m_type = eCharacterAbility.None;
    [SerializeField] LocalValue m_value = new LocalValue();

    public eCharacterAbility type => m_type;
    public string value => m_value.value;
    public int valueInt => m_value.toInt;
    public float valueFloat => m_value.toFloat;
    public BigInteger valueBigInteger => m_value.toBigInteger;
    public BigMoney valueBigMoney => m_value.toBigMoney;

    private static Dictionary<eCharacterAbility, LocalValue.eValue> abilityValueTypes = null;

    private static void initAbilityValueTypes()
    {
        abilityValueTypes = new Dictionary<eCharacterAbility, LocalValue.eValue>();
    }

    public static LocalValue.eValue abilityTypeToValueType(eCharacterAbility abilityType)
    {
        if (null == abilityValueTypes)
            initAbilityValueTypes();

        if (abilityValueTypes.TryGetValue(abilityType, out LocalValue.eValue valueType))
            return valueType;

        return LocalValue.eValue.Float;
    }

    public LocalCharacterAbility()
    {
        m_type = eCharacterAbility.None;
        m_value = new LocalValue();
    }

    public LocalCharacterAbility(eCharacterAbility type, string value)
    {
        m_type = type;

        setValue(value);
    }

    public LocalCharacterAbility clone()
    {
        var ability = new LocalCharacterAbility
        {
            m_type = type,
            m_value = new LocalValue(m_value),
        };

        return ability;
    }

    public void multiplyValue(string originalValue)
    {
        m_value.multiply(originalValue);
    }

    public void addValue(string originalValue)
    {
        m_value.add(originalValue);
    }

    public void delValue(string originalValue)
    {
        m_value.del(originalValue);
    }

    public void setValue(string value)
    {
        var valueType = abilityTypeToValueType(m_type);

        if (LocalValue.eValue.Float == valueType)
        {
            m_value = new LocalValue(StringHelper.toFormat("{0:0.00}", StringHelper.toSingle(value)), valueType);
        }
        else
        {
            m_value = new LocalValue(value, valueType);
        }
    }

    public string toSignValue()
    {
        return m_value.toSign();
    }

    public string toValue()
    {
        return m_value.toString();
    }

    public void applyRatio(float ratio)
    {
        m_value.applyRatio(ratio);
    }

    public static LocalCharacterAbility operator -(LocalCharacterAbility a, LocalCharacterAbility b)
    {
        var r = a.clone();
        r.delValue(b.m_value.value);
        return r;
    }


    public string toString()
    {
        return string.Format("{0} {1}", m_type, m_value.toString());
    }

    public LocalValue.eValue getValueType()
    {
        return m_value.type;
    }// TODO : 2024-05-30 by pms
}

[Serializable]
public class LocalCharacterAbilities

{
    [SerializeField] List<LocalCharacterAbility> m_characterAbilities = new List<LocalCharacterAbility>();

    public int abilityCount => m_characterAbilities.Count;

    public List<LocalCharacterAbility>.Enumerator getAbilityEnumerator()
    {
        return m_characterAbilities.GetEnumerator();
    }

    public void forEachSkills(Action<LocalCharacterAbility> callback)
    {
        foreach (var skill in m_characterAbilities)
        {
            callback(skill);
        }
    }

    public LocalCharacterAbility getAbility(int index)
    {
        if (abilityCount <= index)
            return null;

        return m_characterAbilities[index];
    }

    public LocalCharacterAbility addAbility(eCharacterAbility abilityType, string abilityValue)
    {
        if (tryGetAbility(abilityType, out LocalCharacterAbility ability))
        {
            ability.addValue(abilityValue);
        }
        else
        {
            ability = new LocalCharacterAbility(abilityType, abilityValue);
            m_characterAbilities.Add(ability);
        }

        return ability;
    }

    public void multiplyAbility(eCharacterAbility abilityType, string value)
    {
        if (tryGetAbility(abilityType, out LocalCharacterAbility ability))
        {
            ability.multiplyValue(value);
        }
    }

    public void setAbilityToFirst(eCharacterAbility abilityType, string value)
    {
        if (tryGetAbility(abilityType, out LocalCharacterAbility ability))
        {
            ability.setValue(value);
        }
        else
        {
            ability = new LocalCharacterAbility(abilityType, value);
            m_characterAbilities.Insert(0, ability);
        }
    }

    public void addAbility(LocalCharacterAbility ability)
    {
        if (null == ability)
            return;

        addAbility(ability.type, ability.value);
    }

    public void setAbility(eCharacterAbility abilityType, string abilityValue)
    {
        if (tryGetAbility(abilityType, out LocalCharacterAbility ability))
        {
            ability.setValue(abilityValue);
        }
        else
        {
            ability = new LocalCharacterAbility(abilityType, abilityValue);
            m_characterAbilities.Add(ability);
        }
    }

    public void setAbility(LocalCharacterAbility ability)
    {
        setAbility(ability.type, ability.value);
    }

    public void setAbilities(LocalCharacterAbilities abilities)
    {
        var e = abilities.getAbilityEnumerator();
        while (e.MoveNext())
        {
            setAbility(e.Current);
        }
    }

    public void addAbilities(LocalCharacterAbilities abilities)
    {
        var e = abilities.getAbilityEnumerator();
        while (e.MoveNext())
        {
            addAbility(e.Current);
        }
    }

    public bool isExistAbility(eCharacterAbility abilityType)
    {
        return m_characterAbilities.Exists((ca) => { return ca.type == abilityType; });
    }

    public bool tryGetAbility(eCharacterAbility abilityType, out LocalCharacterAbility ability)
    {
        ability = (from ca in m_characterAbilities
                 where ca.type == abilityType
                   select ca).FirstOrDefault();

        return null != ability;
    }

    public int getIntValue(eCharacterAbility abilityType)
    {
#if UNITY_EDITOR
        checkAbilityTypeAndValueType(abilityType, LocalValue.eValue.Int);
#endif

        if (tryGetAbility(abilityType, out LocalCharacterAbility ability))
            return ability.valueInt;

        return 0;
    }

    public bool tryGetIntValue(eCharacterAbility abilityType, out int value)
    {
#if UNITY_EDITOR
        checkAbilityTypeAndValueType(abilityType, LocalValue.eValue.Int);
#endif

        if (tryGetAbility(abilityType, out LocalCharacterAbility ability))
        {
            value = ability.valueInt;
            return true;
        }

        value = 0;
        return false;
    }

    public float getFloatValue(eCharacterAbility abilityType)
    {
#if UNITY_EDITOR
        checkAbilityTypeAndValueType(abilityType, LocalValue.eValue.Float);
#endif

        if (tryGetAbility(abilityType, out LocalCharacterAbility ability))
            return ability.valueFloat;

        return 0.0f;
    }

    public bool tryGetFloatValue(eCharacterAbility abilityType, out float value)
    {
#if UNITY_EDITOR
        checkAbilityTypeAndValueType(abilityType, LocalValue.eValue.Float);
#endif

        if (tryGetAbility(abilityType, out LocalCharacterAbility ability))
        {
            value = ability.valueFloat;
            return true;
        }

        value = 0.0f;
        return false;
    }

    public BigInteger getBigIntegerValue(eCharacterAbility abilityType)
    {
#if UNITY_EDITOR
        checkAbilityTypeAndValueType(abilityType, LocalValue.eValue.BigInteger);
#endif

        if (tryGetAbility(abilityType, out LocalCharacterAbility ability))
            return ability.valueBigInteger;

        return 0;
    }

    public bool tryGetBigIntegerValue(eCharacterAbility abilityType, out BigInteger value)
    {
#if UNITY_EDITOR
        checkAbilityTypeAndValueType(abilityType, LocalValue.eValue.BigInteger);
#endif

        if (tryGetAbility(abilityType, out LocalCharacterAbility ability))
        {
            value = ability.valueBigInteger;
            return true;
        }

        value = 0;
        return false;
    }

    public string getValue(eCharacterAbility abilityType)
    {
        if (tryGetAbility(abilityType, out LocalCharacterAbility ability))
            return ability.value;

        return null;
    }

    public bool tryGetValue(eCharacterAbility abilityType, out string value)
    {
        if (tryGetAbility(abilityType, out LocalCharacterAbility ability))
        {
            value = ability.value;
            return true;
        }

        value = null;
        return false;
    }

    public LocalCharacterAbilities clone()
    {
        var abilities = new LocalCharacterAbilities();
        foreach (var ability in m_characterAbilities)
        {
            abilities.m_characterAbilities.Add(ability.clone());
        }

        return abilities;
    }

    public static LocalCharacterAbilities operator -(LocalCharacterAbilities a, LocalCharacterAbilities b)
    {
        var abilities = new LocalCharacterAbilities();
        for (int i = 0; i < a.abilityCount; ++i)
        {
            var aAbility = a.getAbility(i);
            var bAbility = b.getAbility(i);
            abilities.addAbility(aAbility - bAbility);
        }

        return abilities;
    }

    public float calcAbilityTime(eCharacterAbility capacityAbility, eCharacterAbility speedAbility)
    {
        var speed = getBigIntegerValue(speedAbility);
        var capacity = getBigIntegerValue(capacityAbility);

        if (0 >= speed)
            return 0.0f;

        var divValue = BigInteger.DivRem(capacity, speed, out BigInteger remainder);

        var t = (float)divValue;
        var strRemainder = StringHelper.toString(remainder);
        if (0 < strRemainder.Length)
        {
            // 소수 첫째자리만 구한다.(첫째 자리만 의미 있을 거 같아서)
            var f = strRemainder[0];
            t += (int)Char.GetNumericValue(f) / 10.0f;
        }

        return t;
    }

    public string toString(string tab = "")
    {
        var sb = new StringBuilder();
        foreach (var ability in m_characterAbilities)
        {
            sb.AppendLine(tab + ability.toString());
        }

        return sb.ToString();
    }

#if UNITY_EDITOR
    private void checkAbilityTypeAndValueType(eCharacterAbility skillType, LocalValue.eValue valueType)
    {
        var validValueType = LocalCharacterAbility.abilityTypeToValueType(skillType);
        if (validValueType != valueType)
        {
            if (Logx.isActive)
            {
                Logx.error("Failed checkAbilityTypeAndValueType, abilityType {0} : validValueType {1}, valueType {2}", skillType, validValueType, valueType);
            }
        }
    }
#endif
}