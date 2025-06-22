using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public struct IdValue
    {
        public int id;
        public int value;

        public IdValue(int _id, int _value)
        {
            id = _id;
            value = _value;
        }
    }

    public struct IdValuef
    {
        public int id;
        public float value;

        public IdValuef(int _id, float _value)
        {
            id = _id;
            value = _value;
        }
    }
}