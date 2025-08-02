using UnityEngine;

public class CharacterCollider : MonoBehaviour
{
    private Character m_character = null;

    public long uuid => m_character.uuid;
    public Transform center => m_character.center;
    public Character character => m_character;

    public void initialize(Character character)
    {
        m_character = character;
    }
}
