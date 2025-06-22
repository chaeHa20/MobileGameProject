using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class CharacterAnimator : MonoBehaviour
{
    /// <summary>
    /// m_isManualLoadFireEffect가 true일 경우에는 onFireEffectAnimationEvent()를 직접 호출해 줘야 된다.
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
