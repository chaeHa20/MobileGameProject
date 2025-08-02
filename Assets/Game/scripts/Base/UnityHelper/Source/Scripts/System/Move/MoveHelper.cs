using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public static class MoveHelper
    {
        [Flags]
        public enum eFixAxis
        {
            None = 0,
            X = 1 << 0,
            Y = 1 << 1,
            Z = 1 << 2,
            All = int.MaxValue
        };

        public static void getMoveDir(Vector3 startPosition, Vector3 destPosition, eFixAxis fixAxis, out Vector3 moveDir, out float moveLen)
        {
            moveDir = destPosition - startPosition;
            if ((eFixAxis.X & fixAxis) != 0)
                moveDir.x = 0.0f;
            if ((eFixAxis.Y & fixAxis) != 0)
                moveDir.y = 0.0f;
            if ((eFixAxis.Z & fixAxis) != 0)
                moveDir.z = 0.0f;

            moveLen = moveDir.magnitude;
            moveDir.Normalize();
        }
    }
}