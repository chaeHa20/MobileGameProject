using UnityEngine;

public class GoalFollowPlayer : GoalMove
{
    private eMotion m_nextMotion = eMotion.Move;
    public override string alias => "FP";

    public static GoalFollowPlayer create(GameObject destination)
    {
        GoalFollowPlayer goal = new GoalFollowPlayer();
        goal.type = eGoal.FollowPlayer;
        goal.movePosition = GameHelper.toVector2(destination.transform.position);
        goal.destination = destination;
        return goal;
    }

    public override void initialize(AI ai, long entityUuid, ref bool isEnd)
    {
        // setMotion(entityUuid, eMotion.Move);
        base.initialize(ai, entityUuid, ref isEnd);

        var character = getEntity<Monster>();
        character.setNavMeshPath(PlayerManager.instance.playerCharacter);
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
            if (tryLookAtTarget(character, movePosition, dt * 10.0f))
            {
                if (checkIsCanAttack(character))
                {
                    isEnd = true;
                    character.addGoal(GoalAttack.create());
                    return;
                }
                else if (null != PlayerManager.instance)
                {
                    destination = PlayerManager.instance.playerCharacterObject;
                    character.setNavMeshPath(PlayerManager.instance.playerCharacter);
                }
                else
                {
                    isEnd = true;
                    character.addGoal(GoalWaiting.create());
                    return;
                }
            }
        }
    }

    private bool checkIsCanAttack(Character character)
    {
        if (null == PlayerManager.instance)
            return false;

        var distance = Vector2.Distance(GameHelper.toVector2(character.transform.position), GameHelper.toVector2(PlayerManager.instance.playerCharacter.position));
        var minDistance = AISettings.instance.followGoal.sqrtMinFollowTargetDistance;
        if (distance < minDistance)
            return true;
        else
            return false;

    }

    public override Goal clone()
    {
        return create(destination);
    }
}
