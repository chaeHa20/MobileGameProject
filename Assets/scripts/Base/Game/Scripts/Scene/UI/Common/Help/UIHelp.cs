using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHelp : MonoBehaviour
{
    [SerializeField] int m_helpRowId = 0;

    public void onClick()
    {
        if (0 == m_helpRowId)
            return;
    }
}
