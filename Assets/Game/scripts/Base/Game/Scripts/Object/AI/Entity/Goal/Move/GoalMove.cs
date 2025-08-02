using UnityEngine;
using UnityHelper;

public class GoalMove : Goal
{

    private float m_lookAtTargetSpeed = 10.0f;
    private Vector2 m_movePosition = Vector2.zero;
    private GameObject m_destination = null;
    // protected NavMeshPath m_navMeshPath = null;
    /// <summary>
    /// 같은 위치에 몇초 동안 머물러 있을 경우에 이동을 못하는걸로 간주하고 이동할 위치를 다시 찾는다.
    /// </summary>
    private UpdateTimer m_cantMoveWaitTimer = new UpdateTimer();
    private Vector2 m_oldPosition = Vector2.zero;
    private float m_moveSpeed = 1.0f;

    public Vector2 movePosition { protected get { return m_movePosition; } set { m_movePosition = value; } }
    public override string alias => "M";
    protected GameObject destination { get { return m_destination; } set { m_destination = value; } }

    public override void initialize(AI ai, long entityUuid, ref bool isEnd)
    {
        base.initialize(ai, entityUuid, ref isEnd);

        var character = getEntity<Character>(entityUuid);

        m_moveSpeed = 1.0f;// character.getMoveSpeed();
        initCantMove(entityUuid);

        setLookAtTargetSpeed(character);
    }

    private void setLookAtTargetSpeed(Character character)
    {
        var moveDir = GameHelper.toVector3(m_movePosition - character.position2);
        var angle = Vector3.Angle(moveDir.normalized, character.forward);
        var angle_t = angle / 180.0f;
        m_lookAtTargetSpeed = AISettings.instance.moveGoal.lookAtTargetSpeedForMove.lerp(angle_t);
    }

    private void initCantMove(long entityUuid)
    {
        m_cantMoveWaitTimer.initialize(AISettings.instance.moveGoal.cantMoveWaitTimer, false);

        Character character = getEntity<Character>(entityUuid);
        m_oldPosition = character.position2;
    }

    public override void resume(AI id, long entityUuid, ref bool isEnd)
    {
        base.resume(id, entityUuid, ref isEnd);

        initCantMove(entityUuid);
    }

    public override void finalize(long entityUuid)
    {
        base.finalize(entityUuid);

        var character = getEntity<Character>(entityUuid);
    }

    protected void setMotion(long entityUuid, eMotion motion)
    {
        Character character = getEntity<Character>(entityUuid);
        // 모션이 똑같이 보이는걸 방지
        character.setMotion(motion);

        if (eMotion.Move == character.model.motion )
        {
            character.setModelLayer();
            character.setAbleAIRotate();
            character.updateCharacterMoveSpeed();
            character.setNavMeshPath(destination.transform);
            character.characterModel.setAnimSpeed();
        }
    }

    protected void lerpModelDir(Character character, Vector2 targetPosition, float dt)
    {
        var moveDir = targetPosition - character.position2;
        character.lerpModelDir(moveDir.normalized, dt * m_lookAtTargetSpeed);
    }

    protected void lerpModelMove(Character character, Vector2 targetPosition, float dt)
    {
        character.position2 = Vector2.Lerp(character.position2, targetPosition, dt);
    }

    protected virtual void search(long entityUuid, Transform target)
    {
        Character character = getEntity<Character>(entityUuid);

        character.setNavMeshPath(target);
    }


    public override void update(eTeam team, float dt, ref bool isEnd)
    {
        base.update(team, dt, ref isEnd);

        var character = getEntity<Character>();

        updateMove(character, dt, ref isEnd, out bool isArrive);

        if (!isEnd)
            checkCantMove(dt, ref isEnd);
    }

    /// <summary>
    /// 같은 장소에 계속 머무르고 있는지 체크
    /// </summary>
    private void checkCantMove(float dt, ref bool isEnd)
    {
        Character character = getEntity<Character>();

        float sqrtDistance = Vector2.SqrMagnitude(character.position2 - m_oldPosition);
        if (0.1f > sqrtDistance)
        {
            if (m_cantMoveWaitTimer.update(dt))
            {
                //if (Logx.isActive)
                //    Logx.warn("Can't Move {0}", character.name);

                // 못 가면 초기화 해서 새로운 적을 찾도록 하자
                //resolveCantMove(character);
                isEnd = true;
            }
        }
        else
        {
            m_oldPosition = character.position2;
            m_cantMoveWaitTimer.initialize(AISettings.instance.moveGoal.cantMoveWaitTimer, false);
        }
    }

    protected virtual void resolveCantMove(Character character)
    {
        var moveDir2 = movePosition - character.position2;
        moveDir2.Normalize();

        var moveDir3 = GameHelper.toVector3(moveDir2);

        // 왼쪽으로 비껴가게 하자.
        var leftDir = Vector3.Cross(moveDir3, Vector3.up);

        var cp = moveDir3 * character.radius * 2.0f;
        var lp = leftDir * character.radius * 2.0f;

        var movePosition3 = character.position3 + cp + lp;
        var movePosition2 = new Vector2(movePosition3.x, movePosition3.z);
        addGoal(GoalPathMove.create(movePosition2));
    }

    protected virtual void updateMove(Character character, float dt, ref bool isEnd, out bool isArrive)
    {
        isArrive = false;
    }

    protected virtual bool tryLookAtTarget(Character character, Vector2 target, float dt)
    {
        var dirToTarget = MathHelper.getDir(character.position3, GameHelper.toVector3(target, character.position3.y));
        return _tryLookAtTarget(character, dirToTarget, dt);
    }

    protected bool _tryLookAtTarget(Character character, in Vector3 dirToTarget, float dt)
    {
        var dot = Vector3.Dot(dirToTarget, character.forward);

        character.forward = Vector3.Lerp(character.forward, dirToTarget, dt * m_lookAtTargetSpeed);

        if (0.99f > dot)
            return false;

        return true;
    }

    protected bool checkIsArrive(Character character)
    {
        var curPosition = character.position2;
        var distance = curPosition - GameHelper.toVector2(destination.transform.position);

        if (distance.magnitude < character.radius)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override Goal clone()
    {
        var goal = new GoalMove
        {
            movePosition = movePosition,
            destination = destination,
        };

        return goal;
    }

#if UNITY_EDITOR
    public override void onDrawGizmos()
    {
        base.onDrawGizmos();

        var character = getEntity<Character>();

        if (character != null)
            Debug.DrawLine(character.position3, GameHelper.toVector3(movePosition), Color.yellow);
    }


#endif
}
