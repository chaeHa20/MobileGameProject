using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityHelper
{
    [Serializable]
    public struct Int2
    {
        public int x;
        public int y;

        public int multi { get { return x * y; } }
        public int sum { get { return x + y; } }
        public int sub { get { return x - y; } }

        public Int2(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
    }
}
