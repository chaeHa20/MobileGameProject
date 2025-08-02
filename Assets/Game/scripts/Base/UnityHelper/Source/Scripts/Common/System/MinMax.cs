using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    [Serializable]
    public class fMinMax
    {
        public float min;
        public float max;

        public float length => Mathf.Abs(max - min);
        public float center => (max + min) * 0.5f;

        public fMinMax(float _min, float _max)
        {
            min = _min;
            max = _max;
        }

        public fMinMax(List<float> values)
        {
            if (0 == values.Count)
            {
                min = max = 0;
            }
            else if (1 == values.Count)
            {
                min = max = values[0];
            }
            else
            {
                min = values[0];
                max = values[1];
            }
        }

        public fMinMax()
        {
            min = 0.0f;
            max = 0.0f;
        }

        /// <summary>
        /// random value
        /// </summary>
        public float value
        {
            get
            {
                return MathHelper.isEqual(min, max) ? min : UnityEngine.Random.Range(min, max);
            }
        }

        public fMinMax copy()
        {
            return new fMinMax(min, max);
        }

        public float clamp(float v)
        {
            return Mathf.Clamp(v, min, max);
        }

        public bool isIn(float v)
        {
            if (min <= v && max >= v)
                return true;

            return false;
        }

        public bool isInMaxExclusive(float v)
        {
            if (min <= v && max > v)
                return true;

            return false;
        }

        public bool isCollision(fMinMax minMax)
        {
            if (isIn(minMax.min))
                return true;
            else if (isIn(minMax.max))
                return true;
            else if (minMax.isIn(min))
                return true;
            else if (minMax.isIn(max))
                return true;

            return false;
        }

        public void combine(fMinMax minMax)
        {
            min = Mathf.Min(min, minMax.min);
            max = Mathf.Max(max, minMax.max);
        }

        public float lerp(float t)
        {
            return Mathf.Lerp(min, max, t);
        }

        public void reduce(float v)
        {
            min += v;
            max -= v;
        }

        public void expand(float v)
        {
            min -= v;
            max += v;
        }

        public void swap()
        {
            SystemHelper.swap(ref min, ref max);
        }
    }

    [Serializable]
    public class iMinMax
    {
        public int min;
        public int max;
        public int length => max - min;

        public iMinMax(int _min, int _max)
        {
            min = _min;
            max = _max;
        }

        public iMinMax(int _minMax)
        {
            min = _minMax;
            max = _minMax;
        }

        public iMinMax(List<int> values)
        {
            if (0 == values.Count)
            {
                min = max = 0;
            }
            else if (1 == values.Count)
            {
                min = max = values[0];
            }
            else
            {
                min = values[0];
                max = values[1];
            }
        }

        public iMinMax()
        {
            min = 0;
            max = 0;
        }

        /// <summary>
        /// random value
        /// </summary>
        public int value
        {
            get
            {
                return UnityEngine.Random.Range(min, max + 1);
            }
        }

        public bool isIn(int v)
        {
            if (min <= v && max >= v)
                return true;

            return false;
        }

        public bool isInMaxExclusive(int v)
        {
            if (min <= v && max > v)
                return true;

            return false;
        }

        public bool isInMinExclusive(int v)
        {
            if (min < v && max >= v)
                return true;

            return false;
        }

        public int clamp(int v)
        {
            if (min > v)
                return min;
            else if (max < v)
                return max;

            return v;
        }

        public int lerp(float t)
        {
            int _t = (int)(t * 100.0f);
            return min + ((max - min) * _t) / 100;
        }

        public bool isSingle()
        {
            return min == max;
        }
    }

    [Serializable]
    public class gMinMax
    {
        public GameObject min;
        public GameObject max;

        public gMinMax(GameObject _min, GameObject _max)
        {
            min = _min;
            max = _max;
        }

        public gMinMax()
        {
            min = null;
            max = null;
        }

        /// <summary>
        /// random value
        /// </summary>
        public Vector3 position
        {
            get
            {
                float x = UnityEngine.Random.Range(min.transform.position.x, max.transform.position.x);
                float y = UnityEngine.Random.Range(min.transform.position.y, max.transform.position.y);
                float z = UnityEngine.Random.Range(min.transform.position.z, max.transform.position.z);

                return new Vector3(x, y, z);
            }
        }

        public Vector3 minPosition { get { return min.transform.position; } }
        public Vector3 maxPosition { get { return max.transform.position; } }
    }
}