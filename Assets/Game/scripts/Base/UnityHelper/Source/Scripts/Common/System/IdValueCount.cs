using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public struct IdValueCount
    {
        public int id;
        public int value;
        public int count;

        public IdValueCount(int _id, int _value, int _count)
        {
            id = _id;
            value = _value;
            count = _count;
        }
    }
}