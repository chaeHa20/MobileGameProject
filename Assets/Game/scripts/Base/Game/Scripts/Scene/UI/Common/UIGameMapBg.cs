using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityHelper;

public class UIGameMapBg : MonoBehaviour
{
    [SerializeField] RectTransform m_bgCenter = null;
    [SerializeField] RectTransform m_content = null;

    private Vector2 m_original = Vector2.zero;

    private void Start()
    {
        m_original = m_content.anchoredPosition;
    }

    public void onMove()
    {
        var changeValue = m_original.y - m_content.anchoredPosition.y;
        m_bgCenter.anchoredPosition = new Vector2(m_bgCenter.anchoredPosition.x, m_bgCenter.anchoredPosition.y - changeValue/5.0f);
        m_original = m_content.anchoredPosition;
    }
}
