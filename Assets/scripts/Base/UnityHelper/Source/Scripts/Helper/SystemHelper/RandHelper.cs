using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public static class RandHelper
    {
        private static System.Random random = null;

        /// <summary>
        /// Thread에서 호출 하면 안된다.
        /// </summary>
        public static Vector2 getPosition(Vector2 pos, float radius)
        {
            var rand_angle = UnityEngine.Random.Range(0.0f, 360.0f);
            var rand_radius = UnityEngine.Random.Range(0.0f, radius);
            var rot = Quaternion.AngleAxis(rand_angle, Vector3.forward);
            var rp = rot * Vector2.right * rand_radius;
            return new Vector2(pos.x + rp.x, pos.y + rp.y);
        }

        public static Vector2 getSystemPosition(Vector2 pos, float radius)
        {
            var rand_angle = systemRange2(0.0f, 360.0f);
            var rand_radius = systemRange2(0.0f, radius);
            var rot = Quaternion.AngleAxis(rand_angle, Vector3.forward);
            var rp = rot * Vector2.right * rand_radius;
            return new Vector2(pos.x + rp.x, pos.y + rp.y);
        }

        private static System.Random getSystemRandom()
        {
            if (null == random)
            {
                random = new System.Random(unchecked((int)DateTime.Now.Ticks) + 1);
            }

            return random;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="min">The inclusive lower bound of the random number returned.</param>
        /// <param name="max">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
        /// <returns></returns>
        public static int systemRange(int min, int max)
        {
            return getSystemRandom().Next(min, max);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="max">The exclusive upper bound of the random number to be generated.maxValue must be greater than or equal to zero.</param>
        /// <returns></returns>
        public static int systemRange(int max)
        {
            return getSystemRandom().Next(max);
        }

        /// <summary>
        ///  Returns a nonnegative random number.
        /// </summary>
        /// <returns></returns>
        public static int systemValue()
        {
            return getSystemRandom().Next();
        }

        public static float systemRange(float min, float max)
        {
            int _min = (int)(min * 1000000.0f);
            int _max = (int)(max * 1000000.0f);
            int _value = systemRange(_min, _max);

            return (float)(_value / 1000000.0f);
        }

        public static float systemRange2(float min, float max)
        {
            var sample = getSystemRandom().NextDouble();
            return (float)(sample * (max - min) + min);
        }
    }
}