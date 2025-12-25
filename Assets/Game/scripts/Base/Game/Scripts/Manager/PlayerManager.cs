using UnityEngine;
using UnityHelper;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    [SerializeField] Transform m_player;
    [SerializeField] GameObject m_characterParent; 
    [SerializeField] float m_rotationPerSecond = 90.0f;
    [SerializeField] float m_speedWeight = 1.0f;
    [SerializeField] Rigidbody m_rigidBody;

    private Player m_playerCharacter;
    private Vector3 rot = Vector3.one;

    public Transform playerCharacter => m_playerCharacter.gameObject.transform;
    public GameObject playerCharacterObject => m_playerCharacter.gameObject;

    public void loadPlayerCharacter()
    {
        var req = new Req_GetPlayerInfo();
        GameLocalDataHelper.instance.request<Res_GetPlayerInfo>(req, (res) =>
        {
            if(res.isSuccess)
            {
                loadPlayerCharacter(res.playerData.level);
            }
        });
    }

    private void loadPlayerCharacter(int playerLevel)
    {
        var data = new PlayerData
        {
            parent = m_characterParent,
            id = (int)ePlayer.PlayerId,
            localUuid = 0,
            team = eTeam._1,
            type = eEntity.Player,
            resourceId = (int)eResource.PlayerCharacter,
            rotation = Quaternion.identity,
        };

        EntityManager.instance.createEntity<Player>(data, (player) =>
        {
            m_playerCharacter = player;
            m_rigidBody.isKinematic = false;
        });
    }

    public void updateJoystick(float dt, Vector3 dir)
    {
        var targetRotation = Quaternion.LookRotation(dir.normalized);

        m_player.position += dir * dt * m_speedWeight;
        m_playerCharacter.transform.rotation = Quaternion.Lerp(m_playerCharacter.transform.rotation, targetRotation, 1.0f);
    }

#if UNITY_EDITOR
    public void updateRotation(float dt, Vector3 rot)
    {
        var angles = m_playerCharacter.transform.rotation.eulerAngles + rot.normalized * m_rotationPerSecond;
        var targetRotation = Quaternion.Euler(angles);

        m_playerCharacter.transform.rotation = Quaternion.Lerp(m_playerCharacter.transform.rotation, targetRotation, dt);

#if UNITY_EDITOR
        if(Logx.isActive)
        Logx.trace($"Character Rotation : {m_playerCharacter.transform.rotation}");
#endif
    }

    public void updateMove(float dt, float weight)
    {
        m_player.position += m_playerCharacter.forward.normalized * weight * dt;
    }
#endif
}
