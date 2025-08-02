using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using System;
using UnityHelper;

[Serializable]
public class LocalValue
{
    public enum eValue { Int, Float, BigInteger };

    [SerializeField] eValue m_type = eValue.Int;
    [SerializeField] string m_value = null;

    public string value { get => m_value; set => m_value = value; }
    public eValue type => m_type;
    public int toInt => StringHelper.toInt32(m_value);
    public float toFloat => StringHelper.toSingle(m_value);
    public BigInteger toBigInteger => StringHelper.toBigInt(m_value);
    public BigMoney toBigMoney => new BigMoney(StringHelper.toBigInt(m_value));

    public LocalValue()
    {
        m_value = "";
        m_type = eValue.Int;
    }

    public LocalValue(string value, eValue type)
    {
        m_value = value;
        m_type = type;

#if UNITY_EDITOR
        if (eValue.BigInteger == type)
            MathHelper.checkValidBigInteger(value);
#endif
    }

    public LocalValue(LocalValue value)
    {
        m_value = value.m_value;
        m_type = value.m_type;

#if UNITY_EDITOR
        if (eValue.BigInteger == m_type)
            MathHelper.checkValidBigInteger(m_value);
#endif
    }

    public void multiply(string value)
    {
        if (eValue.Int == m_type)
        {
            setValue(StringHelper.toInt32(m_value) * StringHelper.toInt32(value));
        }
        else if (eValue.Float == m_type)
        {
            setValue(StringHelper.toSingle(m_value) * StringHelper.toSingle(value));
        }
        else if (eValue.BigInteger == m_type)
        {
            setValue(StringHelper.toBigInt(m_value) * StringHelper.toBigInt(value));
        }
    }

    public void multiply(eValue valueType, string value)
    {
        if (eValue.Int == m_type)
        {
            var curValue = toInt;

            if (eValue.Int == valueType)
            {
                setValue(curValue * StringHelper.toInt32(value));
            }
            else if (eValue.Float == valueType)
            {
                setValue((int)(curValue * StringHelper.toSingle(value)));
            }
            else if (eValue.BigInteger == valueType)
            {
                setValue((int)(curValue * StringHelper.toBigInt(value)));
            }
        }
        else if (eValue.Float == m_type)
        {
            var curValue = toFloat;

            if (eValue.Int == valueType)
            {
                setValue(curValue * StringHelper.toInt32(value));
            }
            else if (eValue.Float == valueType)
            {
                setValue(curValue * StringHelper.toSingle(value));
            }
            else if (eValue.BigInteger == valueType)
            {
                setValue((float)MathHelper.multiply(StringHelper.toBigInt(value), curValue));
            }
        }
        else if (eValue.BigInteger == m_type)
        {
            var curValue = StringHelper.toBigInt(m_value);

            if (eValue.Int == valueType)
            {
                setValue(curValue * StringHelper.toInt32(value));
            }
            else if (eValue.Float == valueType)
            {
                setValue(MathHelper.multiply(curValue, StringHelper.toSingle(value)));
            }
            else if (eValue.BigInteger == valueType)
            {
                setValue(curValue * StringHelper.toBigInt(value));
            }
        }
    }

    public void add(string value)
    {
        if (eValue.Int == m_type)
        {
            setValue(StringHelper.toInt32(m_value) + StringHelper.toInt32(value));
        }
        else if (eValue.Float == m_type)
        {
            setValue(StringHelper.toSingle(m_value) + StringHelper.toSingle(value));
        }
        else if (eValue.BigInteger == m_type)
        {
            setValue(StringHelper.toBigInt(m_value) + StringHelper.toBigInt(value));
        }
    }

    public void del(string value)
    {
        if (eValue.Int == m_type)
        {
            setValue(StringHelper.toInt32(m_value) - StringHelper.toInt32(value));
        }
        else if (eValue.Float == m_type)
        {
            setValue(StringHelper.toSingle(m_value) - StringHelper.toSingle(value));
        }
        else if (eValue.BigInteger == m_type)
        {
            setValue(StringHelper.toBigInt(m_value) - StringHelper.toBigInt(value));
        }
    }

    public string toSign()
    {
        if (eValue.Int == m_type)
        {
            var v = toInt;
            if (0 == v)
                return value;

            var sign = (0 > v) ? "-" : "+";
            return sign + Mathf.Abs(v);
        }
        else if (eValue.Float == m_type)
        {
            var v = toFloat;
            if (MathHelper.isZero(v))
                return value;

            var sign = (0.0f > v) ? "-" : "+";
            return sign + StringHelper.toFormat("{0:0.00}", Mathf.Abs(v));
        }
        else if (eValue.BigInteger == m_type)
        {
            // TODO : 2024-03-15 by pms
            var v = new BigMoney(toBigInteger);
            if (0 == v.value)
                return value;

            var sign = (0 > v.value) ? "-" : "+";

            v.value = BigInteger.Abs(v.value);

            return sign + v.toString();
        }
        else
        {
            return "";
        }
    }

    public string toString()
    {
        if (eValue.BigInteger == m_type)
        {
            var bigMoney = new BigMoney(m_value);
            return bigMoney.toString();
        }
        else
        {
            return m_value;
        }
    }

    public void applyRatio(float ratio)
    {
        if (eValue.Int == m_type)
        {
            var v = toInt * ratio;
            setValue(Mathf.RoundToInt(v));
        }
        else if (eValue.Float == m_type)
        {
            var v = toFloat * ratio;
            setValue((float)Math.Round(v, 2));
        }
        else if (eValue.BigInteger == m_type)
        {
            setValue(MathHelper.multiply(toBigInteger, ratio));
        }
    }

    private void setValue(int v)
    {
        m_value = StringHelper.toString(v);
    }

    private void setValue(float v)
    {
        m_value = StringHelper.toFormat("{0:0.00}", v);
    }

    private void setValue(BigInteger v)
    {
        m_value = StringHelper.toString(v);
    }
}
