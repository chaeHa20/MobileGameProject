using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class LocalEntityMember : LocalMember
{
    [SerializeField] int m_health = 1;
    [SerializeField] eEntity m_entityType = eEntity.None;

    public eEntity entityType => m_entityType;
    public int health { get => m_health; set => m_health = value; }
    
    // TODO : 2024-02-05 update by pms
    public bool isDead => 0 >= m_health;
    //

    public virtual void initialize(int id, eEntity entityType)
    {
        base.initialize(id);

        m_entityType = entityType;
    }

    public void recovery(int maxHealth)
    {
        m_health = maxHealth;
    }

    public virtual long getBattlePower(LocalAbilities abilities)
    {
        return 0;
    }
    // TODO : 2024-02-06 update by pms
    public virtual void setEntityDead()
    {
        m_health = 0;        
    }
}
