public class GoalIdle : Goal
{
    public override string alias => "I";

    public static GoalIdle create()
    {
        GoalIdle goalIdle = new GoalIdle();
        goalIdle.type = eGoal.Idle;
        return goalIdle;
    }

    public override void initialize(AI ai, long entityUuid, ref bool isEnd)
    {
        base.initialize(ai, entityUuid, ref isEnd);
        if (isEnd)
            return;

        initUpdateTimer(AISettings.instance.idleGoal.updateTime, false);
        var entity = getEntity<Entity>(entityUuid);
        entity.setMotion(eMotion.Idle);
    }

    public override void finalize(long entityUuid)
    {
        base.finalize(entityUuid);
        var entity = getEntity<Entity>(entityUuid);

        var character = entity as Character;
        character.addGoal(GoalDefault.create());
    }

    public override void resume(AI id, long entityUuid, ref bool isEnd)
    {
        base.resume(id, entityUuid, ref isEnd);
        if (isEnd)
            return;

        initUpdateTimer(AISettings.instance.idleGoal.updateTime, false);
    }

    public override void update(eTeam team, float dt, ref bool isEnd)
    {
        base.update(team, dt, ref isEnd);
        if (isEnd)
        {
            return;
        }

        if (m_updateTimer.update(dt))
        {
            isEnd = true;
            return;
        }
    }

    public override Goal clone()
    {
        return create();
    }
}
