using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public class StaticInstanceID
    {
        private static int ID = 0;

        public static int get()
        {
            return ++ID;
        }
    }
}