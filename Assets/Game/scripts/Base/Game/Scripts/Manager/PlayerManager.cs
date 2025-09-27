using UnityEngine;
using UnityHelper;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    [SerializeField] Transform m_player;
    [SerializeField] Transform m_playerCharacter;
    [SerializeField] float m_rotationPerSecond = 90.0f;

    private Vector3 rot = Vector3.one;

    // 회전 이동 구현
    public void updateRotation(float dt, Vector3 rot)
    {
        var angles = m_playerCharacter.rotation.eulerAngles + rot.normalized * m_rotationPerSecond;
        var targetRotation = Quaternion.Euler(angles);

        m_playerCharacter.rotation = Quaternion.Lerp(m_playerCharacter.rotation, targetRotation, dt);

        Debug.Log($"Character Rotation : {m_playerCharacter.rotation}");
    }

    // 조이스틱 이동 구현
    public void updateDirection(float dt, Vector3 dir)
    {
        var targetRotation = Quaternion.LookRotation(dir.normalized);

        m_player.position += dir.normalized * dt;
        m_playerCharacter.rotation = Quaternion.Lerp(m_playerCharacter.rotation, targetRotation, dt);
    }

    public void updateMove(float dt, float weight)
    {
        m_player.position += m_playerCharacter.forward.normalized * weight * dt;
    }
}
