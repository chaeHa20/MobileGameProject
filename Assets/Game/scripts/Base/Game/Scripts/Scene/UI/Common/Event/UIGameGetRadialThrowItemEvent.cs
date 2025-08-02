using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class UIGameGetRadialThrowItemEvent : UIGameGetThrowItemEvent
{
    public Vector2 radialLength { get; set; }
    public Vector3 radialAxis { get; set; }
    public float radialSmoothTime { get; set; }
    public float radialMoveEndWait { get; set; }

    protected override BaseMove createMover()
    {
        RadialAndLinearMove mover = new RadialAndLinearMove();
        mover.speed = speed;
        mover.radialLength = UnityEngine.Random.Range(radialLength.x, radialLength.y);
        mover.radialAxis = radialAxis;
        mover.radialSmoothTime = radialSmoothTime;
        mover.radialMoveEndWait = radialMoveEndWait;

        return mover;
    }
}
