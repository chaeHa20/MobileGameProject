using System.Collections.Generic;

namespace UnityHelper
{
    public struct IdPackage
    {
        public int mainType;
        public int subType;
        public int id;
        public int count;

        public IdPackage(int _main, int _sub, int _id, int _count)
        {
            mainType = _main;
            subType = _sub;
            id = _id;
            count = _count;
        }

        public IdPackage(List<int> datas)
        {
            mainType = 0;
            subType = 0;
            id = 0;
            count = 0;

            if (1 <= datas.Count)
                mainType = datas[0];
            if (2 <= datas.Count)
                subType = datas[1];
            if (3 <= datas.Count)
                id = datas[2];
            if (4 <= datas.Count)
                count = datas[3];
        }
    }
}