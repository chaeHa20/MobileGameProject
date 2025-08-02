using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace UnityHelper
{
    public static class MathHelper
    {
        /// <summary>
        /// http://mbseo.wo.tc/166
        /// </summary>
        public static UnityEngine.Vector3 bezierCurve(float t, UnityEngine.Vector3 p0, UnityEngine.Vector3 p1, UnityEngine.Vector3 p2)
        {
            var pa = UnityEngine.Vector3.Lerp(p0, p1, t);
            var pb = UnityEngine.Vector3.Lerp(p1, p2, t);
            return UnityEngine.Vector3.Lerp(pa, pb, t);
        }

        public static bool isEqual(float a, float b)
        {
            // float.Epsilon은 너무 값이 작아서 체그가 잘 안된다.
            float epsilon = 0.000001f;
            if (Mathf.Abs(b - a) <= epsilon)
                return true;

            return false;
        }

        public static bool isZero(float v)
        {
            return isEqual(v, 0.0f);
        }

        public static bool isZero(UnityEngine.Vector3 v)
        {
            if (!isZero(v.x) || !isZero(v.y) || !isZero(v.z))
                return false;

            return true;
        }

        public static float calcV(float t, float s)
        {
            return t / s;
        }

        public static float clacT(float v, float s)
        {
            return s / v;
        }

        public static float calcS(float t, float v)
        {
            return t * v;
        }

        public static UnityEngine.Vector3 getDir(UnityEngine.Vector3 start, UnityEngine.Vector3 end)
        {
            var dir = end - start;
            dir.Normalize();
            return dir;
        }

        public static UnityEngine.Vector3 getDir(GameObject start, GameObject end)
        {
            return getDir(start.transform.position, end.transform.position);
        }

        /// <summary>
        /// -180 ~ 180
        /// https://forum.unity.com/threads/solved-how-to-get-rotation-value-that-is-in-the-inspector.460310/page-2
        /// </summary>
        public static float wrapAngle(float angle)
        {
            angle %= 360;
            if (angle > 180)
                return angle - 360;

            return angle;
        }

        /// <summary>
        /// 0 ~ 360.0f
        /// https://forum.unity.com/threads/solved-how-to-get-rotation-value-that-is-in-the-inspector.460310/page-2
        /// </summary>
        public static float unwrapAngle(float angle)
        {
            if (angle >= 0)
                return angle;

            angle = -angle % 360;

            return 360 - angle;
        }

        /// <summary>
        /// 정규 분포 함수
        /// </summary>
        public static float getNormalDistribution(float sigma, float x, float mu)
        {
            return Mathf.Exp(-0.5f * Mathf.Pow((x - mu) / sigma, 2.0f)) / (sigma * Mathf.Sqrt(2.0f * Mathf.PI));
        }

        /// <summary>
        /// 소수점 3자리까지 절사 된다.
        /// </summary>
        public static double cutOff(float value, int digits)
        {
            switch(digits)
            {
                case 1: return System.Math.Truncate(value * 10.0f) * 0.1f;
                case 2: return System.Math.Truncate(value * 100.0f) * 0.01f;
                case 3: return System.Math.Truncate(value * 1000.0f) * 0.001f;
                default: return value;
            }
        }

        public static float degreeToRadian(float degree)
        {
            return (Mathf.PI / 180.0f) * degree;
        }

        public static float radianToDgree(float radian)
        {
            return (180.0f / Mathf.PI) * radian;
        }

        public static UnityEngine.Quaternion getTargetRotation(UnityEngine.Vector2 targetDir)
        {
            float angle = UnityEngine.Vector3.Angle(UnityEngine.Vector2.right, targetDir);
            if (0.0f > targetDir.y)
                angle *= -1.0f;
            return UnityEngine.Quaternion.AngleAxis(angle, UnityEngine.Vector3.forward);
        }

        /// <summary>
        /// GPT 선생님에게 물어봄
        /// </summary>
        public static BigInteger lerp(BigInteger a, BigInteger b, double t)
        {
            BigInteger diff = b - a;
            BigInteger weight = (BigInteger)(t * 1000); // 가중치를 1000배 한다.
            var result = BigInteger.DivRem(diff * weight, 1000, out BigInteger remainder);
            return a + result;
        }

        /// <summary>
        /// 소수점은 표현이 안된다.
        /// </summary>
        public static BigInteger divide(BigInteger dividend, double divisor)
        {
            BigInteger weight = (BigInteger)(divisor * 1000); // 가중치를 1000배 한다.
            var result = BigInteger.DivRem(dividend * 1000, weight, out BigInteger remainder);

            return result;
        }

        public static double divide(BigInteger dividend, BigInteger divisor)
        {
            var result = (double)BigInteger.Divide(dividend * 1000, divisor) /1000;

            return result;
        }

        public static BigInteger divide2(BigInteger dividend, BigInteger divisor)
        {
            var result = BigInteger.Divide(dividend * 1000, divisor) / 1000;

            return result;
        }

        public static BigInteger multiply(BigInteger a, double t)
        {
            BigInteger weight = (BigInteger)(t * 1000);
            var result = (a * weight) / 1000;

            return result;
        }

        public static void checkValidBigInteger(string v)
        {
            if (!BigInteger.TryParse(v, out BigInteger b))
            {
                if (Logx.isActive)
                    Logx.error("Failed Parse BigInteger {0}", v);
            }
        }
    }
}