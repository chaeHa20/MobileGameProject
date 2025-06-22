using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text.RegularExpressions;
using UnityEngine;

namespace UnityHelper
{
    /// <summary>
    /// https://blog.shlife.dev/46
    /// </summary>
    [Serializable]
    public class BigMoney
    {
        [SerializeField] string m_strBigInteger = null;

        private BigInteger m_bigInteger = new BigInteger();

        private static List<string> m_units = new List<string>();
        private static Dictionary<string, BigInteger> m_unitValues = new Dictionary<string, BigInteger>();
        private static Dictionary<string, int> m_unitIndexes = new Dictionary<string, int>();
        private static int m_unitCycleCount = 26;
        private static int m_unitSize = 1000;
        private static int m_oneLowUnitSize = 100;
        private static bool m_isInitialize = false;

        public BigInteger value
        {
            get
            {
                return (BigInteger)m_bigInteger;
            }
            set
            {
                m_bigInteger = value;
            }
        }

        private static void initializeUnit()
        {
            if (m_isInitialize)
                return;

            m_isInitialize = true;

            int asciiA = 65;
            int asciiZ = 90;

            for (int n = 0; n <= m_unitCycleCount; n++)
            {
                for (int i = asciiA; i <= asciiZ; i++)
                {
                    string unit = null;
                    if (n == 0)
                    {
                        unit = ((char)i).ToString();
                    }
                    else
                    {
                        var nextChar = asciiA + n - 1;
                        var fAscii = (char)nextChar;
                        var tAscii = (char)i;
                        unit = $"{fAscii}{tAscii}";
                    }

                    addUnit(unit);
                }
            }
        }

        private static void initializeUnitKMBT()
        {
            if (m_isInitialize)
                return;

            m_isInitialize = true;

            addUnit("K");
            addUnit("M");
            addUnit("B");
            addUnit("T");

            int asciiA = 97;
            int asciiZ = 122;

            for (int n = 1; n <= m_unitCycleCount; n++)
            {
                for (int i = asciiA; i <= asciiZ; i++)
                {
                    //    string unit = null;

                    //    var tAscii = (char)i;

                    //    for (int c = 0; c <  + 2; ++c)
                    //    {
                    //        unit += $"{tAscii}";
                    //    }
                    //    addUnit(unit);
                    string unit = null;

                    var nextChar = asciiA + n - 1;
                    var fAscii = (char)nextChar;
                    var tAscii = (char)i;
                    unit = $"{fAscii}{tAscii}";

                    addUnit(unit);
                }
            }
        }

        private static void addUnit(string unit)
        {
            m_units.Add(unit);
            m_unitValues.Add(unit, BigInteger.Pow(m_unitSize, m_units.Count - 1));
            m_unitIndexes.Add(unit, m_units.Count - 1);
        }

        private static int getFirstPoint(int value)
        {
            return (value % m_unitSize) / 100;
        }

        private static int getSecondPoint(int value)
        {
            return (value % m_oneLowUnitSize) / 10;
        }

        private static (int value, int idx, int firstPoint, int secondPoint) getSize(BigInteger value)
        {
            //단위를 구하기 위한 값으로 복사
            var currentValue = value;

            if (m_unitSize > currentValue)
            {
                return ((int)value, -1, -1, -1);
            }
            else
            {
                var idx = -1;
                var lastValue = 0;
                // 현재 값이 999(unitSize) 이상인경우 나눠야함.
                while (currentValue > m_unitSize - 1)
                {
                    var predCurrentValue = currentValue / m_unitSize;
                    if (predCurrentValue <= m_unitSize - 1)
                        lastValue = (int)currentValue;
                    currentValue = predCurrentValue;
                    idx += 1;
                }

                var firstPoint = getFirstPoint(lastValue);
                var secondPoint = getSecondPoint(lastValue);
                return ((int)currentValue, idx, firstPoint, secondPoint);
            }
        }

        public static string toUnit(BigInteger value)
        {
            if (m_isInitialize == false)
            {
                //if ("ko" == System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName || "ja" == System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                     initializeUnit();
                //else
                // initializeUnitKMBT();
            }

            var sizeStruct = getSize(value);
            if (0 > sizeStruct.firstPoint)
            {
                return StringHelper.toString(value, "N0");
            }
            else
            {
                if (100 <= sizeStruct.value)
                {
                    return $"{sizeStruct.value}{m_units[sizeStruct.idx]}";
                }
                else if (10 <= sizeStruct.value)
                {
                    return $"{sizeStruct.value}.{sizeStruct.firstPoint}{m_units[sizeStruct.idx]}";
                }
                else
                {
                    return $"{sizeStruct.value}.{sizeStruct.firstPoint}{sizeStruct.secondPoint}{m_units[sizeStruct.idx]}";
                }
            }
        }

        public static BigInteger toValue(string unit)
        {
            if (m_isInitialize == false)
            {
                //if ("ko" == System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName || "ja" == System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                  // initializeUnit();
                //else
               initializeUnitKMBT();
            }

            if (string.IsNullOrEmpty(unit))
                return 0;

            var split = unit.Split('.');
            string kmbt = String.Join(" ", m_units.ToArray());
            //소수점에 관한 연산 들어감
            if (split.Length >= 2)
            {
                var value = StringHelper.toBigInt(split[0]);
                var point = StringHelper.toBigInt((Regex.Replace(split[1], "[^0-9]", "")));
                var unitStr = Regex.Replace(split[1], kmbt, "");

                if (point == 0)
                {
                    return (m_unitValues[unitStr] * value);
                }
                else
                {
                    var unitValue = m_unitValues[unitStr];
                    return ((unitValue * value) + (unitValue / 10) * point);
                }

            }
            //비소수점 연산 들어감
            else
            {
                var value = StringHelper.toBigInt((Regex.Replace(unit, "[^0-9]", "")));
                var unitStr = Regex.Replace(unit, "[^A-Z]", "");
                BigInteger result = 0;
                if (0 < unitStr.Length)
                    result = (BigInteger)m_unitValues[unitStr] * value;
                else
                    result = value;

                return result;

                /*
                if (result == 0)
                    return int.Parse((unit));
                else
                    return result;
                */
            }
        }

        public BigMoney()
        {
            m_bigInteger = 0;
        }

        public BigMoney(BigInteger bigInteger)
        {
            m_bigInteger = bigInteger;
        }

        public BigMoney(string value)
        {
#if UNITY_EDITOR
            MathHelper.checkValidBigInteger(value);
#endif
            m_bigInteger = BigMoney.toValue(value);
        }

        public BigMoney(float value, string unit)
        {
            m_bigInteger = BigMoney.toValue(string.Format("{0.00}{1}", value, unit));
        }

        public string toString()
        {
            try
            {
                return toUnit((BigInteger)m_bigInteger);
            }
            catch (System.Exception e)
            {
                if (Logx.isActive)
                    Logx.exception(e);
            }

            return "--";
        }

        public void serialize()
        {
            m_strBigInteger = StringHelper.toString(m_bigInteger);
        }

        public void deserialize()
        {
            if (BigInteger.TryParse(m_strBigInteger, out BigInteger value))
            {
                m_bigInteger = value;
            }
        }

        public static BigMoney operator +(BigMoney a) => a;
        public static BigMoney operator -(BigMoney a) => new BigMoney(-a.value);

        public static BigMoney operator +(BigMoney a, BigInteger b) => new BigMoney(a.value + b);
        public static BigMoney operator -(BigMoney a, BigInteger b) => new BigMoney(a.value - b);
        public static BigMoney operator *(BigMoney a, BigInteger b) => new BigMoney(a.value * b);
        public static BigMoney operator /(BigMoney a, BigInteger b) => new BigMoney(a.value / b);

        public static BigMoney operator +(BigMoney a, BigMoney b) => new BigMoney(a.value + b.value);
        public static BigMoney operator -(BigMoney a, BigMoney b) => new BigMoney(a.value - b.value);
        public static BigMoney operator *(BigMoney a, BigMoney b) => new BigMoney(a.value * b.value);
        public static BigMoney operator /(BigMoney a, BigMoney b) => new BigMoney(a.value / b.value);
    }
}