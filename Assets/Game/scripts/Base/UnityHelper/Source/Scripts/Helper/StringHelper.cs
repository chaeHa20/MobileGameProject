using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using System.Numerics;
using System;

namespace UnityHelper
{
    public partial class StringHelper
    {
        public static int toInt32(string value)
        {
            if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int result))
            {
                return result;
            }
            else
            {
                if (Logx.isActive)
                    Logx.error("int.TryParse error {0}", value);

                return 0;
            }
        }

        public static uint toUInt32(string value)
        {
            if (uint.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out uint result))
            {
                return result;
            }
            else
            {
                if (Logx.isActive)
                    Logx.error("uint.TryParse error {0}", value);

                return 0;
            }
        }

        public static long toInt64(string value)
        {
            if (long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out long result))
            {
                return result;
            }
            else
            {
                if (Logx.isActive)
                    Logx.error("long.TryParse error {0}", value);

                return 0;
            }
        }

        public static float toSingle(string value)
        {
            if (float.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out float result))
            {
                return result;
            }
            else
            {
                if (Logx.isActive)
                    Logx.error("float.TryParse error {0}", value);

                return 0.0f;
            }
        }

        public static double toDouble(string value)
        {
            if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out double result))
            {
                return result;
            }
            else
            {
                if (Logx.isActive)
                    Logx.error("float.TryParse error {0}", value);

                return 0.0f;
            }
        }

        public static BigInteger toBigInt(string value)
        {
            if (BigInteger.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out BigInteger result))
            {
                return result;
            }
            else
            {
                if (Logx.isActive)
                    Logx.error("BigInteger.TryParse error {0}", value);

                return 0;
            }
        }

        public static string toString(int value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public static string toString(int value, string format)
        {
            return value.ToString(format, CultureInfo.InvariantCulture);
        }

        public static string toString(long value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public static string toString(long value, string format)
        {
            return value.ToString(format, CultureInfo.InvariantCulture);
        }

        public static string toString(float value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public static string toString(float value, string format)
        {
            return value.ToString(format, CultureInfo.InvariantCulture);
        }

        public static string toString(BigInteger value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public static string toString(BigInteger value, string format)
        {
            return value.ToString(format, CultureInfo.InvariantCulture);
        }

        public static string toFormat(string format, params object[] args)
        {
            return string.Format(CultureInfo.InvariantCulture, format, args);
        }
    }
}