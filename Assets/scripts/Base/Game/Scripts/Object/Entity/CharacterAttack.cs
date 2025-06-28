using System.Collections.Generic;
using UnityEngine;
using UnityHelper;


public partial class Character : AbilityEntity
{
    private float m_attackTime = 0.0f;
    private int m_attackPower = 0;
    private float m_criticalProbability = 0.0f;
    private float m_criticalDamagePercent = 0.0f;
    private int m_attackCount = 0;
    private eAttackType m_attackType = eAttackType.None;

    protected HashSet<long> m_attackingTargetUuids = new HashSet<long>();
    protected HashSet<long> m_removeAttackingTargetUuidPools = new HashSet<long>();

    public HashSet<long> attackingTargetUuids => m_attackingTargetUuids;
    public HashSet<long> removeAttackingTargetUuidPools => m_removeAttackingTargetUuidPools;

    private void initAttackData()
    {
        m_attackPower = getAbilityValueInt(eAbility.AttackPower);
        m_attackCount = getAbilityValueInt(eAbility.AttackCount);
        m_attackTime = GameSettings.instance.defaultAttackTime / getAbilityValueFloat(eAbility.AttackPerTime);
        m_criticalProbability = getAbilityValueFloat(eAbility.CriticalProbability);
        m_criticalDamagePercent = getAbilityValueFloat(eAbility.CriticalDamagePercent);
        m_attackCount = getAbilityValueInt(eAbility.AttackCount);
    }

    protected void setAttackType(eAttackType attackType)
    {
        m_attackType = attackType;
    }

    protected virtual void attackEvent()
    {
        if (eAttackType.Melee == m_attackType)
        {
            if (eTeam._2 == team)
                attackTackleToPlayer();
            else if (eTeam._1 == team)
                attackMeleeToEnemy();
        }
        else if (eAttackType.Ranged == m_attackType)
        {
            if (eTeam._2 == team)
                bulletFireToPlayer();
            else if (eTeam._1 == team)
                bulletFireToEnemy();
        }
    }

    protected virtual void attackTackleToPlayer()
    {
        return;
    }

    protected virtual void attackMeleeToEnemy()
    {
        return;
    }

    protected virtual void bulletFireToPlayer()
    {
        return;
    }

    protected virtual void bulletFireToEnemy()
    {
        return;
    }

    protected Damage getAttackDamage()
    {
        return getDamage(m_attackCount);
    }

    private Damage getDamage(int attackCount)
    {
        if (0 > attackCount)
            attackCount = 1;

        var oriAttackPower = (float)m_attackPower / (float)attackCount;
        var attackPower = oriAttackPower;

        var isCritical = calcIsCritical();
        if (isCritical)
            attackPower += attackPower * m_criticalDamagePercent;

        var damage = new Damage(attackPower, oriAttackPower, isCritical);
        return damage;
    }

    private bool calcIsCritical()
    {
        return ProbabilityHelper.isIn(m_criticalProbability);
    }

    private void fireBullet(int customBulletId)
    {
    }

    public void setAttackingTargetUuid(List<long> targetUuids)
    {
        clearAttackingTargets();

        if (0 < targetUuids.Count)
        {
            foreach (var targetUuid in targetUuids)
            {
                var target = EntityManager.instance.getEntity<Player>(targetUuid);
                if (null == target || target.isDead)
                    continue;

                m_attackingTargetUuids.Add(targetUuid);
                if (m_removeAttackingTargetUuidPools.Contains(targetUuid))
                    m_removeAttackingTargetUuidPools.Remove(targetUuid);

                target.addAttakingMeUuid(uuid);
            }
        }
    }

    public void clearAttackingTargets()
    {
        foreach (var targetUuid in m_attackingTargetUuids)
        {
            var target = EntityManager.instance.getEntity<Player>(targetUuid);
            if (null == target || target.isDead)
                continue;

            target.removeAttackingMeUuid(uuid);
        }

        m_attackingTargetUuids.Clear();
    }
}
