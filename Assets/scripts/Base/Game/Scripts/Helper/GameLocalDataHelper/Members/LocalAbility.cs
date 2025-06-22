using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Numerics;
using UnityHelper;
using System.Text;

[Serializable]
public class LocalAbility
{
    [SerializeField] eAbility m_type = eAbility.None;
    [SerializeField] eAbilityOwner m_owner = eAbilityOwner.None;
    [SerializeField] LocalAbilityValue m_value = new LocalAbilityValue();

    public eAbility type => m_type;
    public eAbilityOwner owner => m_owner;
    public string value => m_value.value;
    public int valueInt => m_value.toInt;
    public float valueFloat => m_value.toFloat;
    public BigInteger valueBigInteger => m_value.toBigInteger;
    public BigMoney valueBigMoney => m_value.toBigMoney;

    private static Dictionary<eAbility, LocalValue.eValue> abilityValueTypes = null;

    private static void initAbilityValueTypes()
    {
        abilityValueTypes = new Dictionary<eAbility, LocalValue.eValue>();

        // abilityValueTypes.Add(eAbility.ChangeResource, LocalValue.eValue.Int);
    }

    public static LocalValue.eValue abilityTypeToValueType(eAbility abilityType)
    {
        if (null == abilityValueTypes)
            initAbilityValueTypes();

        if (abilityValueTypes.TryGetValue(abilityType, out LocalValue.eValue valueType))
            return valueType;

        return LocalValue.eValue.Float;
    }

    public LocalAbility()
    {
        m_type = eAbility.None;
        m_owner = eAbilityOwner.None;
        m_value = new LocalAbilityValue();
    }

    public LocalAbility(eAbility type, string value, eAbilityOwner owner)
    {
        m_type = type;
        m_owner = owner;

        setValue(value);
    }

    public LocalAbility(LocalAbility ability)
    {
        m_type = ability.type;
        m_owner = ability.owner;
        m_value = new LocalAbilityValue(ability.m_value);
    }

    public LocalAbility clone()
    {
        var ability = new LocalAbility
        {
            m_type = type,
            m_owner = owner,
            m_value = new LocalAbilityValue(m_value),
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
            m_value = new LocalAbilityValue(StringHelper.toFormat("{0:0.00}", StringHelper.toSingle(value)), valueType);
        }
        else
        {
            m_value = new LocalAbilityValue(value, valueType);
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

    public static LocalAbility operator -(LocalAbility a, LocalAbility b)
    {
        var r = a.clone();
        r.delValue(b.m_value.originalValue);
        return r;
    }

    public string toString()
    {
        return string.Format("{0} {1}", m_type, m_value.toString());
    }

    public LocalValue.eValue getValueType()
    {
        return m_value.getValueType();
    }// TODO : 2024-05-30 by pms
}

[Serializable]
public class LocalAbilities
{
    [SerializeField] eAbilityOwner m_owner = eAbilityOwner.None;
    [SerializeField] List<LocalAbility> m_abilities = new List<LocalAbility>();

    public int abilityCount => m_abilities.Count;
    public eAbilityOwner owner => m_owner;

    public LocalAbilities(eAbilityOwner owner)
    {
        m_owner = owner;
    }

    public LocalAbilities(LocalAbilities abilities)
    {
        m_owner = abilities.owner;
        addAbilities(abilities);
    }

    public List<LocalAbility>.Enumerator getAbilityEnumerator()
    {
        return m_abilities.GetEnumerator();
    }

    public void forEachAbilities(Action<LocalAbility> callback)
    {
        foreach(var ability in m_abilities)
        {
            callback(ability);
        }
    }

    public LocalAbility getAbility(int index)
    {
        if (abilityCount <= index)
            return null;

        return m_abilities[index];
    }

    public LocalAbility addAbility(eAbility abilityType, string abilityValue)
    {
        if (tryGetAbility(abilityType, out LocalAbility ability))
        {
            ability.addValue(abilityValue);
        }
        else
        {
            ability = new LocalAbility(abilityType, abilityValue, m_owner);
            m_abilities.Add(ability);
        }

        return ability;
    }

    public void multiplyAbility(eAbility abilityType, string abilityValue)
    {
        if (tryGetAbility(abilityType, out LocalAbility ability))
        {
            ability.multiplyValue(abilityValue);
        }
    }

    public void setAbilityToFirst(eAbility abilityType, string abilityValue)
    {
        if (tryGetAbility(abilityType, out LocalAbility ability))
        {
            ability.setValue(abilityValue);
        }
        else
        {
            ability = new LocalAbility(abilityType, abilityValue, m_owner);
            m_abilities.Insert(0, ability);
        }
    }

    public void addAbility(LocalAbility ability)
    {
        if (null == ability)
            return;

        addAbility(ability.type, ability.value);
    }

    public void setAbility(eAbility abilityType, string abilityValue)
    {
        if (tryGetAbility(abilityType, out LocalAbility ability))
        {
            ability.setValue(abilityValue);
        }
        else
        {
            ability = new LocalAbility(abilityType, abilityValue, m_owner);
            m_abilities.Add(ability);
        }
    }

    public void setAbility(LocalAbility ability)
    {
        setAbility(ability.type, ability.value);
    }

    public void setAbilities(LocalAbilities abilities)
    {
        var e = abilities.getAbilityEnumerator();
        while(e.MoveNext())
        {
            setAbility(e.Current);
        }
    }

    public void addAbilities(LocalAbilities abilities)
    {
        var e = abilities.getAbilityEnumerator();
        while (e.MoveNext())
        {
            addAbility(e.Current);
        }
    }

    public bool isExistAbility(eAbility abilityType)
    {
        return m_abilities.Exists((ca) => { return ca.type == abilityType; });
    }

    public bool tryGetAbility(eAbility abilityType, out LocalAbility ability)
    {
        ability = (from ca in m_abilities
                       where ca.type == abilityType
                       select ca).FirstOrDefault();

        return null != ability;
    }

    public int getIntValue(eAbility abilityType)
    {
#if UNITY_EDITOR
        checkAbilityTypeAndValueType(abilityType, LocalValue.eValue.Int);
#endif

        if (tryGetAbility(abilityType, out LocalAbility ability))
            return ability.valueInt;

        return 0;
    }

    public bool tryGetIntValue(eAbility abilityType, out int value)
    {
#if UNITY_EDITOR
        checkAbilityTypeAndValueType(abilityType, LocalValue.eValue.Int);
#endif

        if (tryGetAbility(abilityType, out LocalAbility ability))
        {
            value = ability.valueInt;
            return true;
        }

        value = 0;
        return false;
    }

    public float getFloatValue(eAbility abilityType)
    {
#if UNITY_EDITOR
        checkAbilityTypeAndValueType(abilityType, LocalValue.eValue.Float);
#endif

        if (tryGetAbility(abilityType, out LocalAbility ability))
            return ability.valueFloat;

        return 0.0f;
    }

    public bool tryGetFloatValue(eAbility abilityType, out float value)
    {
#if UNITY_EDITOR
        checkAbilityTypeAndValueType(abilityType, LocalValue.eValue.Float);
#endif

        if (tryGetAbility(abilityType, out LocalAbility ability))
        {
            value = ability.valueFloat;
            return true;
        }

        value = 0.0f;
        return false;
    }

    public BigInteger getBigIntegerValue(eAbility abilityType)
    {
#if UNITY_EDITOR
        checkAbilityTypeAndValueType(abilityType, LocalValue.eValue.BigInteger);
#endif

        if (tryGetAbility(abilityType, out LocalAbility ability))
            return ability.valueBigInteger;

        return 0;
    }

    public bool tryGetBigIntegerValue(eAbility abilityType, out BigInteger value)
    {
#if UNITY_EDITOR
        checkAbilityTypeAndValueType(abilityType, LocalValue.eValue.BigInteger);
#endif

        if (tryGetAbility(abilityType, out LocalAbility ability))
        {
            value = ability.valueBigInteger;
            return true;
        }

        value = 0;
        return false;
    }

    public string getValue(eAbility abilityType)
    {
        if (tryGetAbility(abilityType, out LocalAbility ability))
            return ability.value;

        return null;
    }

    public bool tryGetValue(eAbility abilityType, out string value)
    {
        if (tryGetAbility(abilityType, out LocalAbility ability))
        {
            value = ability.value;
            return true;
        }

        value = null;
        return false;
    }

    public LocalAbilities clone()
    {
        var abilities = new LocalAbilities(m_owner);
        foreach(var ability in m_abilities)
        {
            abilities.m_abilities.Add(ability.clone());
        }

        return abilities;
    }

    public static LocalAbilities operator -(LocalAbilities a, LocalAbilities b)
    {
        if (Logx.isActive)
            Logx.assert(a.owner == b.owner, "Failed LocalAbilities sub, not equal eAbilityOwner");

        var abilities = new LocalAbilities(a.owner);
        for (int i = 0; i < a.abilityCount; ++i)
        {
            var aAbility = a.getAbility(i);
            var bAbility = b.getAbility(i);
            abilities.addAbility(aAbility - bAbility);
        }

        return abilities;
    }

    public float calcAbilityTime(eAbility capacityAbility, eAbility speedAbility)
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
        foreach(var ability in m_abilities)
        {
            sb.AppendLine(tab + ability.toString());
        }

        return sb.ToString();
    }

#if UNITY_EDITOR
    private void checkAbilityTypeAndValueType(eAbility abilityType, LocalValue.eValue valueType)
    {
        var validValueType = LocalAbility.abilityTypeToValueType(abilityType);
        if (validValueType != valueType)
        {
            if (Logx.isActive)
            {
                Logx.error("Failed checkAbilityTypeAndValueType, abilityType {0} : validValueType {1}, valueType {2}", abilityType, validValueType, valueType);
            }
        }
    }
#endif
}