using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class UIGameGetBasicThrowItemEvent : UIGameGetThrowItemEvent
{
    public int randAngle { get; set; }
    public float centerPositionRate { get; set; }

    protected override BaseMove createMover()
    {
        DirectlyMove mover = new DirectlyMove();
        mover.speed = speed;
        //mover.randAngle = randAngle;
        mover.centerPositionRate = centerPositionRate;

        return mover;
    }
}
