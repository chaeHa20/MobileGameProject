using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    [Serializable]
    public class fRange
    {
        public float start;
        public float end;
        public float length => Mathf.Abs(end - start);
        public float randValue => UnityEngine.Random.Range(start, end);

        public fRange()
        {
            start = 0.0f;
            end = 0.0f;
        }

        public fRange(float _start, float _end)
        {
            start = _start;
            end = _end;
        }

        public fRange(List<float> values)
        {
            if (Logx.isActive)
                Logx.assert(2 == values.Count, "Invalid fRange values count {0}", values.Count);

            start = values[0];
            end = values[1];
        }

        public float clamp(float v)
        {
            if (start > v)
                return start;
            else if (end < v)
                return end;
            else
                return v;
        }

        public float lerp(float t)
        {
            return Mathf.Lerp(start, end, t);
        }

        public bool isIn(float v, bool isInclusiveStart = true, bool isInclusiveEnd = true)
        {
            if (isInclusiveStart)
            {
                if (isInclusiveEnd)
                {
                    return start <= v && end >= v;
                }
                else
                {
                    return start <= v && end > v;
                }
            }
            else
            {
                if (isInclusiveEnd)
                {
                    return start < v && end >= v;
                }
                else
                {
                    return start < v && end > v;
                }
            }
        }
    }
}