using System.Collections;
using UnityEngine;

public class UIPushButtonAnimator : MonoBehaviour
{
    [SerializeField] float m_buttonAnimationTime = 0.5f;

    private RectTransform m_btnBg = null;
    private Vector2 m_oriRectPosition = Vector2.zero;
    private Vector2 m_oriRectSize = Vector2.zero;
    private bool m_isPlatCoroutine = false;

    private void Awake()
    {
        m_btnBg = GetComponent<RectTransform>();
        m_oriRectPosition = m_btnBg.anchoredPosition;
        m_oriRectSize = m_btnBg.sizeDelta;
    }

    public void onPushButton()
    {
        m_btnBg.anchoredPosition = new Vector2(m_oriRectPosition.x, m_oriRectPosition.y - 2.5f);
        m_btnBg.sizeDelta = new Vector2(m_oriRectSize.x, m_oriRectSize.y - 5.0f);

        if (!m_isPlatCoroutine)
            StartCoroutine(coPushButtom());
    }

    IEnumerator coPushButtom()
    {
        m_isPlatCoroutine = true;
        yield return new WaitForSeconds(m_buttonAnimationTime);

        m_btnBg.anchoredPosition = m_oriRectPosition;
        m_btnBg.sizeDelta = m_oriRectSize;
        m_isPlatCoroutine = false;
    }
}
