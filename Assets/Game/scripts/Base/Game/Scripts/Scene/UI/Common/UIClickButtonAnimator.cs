using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIClickButtonAnimator : MonoBehaviour
{
    private enum eButtonClickMotion { Click };
    private RectTransform m_btnBg = null;
    private Button m_btn = null;
    private Vector2 m_originalPosition = Vector2.one;
    private Vector2 m_originalSize = Vector2.one;

    private void Awake()
    {
        m_btnBg = GetComponent<RectTransform>();
        m_btn = GetComponent<Button>();
        m_originalPosition = m_btnBg.anchoredPosition;
        m_originalSize = m_btnBg.sizeDelta;
    }

    public void onPointerDown(BaseEventData eventData)
    {
        if (!m_btn.interactable)
            return;

        m_btnBg.anchoredPosition = new Vector2(m_originalPosition.x, m_originalPosition.y - 2.5f);
        m_btnBg.sizeDelta = new Vector2(m_originalSize.x, m_originalSize.y - 5.0f);
    }

    public void onPointerUp(BaseEventData eventData)
    {
        m_btnBg.anchoredPosition = m_originalPosition;
        m_btnBg.sizeDelta = m_originalSize;
    }
}
