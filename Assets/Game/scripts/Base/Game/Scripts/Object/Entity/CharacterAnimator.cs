using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class CharacterAnimator : MonoBehaviour
{
    /// <summary>
    /// m_isManualLoadFireEffect�� true�� ��쿡�� onFireEffectAnimationEvent()�� ���� ȣ���� ��� �ȴ�.
    /// </summary>
    [SerializeField] bool m_isManualLoadFireEffect = false;

    private Character m_character = null;

    public bool isEmptyCharacter => null == m_character;

    public void initialize(Character character)
    {
        m_character = character;
    }

    public void onLoadFireEffectsAnimationEvent()
    {
        if (Logx.isActive)
            Logx.assert(m_isManualLoadFireEffect, "Call onFireEffectAnimationEvent(), m_isManualLoadFireEffect must be true");

    }
}
