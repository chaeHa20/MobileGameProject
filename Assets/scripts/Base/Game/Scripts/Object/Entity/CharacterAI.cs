using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityHelper;

public partial class Character : AbilityEntity
{
    private float m_moveSpeed = 0.0f;
    private Vector2 m_targetPosition2 = Vector3.zero;
    private NavMeshAgent m_navMeshAgent = null;
    private AI m_ai = null;

    public float moveSpeed { get => m_moveSpeed; protected set => m_moveSpeed = value; }
    public bool isEnableMove => 0.0f < m_moveSpeed;
    public AI ai => m_ai;

    public Vector2 targetPosition2
    {
        get { return m_targetPosition2; }
        set { m_targetPosition2 = value; }
    }

    public virtual void updateCharacterMoveSpeed()
    {
        var multiplier = getAttachedEquipmentMoveAbility();
        var multiplierLimit = GameSettings.instance.work.maxSpeedMultiplier * 0.01f;
        if (multiplierLimit < multiplier)
            multiplier = multiplierLimit;

        var speed = moveSpeed * multiplier;

        if (speed < GameSettings.instance.work.maxSpeed)
            m_navMeshAgent.speed= speed;
        else
            m_navMeshAgent.speed= GameSettings.instance.work.maxSpeed;
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

    private void initNavMeshAgent(Vector3 position)
    {
        if (null != m_navMeshAgent)
        {
            setEnableNavMesh(true);
            /// <summary>
            /// 출처 : https://www.reddit.com/r/Unity3D/comments/9cpim6/objects_not_spawning_properly_with_navmeshagent/
            /// </summary>
            m_navMeshAgent.Warp(position); // TODO : 2024-04-05 by pms            
            stopNavMeshPath(true);
        }
    }
    private void setEnableNavMesh(bool isEnable)
    {
        if (null != m_navMeshAgent)
        {
            m_navMeshAgent.enabled = isEnable;
        }
    }

    public bool setNavMeshPath(Transform targetPoint)
    {
        if (null == m_navMeshAgent)
            return false;

        NavMeshPath path = new NavMeshPath();
        
        if (!m_navMeshAgent.CalculatePath(targetPoint.position, path))
        {
            if (Logx.isActive)
                Logx.trace("Failed calculatePath");
        }

        if (NavMeshPathStatus.PathComplete == path.status)
        {
            stopNavMeshPath(false);
            m_navMeshAgent.SetPath(path);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void stopNavMeshPath(bool isStop)
    {
        if (null == m_navMeshAgent)
            return;

        if (gameObject.activeInHierarchy)
            m_navMeshAgent.isStopped = isStop;

        // 다른 agent의 push 방지(https://forum.unity.com/threads/navmeshagent-how-to-disable-push-behaviour.239443/)
        if (characterModel != null)
        {
            m_navMeshAgent.radius = characterModel.radius;
        }
    }

    protected virtual void revive()
    {
        if (GameSceneHelper.instance.isCurrentScene(eScene.Main.ToString()))
            ai.clearGoal();
    }

    public void setUnableAIRotate()
    {
        if (m_navMeshAgent != null)
            m_navMeshAgent.isStopped = true;
    }

    public void setAbleAIRotate()
    {
        if (m_navMeshAgent != null)
            m_navMeshAgent.isStopped = false;
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
