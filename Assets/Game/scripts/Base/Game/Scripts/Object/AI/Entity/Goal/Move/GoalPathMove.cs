using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPathMove : GoalMove 
{
    public override string alias => "PM";

    public static GoalPathMove create(Vector2 movePosition)
    {
        GoalPathMove goal = new GoalPathMove();
        goal.type = eGoal.PathMove;
        goal.movePosition = movePosition;
        return goal;
    }

    public override void initialize(AI ai, long entityUuid, ref bool isEnd)
    {
        base.initialize(ai, entityUuid, ref isEnd);
        if (isEnd)
            return;

        search(entityUuid, destination.transform);

        initDatas(entityUuid);
    }

    private void initDatas(long entityUuid)
    {

    }

    public override void resume(AI id, long entityUuid, ref bool isEnd)
    {
        base.resume(id, entityUuid, ref isEnd);

        search(entityUuid, destination.transform);
    }

    public override void finalize(long entityUuid)
    {
        base.finalize(entityUuid);
    }

    protected override void updateMove(Character character, float dt, ref bool isEnd, out bool isArrive)
    {
        base.updateMove(character, dt, ref isEnd, out isArrive);

        if (isEnd)
        {
            return;
        }

        isArrive = checkIsArrive(character);

        if (isArrive)
        {
            isEnd = true;
            return;
        }
    }

    //private bool checkIsArrive()
    //{
    //    var character = getEntity<Character>();
    //    var curPosition = character.position3;
    //    var distance = curPosition - GameHelper.toVector3(movePosition);
    //    if (distance.magnitude <= 0.1f)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    public override Goal clone()
    {
        return create(movePosition);
    }

#if UNITY_EDITOR
    public override void onDrawGizmos()
    {
        base.onDrawGizmos();
    }
#endif
}
