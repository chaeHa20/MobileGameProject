using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityHelper;

[Serializable]
public class LocalGameOption
{
    [SerializeField] eGraphicQuality m_graphicQuality = eGraphicQuality.Medium;
    [SerializeField] bool m_isBgmOn = true;
    [SerializeField] bool m_isSfxOn = true;
    [SerializeField] bool m_isViverationOn = true;

    public eGraphicQuality graphicQuality { get { return m_graphicQuality; } set { m_graphicQuality = value; } }
    public bool isBgmOn { get { return m_isBgmOn; } set { m_isBgmOn = value; } }
    public bool isSfxOn { get { return m_isSfxOn; } set { m_isSfxOn = value; } }
    public bool isViverationOn { get { return m_isViverationOn; } set { m_isViverationOn = value; } }
}
