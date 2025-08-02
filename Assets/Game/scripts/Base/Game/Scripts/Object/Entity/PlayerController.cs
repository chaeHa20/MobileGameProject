using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityHelper;

public class PlayerController : NonMonoSingleton<PlayerController>, IDisposable
{
    [SerializeField] Player m_player;
    [SerializeField] UIPlayerHealth m_playerHealth;
    

    public void setPlayer(Player player)
    {
        m_player = player;
        if(null != player)
        {
            m_player.setHealthUpdateCallback(onHealthUpdate);
            m_player.setPlayerDeadCallback(onPlayerDeadUpdate);
        }
    }

    public void setPlayerHealthUI(UIPlayerHealth healthUi)
    {
        m_playerHealth = healthUi;
    }

    private void onHealthUpdate()
    {
        if (null == m_playerHealth || null == m_player)
            return;

        m_playerHealth.updateHealth(m_player.hp);
    }

    private void onPlayerDeadUpdate()
    {
        if (null == m_playerHealth || null == m_player)
            return;

        m_playerHealth.updateHealth(0);
    }

    public void Dispose()
    {
        Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
        }
    }
}
