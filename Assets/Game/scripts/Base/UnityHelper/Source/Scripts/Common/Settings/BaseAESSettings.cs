using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public class BaseAESSettings : ScriptableObject
    {
        public Crypto table = new Crypto();
        public Crypto localData = new Crypto();
    }
}   