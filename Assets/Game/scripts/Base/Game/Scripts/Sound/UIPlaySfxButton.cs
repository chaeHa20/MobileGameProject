using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlaySfxButton : MonoBehaviour
{
    [SerializeField] eSfx m_sfxId = eSfx.None;

    public void onPlay()
    {
        if (eSfx.None != m_sfxId)
            GameSoundHelper.getInstance().playShare((int)m_sfxId);
    }
}
