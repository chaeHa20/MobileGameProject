public class GoalAttack : Goal
{
    public override string alias => "ATK";

    public static GoalAttack create()
    {
        GoalAttack goalAttack = new GoalAttack();
        goalAttack.type = eGoal.Attack;
        return goalAttack;
    }

    public override void initialize(AI ai, long entityUuid, ref bool isEnd)
    {
        base.initialize(ai, entityUuid, ref isEnd);
        if (isEnd)
            return;

        initUpdateTimer(AISettings.instance.attackGoal.updateTime, false);
        var entity = getEntity<Entity>(entityUuid);
        entity.setMotion(eMotion.Attack);
    }

    public override void finalize(long entityUuid)
    {
        base.finalize(entityUuid);
        var entity = getEntity<Entity>(entityUuid);

        if (eEntity.Monster == entity.type)
        {
            var monster = entity as Monster;
            monster.addGoal(GoalMonster.create());
        }
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
