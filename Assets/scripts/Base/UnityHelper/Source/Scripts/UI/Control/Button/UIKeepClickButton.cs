using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Xml.Schema;

namespace UnityHelper
{
    [RequireComponent(typeof(Button))]
    public class UIKeepClickButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        [SerializeField] UnityEvent m_click = null;
        [SerializeField] float m_firstClickWaitTime = 0.5f;
        [SerializeField] float m_keepClickWaitTime = 0.1f;

        private Button m_button = null;

        void Awake()
        {
            m_button = GetComponent<Button>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!m_button.interactable)
                return;

            StopAllCoroutines();
            StartCoroutine(coPointerDown());
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            StopAllCoroutines();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            StopAllCoroutines();
        }

        IEnumerator coPointerDown()
        {
            if (null == m_click)
                yield break;

            click();

            yield return new WaitForSeconds(m_firstClickWaitTime);

            var wfs = new WaitForSeconds(m_keepClickWaitTime);
            while (true)
            {
                click();

                yield return wfs;
            }
        }

        private void click()
        {
            m_click.Invoke();
        }
    }
}