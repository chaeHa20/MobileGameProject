using UnityHelper;

public class GoalMonster : GoalDefault
{
    public static GoalMonster create(bool isInit)
    {
        var goal = new GoalMonster();
        goal.type = eGoal.Monster;
        return goal;
    }

    protected override void setDefaultGoal(ref bool isEnd)
    {
        //base.setDefaultGoal(ref isEnd);

        var monster = getEntity<Monster>();
        if (null != monster && null!= PlayerManager.instance)
            monster.addGoal(GoalFollowPlayer.create(PlayerManager.instance.playerCharacterObject));
    }
}
