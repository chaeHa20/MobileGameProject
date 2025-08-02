using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public struct IdCount
    {
        public int id;
        public int count;

        public IdCount(int _id, int _count)
        {
            id = _id;
            count = _count;
        }

        public IdCount(List<int> datas)
        {
            id = 0;
            count = 0;

            if (1 <= datas.Count)
                id = datas[0];
            if (2 <= datas.Count)
                count = datas[1];
        }
    }
}