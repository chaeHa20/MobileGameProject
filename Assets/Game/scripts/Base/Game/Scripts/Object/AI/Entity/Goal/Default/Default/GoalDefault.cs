public class GoalDefault : Goal
{

    public static GoalDefault create()
    {
        GoalDefault goal = new GoalDefault();
        goal.type = eGoal.None;
        return goal;
    }

    public override void initialize(AI ai, long entityUuid, ref bool isEnd)
    {
        base.initialize(ai, entityUuid, ref isEnd);
        if (isEnd)
            return;

        initDatas();
    }

    public override void resume(AI id, long entityUuid, ref bool isEnd)
    {
        base.resume(id, entityUuid, ref isEnd);
        if (isEnd)
            return;

        // initDatas();
    }

    public override void finalize(long entityUuid)
    {
        base.finalize(entityUuid);

        var entity = getEntity<Entity>(entityUuid);

        var character = entity as Character;
        character.addGoal(GoalIdle.create());
    }

    private void initDatas()
    {
        initUpdateTimer(AISettings.instance.defaultGoal.updateTime, false);
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
            setDefaultGoal(ref isEnd);
        }
    }

    protected virtual void setDefaultGoal(ref bool isEnd)
    {
        isEnd = true;
    }

    protected void addNoneGoal()
    {
        var goal = new Goal();
        goal.type = eGoal.None;
        addGoal(goal);
    }
}
