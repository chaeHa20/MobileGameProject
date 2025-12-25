using UnityEngine;
using System;
using UnityHelper;

public class MonsterData : CharacterData
{ }

public class Monster : Character
{

    protected override AI initAI()
    {
        var ai = base.initAI();
        ai.initialize(uuid, GoalMonster.create(true));
        return ai;
    }

    protected override void loadModel(int id, Action callback)
    {
        callback?.Invoke();
    }
}
