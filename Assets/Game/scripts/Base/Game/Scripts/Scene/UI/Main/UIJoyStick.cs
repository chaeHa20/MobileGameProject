using UnityEngine;
using UnityHelper;
using UnityEngine.EventSystems;

public class UIJoyStick : UIComponent, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] RectTransform m_rect;
    [SerializeField] RectTransform m_stickRect;

    [SerializeField, Range(1f, 100f)]
    private float m_joystickDirRange = 100.0f;

    private bool m_isActiveJoyStick = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_isActiveJoyStick = true;

        var inputDir = eventData.position - m_rect.anchoredPosition;
        var clampedDir = inputDir.magnitude < m_joystickDirRange ? inputDir : inputDir.normalized * m_joystickDirRange;

        m_stickRect.anchoredPosition = clampedDir;
    }

    public void OnDrag(PointerEventData eventData)
    {

        var inputDir = eventData.position - m_rect.anchoredPosition;
        var clampedDir = inputDir.magnitude < m_joystickDirRange ? inputDir : inputDir.normalized * m_joystickDirRange;

        m_stickRect.anchoredPosition = clampedDir;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        m_isActiveJoyStick = false;
        m_stickRect.anchoredPosition = Vector2.zero;
    }

    public override void updateJoystick(float dt)
    {
        base.updateJoystick(dt);

        if (!m_isActiveJoyStick)
            return;

        var dir = toVector3(m_stickRect.anchoredPosition);
        dir /= m_joystickDirRange;
        if (null != PlayerManager.instance)
            PlayerManager.instance.updateJoystick(dt, dir);
    }

    private Vector3 toVector3(Vector2 vec2, float y = 0.0f)
    {
        return new Vector3(vec2.x, y, vec2.y);
    }
}
