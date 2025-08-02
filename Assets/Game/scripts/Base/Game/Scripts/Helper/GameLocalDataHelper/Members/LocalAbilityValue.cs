using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityHelper;
using System.Numerics;

[Serializable]
public class LocalAbilityValue
{
    [SerializeField] LocalValue m_originalValue = new LocalValue();
    [SerializeField] LocalValue m_lastValue = new LocalValue();
    
    public string value => m_lastValue.value;
    public string originalValue => m_originalValue.value;
    public int toInt => m_lastValue.toInt;
    public float toFloat => m_lastValue.toFloat;
    public BigInteger toBigInteger => m_lastValue.toBigInteger;
    public BigMoney toBigMoney => m_lastValue.toBigMoney;
    
    public LocalAbilityValue()
    {
        m_originalValue = new LocalValue();
    }

    public LocalAbilityValue(string originalValue, LocalValue.eValue type)
    {
        m_originalValue = new LocalValue(originalValue, type);
        m_lastValue = new LocalValue(m_originalValue);
    }

    public LocalAbilityValue(LocalAbilityValue value)
    {
        m_originalValue = new LocalValue(value.m_originalValue);
        m_lastValue = new LocalValue(value.m_lastValue);
    }

    public void multiply(string originalValue)
    {
        m_originalValue.multiply(originalValue);
    }

    public void add(string originalValue)
    {
        m_originalValue.add(originalValue);
    }

    public void del(string originalValue)
    {
        m_originalValue.del(originalValue);
    }

    public string toSign()
    {
        return m_lastValue.toSign();
    }

    public string toString()
    {
        return m_lastValue.toString();
    }

    public void applyRatio(float ratio)
    {
        m_originalValue.applyRatio(ratio);
    }

    public LocalValue.eValue getValueType()
    {
        return m_lastValue.type;
    }// TODO : 2024-05-30 by pms
}