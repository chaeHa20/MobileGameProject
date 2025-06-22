using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public struct IdValue2
    {
        public int id;
        public int value01;
        public int value02;

        public IdValue2(int _id, int _value01, int _value02)
        {
            id = _id;
            value01 = _value01;
            value02 = _value02;
        }
    }

    public struct IdValueD2
    {
        public int id;
        public decimal value01;
        public decimal value02;

        public IdValueD2(int _id, decimal _value01, decimal _value02)
        {
            id = _id;
            value01 = _value01;
            value02 = _value02;
        }
    }
}