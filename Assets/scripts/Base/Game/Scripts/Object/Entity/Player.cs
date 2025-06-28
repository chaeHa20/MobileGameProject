using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;


public class PlayerData : AbilityEntityData
{
    public float scale = 1.0f;
    public int level;
}

public class Player : AbilityEntity
{
    private event Action m_healthUpdate;
    private event Action m_playerDead;

    private float m_moveSpeed = 0.0f;
    private Vector2 m_createPosition2 = Vector3.zero;
    private int m_level;
    private int m_hp;
    private int m_maxHp;
    private bool m_isDead = false;

    private HashSet<long> m_attackingMeUuids = new HashSet<long>();


    public PlayerModel playerModel => model as PlayerModel;
    public Transform center => playerModel.center;

    public float moveSpeed => m_moveSpeed;
    public int level => m_level;
    public int hp => m_hp;
    public bool isDead => m_isDead; 

    public Vector2 position2
    {
        get { return GameHelper.toVector2(position3); }
        set { base.position3 = GameHelper.toVector3(value, position3.y); }
    }

    public override void initialize(EntityData entityData)
    {
        var d = entityData as PlayerData;
        m_createPosition2 = new Vector2(d.position.x, d.position.z);
        m_level = d.level;

        base.initialize(entityData);

        m_moveSpeed = getAbilityValueFloat(eAbility.MoveSpeed);
        m_hp = getAbilityValueInt(eAbility.MaxHealth);
        m_maxHp = m_hp;
        m_isDead = false;
        m_attackingMeUuids.Clear();

        PlayerController.instance.setPlayer(this);
    }

    protected override void setModel(int id, EntityModel model)
    {
        base.setModel(id, model);

        setActiveModel(true);
    }

    public void addAttakingMeUuid(long attackingUuid)
    {
        if (!m_attackingMeUuids.Contains(attackingUuid))
        {
            m_attackingMeUuids.Add(attackingUuid);
        }
    }

    public void removeAttackingMeUuid(long attackingUuid)
    {
        if (m_attackingMeUuids.Contains(attackingUuid))
        {
            m_attackingMeUuids.Remove(attackingUuid);
        }
    }

    public void setDamage(in Damage damage)
    {
        if (m_isDead)
            return;

        var damageValue = damage.intValue;
        if (m_hp <= damageValue)
        {
            damageValue = m_hp;
            m_hp = 0;
            setDead();
        }
        else
        {
            m_hp -= damageValue;
        }

        updateHpCallback();
    }

    public void setRecovery(float value)
    {
        if (m_isDead)
            return;

        var result = (int)((float)m_maxHp * (value / 100.0f));
        if (m_maxHp < m_hp + result)
            result = m_maxHp - m_hp;

        m_hp += result;
        updateHpCallback();
    }

    private void updateHpCallback()
    {
        m_playerDead?.Invoke();
    }

    private void setDead()
    {
        m_isDead = true;
        m_playerDead?.Invoke();
    }

    public void setHealthUpdateCallback(Action callback)
    {
        m_healthUpdate += callback;
    }

    public void setPlayerDeadCallback(Action callback)
    {
        m_isDead = false;
        m_playerDead += callback;
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}
