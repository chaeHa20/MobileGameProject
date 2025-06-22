using Pathfinding;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;


public class CharacterData : AbilityEntityData
{
    public float scale = 1.0f;
    public int level;// TODO : 2024-05-29 by pms
}

public class Character : AbilityEntity
{
    [SerializeField] AIDestinationSetter m_destSetter = null;
    [SerializeField] AIPath m_aiPath = null;

    private float m_moveSpeed = 0.0f;
    private AI m_ai = null;
    private Vector2 m_movingVelocity = Vector2.zero;
    private Vector2 m_createPosition2 = Vector3.zero;
    private Vector2 m_targetPosition2 = Vector3.zero;
    private int m_level = 1;
    
    public CharacterModel characterModel => model as CharacterModel;

    protected AIPath aiPath => m_aiPath;

    public float moveSpeed { get => m_moveSpeed; protected set => m_moveSpeed = value; }
    public Vector2 movingVelocity { get => m_movingVelocity; set => m_movingVelocity = value; }
    public bool isEnableMove => 0.0f < m_moveSpeed;
    public AI ai => m_ai;
    public Transform center => characterModel.center;
    public int level => m_level;

    public Vector2 position2
    {
        get { return GameHelper.toVector2(position3); }
        set { base.position3 = GameHelper.toVector3(value, position3.y); }
    }

    public Vector2 targetPosition2
    {
        get { return m_targetPosition2; }
        set { m_targetPosition2 = value; }
    }

    private void Awake()
    {
    }

    public override void initialize(EntityData entityData)
    {
        var d = entityData as CharacterData;
        // base.initialize 호출 전에 설정돼야 된다.
        setModelParentLocalScale(d.scale);

        m_createPosition2 = new Vector2(d.position.x, d.position.z);
        m_level = d.level;
        m_movingVelocity = Vector2.zero;
        m_destSetter.target = null;
        m_aiPath.canMove = false;

        base.initialize(entityData);

        initAI();
    }

    public void setLevelHud(int level)
    {
        m_level = level;

        if (null != characterModel)
        {
            // characterModel.setLevelHud(level);
        }
    }

    public override void setAbilities(LocalAbilities abilities)
    {
        base.setAbilities(abilities);
    }

    public virtual void updateCharacterMoveSpeed()
    {
        var multiplier = getAttachedEquipmentMoveAbility();
        var multiplierLimit = GameSettings.instance.work.maxSpeedMultiplier * 0.01f;
        if (multiplierLimit < multiplier)
            multiplier = multiplierLimit;

        var speed = moveSpeed * multiplier;

        if (speed < GameSettings.instance.work.maxSpeed)
            m_aiPath.maxSpeed = speed;
        else
            m_aiPath.maxSpeed = GameSettings.instance.work.maxSpeed;
    }

    protected float getAttachedEquipmentMoveAbility()
    {
        var weight = 1.0f;
        var playerWeight = 1.0f;
        var req = new Req_GetPlayerInfo();

        GameLocalDataHelper.instance.request<Res_GetPlayerInfo>(req, (res) =>
        {
            if (res.isSuccess)
            {
                foreach (var item in res.playerData.attachedItems)
                {
                    if (null == item || item.id <= 0)
                        continue;
                }
            }
        });

        if (eEntity.Player == type)
            return weight * playerWeight;
        else
            return weight;
    }

    protected virtual AI initAI()
    {
        m_ai = new AI();
        return m_ai;
    }

    protected override void setModel(int id, EntityModel model)
    {
        base.setModel(id, model);

        setActiveModel(true);
    }

    public void addGoal(Goal goal, bool isImmediately = false)
    {
        if (null != m_ai)
            m_ai.addGoal(goal, isImmediately);
    }

    public virtual bool isEnableKnockBack()
    {
        return false;
    }

    public override void startBattle()
    {
        base.startBattle();
    }

    public override void setModelLayer()
    {
        if (null != characterModel)
            characterModel.setModelLayer();
    }

    public override void endBattle()
    {
        base.endBattle();
    }

    public override void update(float dt)
    {
        base.update(dt);

        if (null != m_ai)
            m_ai.update(team, dt);
    }

    public void setTargetPoint(Transform targetPoint)
    {
        if (null != m_destSetter && null != targetPoint.gameObject)
            m_destSetter.target = targetPoint;
        if (!m_aiPath.canMove)
            m_aiPath.canMove = true;
    }

    protected virtual void revive()
    {
        if (GameSceneHelper.instance.isCurrentScene(eScene.Main.ToString()))
            ai.clearGoal();
    }

    public void setUnableAIRotate()
    {
        if (m_aiPath != null)
            m_aiPath.enableRotation = false;
    }

    public void setAbleAIRotate()
    {
        if (m_aiPath != null)
            m_aiPath.enableRotation = true;
    }

    public override void Dispose()
    {
        base.Dispose();
    }

#if UNITY_EDITOR
    protected override void onDrawGizmos()
    {
        base.onDrawGizmos();

        var gizmo = DebugSettings.instance.gizmo;

        var p = transform.position;

        if (gizmo.isDrawGoalAlas)
        {
            if (null != m_ai)
            {
                p.y += 0.1f;
                EditorHelper.drawHandleLabel(p, m_ai.getGoalAlias());
            }
        }

        if (null != m_ai)
            m_ai.onDrawGizmos();
    }
#endif
}
