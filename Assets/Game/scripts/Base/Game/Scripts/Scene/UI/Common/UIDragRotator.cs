using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class UIDragRotator : MonoBehaviour
{
    [Serializable]
    private sealed class Option
    {
        public float panSpeedX = 80.0f;
        public float panSmoothTime = 5.0f;
    }

    [SerializeField] Option m_mouseOption = new Option();
    [SerializeField] Option m_touchOption = new Option();

    private Vector3 m_lastPanPosition = Vector3.zero;
    private float m_rotationEulerY = 0.0f;
    private Option m_option = null;
    private Camera m_camera = null;
    private int m_panFingerId = 0;
    private bool m_isBeginDrag = false;

    private void Awake()
    {
        m_camera = Camera.main;

#if UNITY_EDITOR
        m_option = m_mouseOption;
#else
        m_option = m_touchOption;
#endif
    }

    public void beginDrag()
    {
        m_isBeginDrag = true;
        m_rotationEulerY = transform.localRotation.eulerAngles.y;
        setOption();
        StartCoroutine(coSmoothPan());
    }

    public void stopDrag()
    {
        m_isBeginDrag = false;
        m_rotationEulerY = transform.localRotation.eulerAngles.y;
        StopCoroutine(coSmoothPan());
    }

    private void setOption()
    {
        if(null== m_option || null == m_camera)
        {
            m_camera = Camera.main;

#if UNITY_EDITOR
            m_option = m_mouseOption;
#else
        m_option = m_touchOption;
#endif
        }
    }

    void Update()
    {
        if (m_isBeginDrag)
        {
#if UNITY_EDITOR
            updateMouse();
#else
            updateTouch();
#endif
        }
    }

    private void updateMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_lastPanPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            setPan(Input.mousePosition);
        }
    }

    private void updateTouch()
    {
        if (1 == Input.touchCount)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                m_lastPanPosition = touch.position;
                m_panFingerId = touch.fingerId;
            }
            else if (touch.fingerId == m_panFingerId && touch.phase == TouchPhase.Moved)
            {
                setPan(touch.position);
            }
        }
    }

    private void setPan(Vector3 panPosition)
    {
        Vector3 offset = m_camera.ScreenToViewportPoint(m_lastPanPosition - panPosition);
        offset.x *= m_option.panSpeedX;

        m_rotationEulerY += offset.x;

        if (360.0f < m_rotationEulerY)
            m_rotationEulerY -= 360.0f;
        else if (-360.0f > m_rotationEulerY)
            m_rotationEulerY += 360.0f;

        m_lastPanPosition = panPosition;
    }

    IEnumerator coSmoothPan()
    {
        while (true)
        {
            var y = m_rotationEulerY;
            if (0.0f > y)
                y += 360.0f;

            var r = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0.0f, y, 0.0f), Time.deltaTime * m_option.panSmoothTime);
            transform.localRotation = r;

            yield return null;
        }
    }
}
