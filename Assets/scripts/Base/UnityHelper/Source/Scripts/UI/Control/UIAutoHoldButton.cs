using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityHelper
{
    public class UIAutoHoldButton : MonoBehaviour
    {
        [Serializable] class ButtonClickedEvent : UnityEvent<Action<bool>> { }

        [SerializeField] ButtonClickedEvent m_clickedEvent = new ButtonClickedEvent();
        [SerializeField] float m_startDelay = 0.5f;
        [SerializeField] float m_updateInterval = 0.25f;
        [SerializeField] float m_minIntervalTime = 0.25f;

        private bool m_isHold = false;
        private float m_originalInterval = 0.5f;
        private Coroutine m_coStartHold = null;

        private void Awake()
        {
            initEventTriggers();
            m_originalInterval = m_updateInterval;
        }

        private void initEventTriggers()
        {
            var eventTrigger = GetComponent<EventTrigger>();
            if (null == eventTrigger)
            {
                eventTrigger = gameObject.AddComponent<EventTrigger>();
            }

            addEventListener(eventTrigger, EventTriggerType.PointerDown, onPointerDown);
            addEventListener(eventTrigger, EventTriggerType.PointerUp, onPointerUp);
        }

        private void addEventListener(EventTrigger eventTrigger, EventTriggerType eventID, Action<BaseEventData> listener)
        {
            foreach (var trigger in eventTrigger.triggers)
            {
                if (trigger.eventID == eventID)
                {
                    trigger.callback.AddListener(data => listener.Invoke(data));
                    return;
                }
            }

            var entry = new EventTrigger.Entry();
            entry.eventID = eventID;
            entry.callback.AddListener(data => listener.Invoke(data));
            eventTrigger.triggers.Add(entry);
        }

        private void onPointerDown(BaseEventData eventData)
        {
            m_clickedEvent?.Invoke((isSuccess) =>
            {
                if (isSuccess)
                {
                    m_isHold = true;

                    if (null != m_coStartHold)
                        StopCoroutine(m_coStartHold);

                    m_coStartHold = StartCoroutine(coStartHold());
                }
            });
        }

        private void onPointerUp(BaseEventData eventData)
        {
            m_isHold = false;
            m_updateInterval = m_originalInterval;
        }

        IEnumerator coStartHold()
        {
            yield return new WaitForSeconds(m_startDelay);

            m_coStartHold = null;

            if (!m_isHold)
                yield break;

            StartCoroutine(coUpdateHold());
        }

        IEnumerator coUpdateHold()
        {
            var wfs = new WaitForSeconds(m_updateInterval);

            bool isLoop = true;
            while (isLoop)
            {
                m_clickedEvent?.Invoke((isSuccess) =>
                {
                    if (isSuccess)
                    {
                        if (!m_isHold)
                            isLoop = false;
                    }
                    else
                    {
                        isLoop = false;
                    }

                    if (m_minIntervalTime < m_updateInterval)
                        m_updateInterval *= GameSettings.instance.buttonEventIntervalWeight;
                });

                yield return wfs;
            }
        }
    }
}