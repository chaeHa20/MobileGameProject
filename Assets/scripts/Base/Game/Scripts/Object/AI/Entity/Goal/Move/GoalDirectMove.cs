using UnityEngine;

public class GoalDirectMove : GoalMove
{
    private eMotion m_nextMotion = eMotion.Move;
    public override string alias => "DM";

    public eMotion nextMotion { get { return m_nextMotion; } set { m_nextMotion = value; } }

    public static GoalDirectMove create(Vector2 movePosition, GameObject destination, eMotion nextMotion)
    {
        GoalDirectMove goal = new GoalDirectMove();
        goal.type = eGoal.DirectMove;
        goal.movePosition = movePosition;
        goal.nextMotion = nextMotion;
        goal.destination = destination;
        return goal;
    }

    public override void initialize(AI ai, long entityUuid, ref bool isEnd)
    {
        setMotion(entityUuid, eMotion.Move);
        base.initialize(ai, entityUuid, ref isEnd);
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
            character.setUnableAIRotate();
            character.setMotion(eMotion.Idle);

            if (tryLookAtTarget(character, movePosition, dt * 10.0f))
            {
                isEnd = true;
                character.addGoal(GoalWaiting.create());
                return;
            }
        }
    }

    public override Goal clone()
    {
        return create(movePosition, destination, nextMotion);
    }
}
