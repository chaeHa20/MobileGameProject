using UnityHelper;

public class GoalWaiting : Goal
{
    public static GoalWaiting create()
    {
        var goal = new GoalWaiting();
        goal.type = eGoal.Waiting;
        return goal;
    }

    public override void initialize(AI ai, long entityUuid, ref bool isEnd)
    {
        base.initialize(ai, entityUuid, ref isEnd);

        isEnd = true;

        setWaiting(entityUuid);
    }

    private void setWaiting(long entityUuid)
    {
        var character = getEntity<Character>(entityUuid);
        character.setMotion(eMotion.Idle);
    }
}
