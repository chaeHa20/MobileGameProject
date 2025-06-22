using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace UnityHelper
{
    public class UIRollingObject : MonoBehaviour
    {
        [SerializeField] float m_rollingTime = 0.2f;
        [SerializeField] GameObject m_mask = null;
        [SerializeField] GameObject m_position = null;

        private List<GameObject> m_positions = new List<GameObject>();
        private List<GameObject> m_objects = new List<GameObject>();

        private int m_activeObjectIndex = 0;
        private bool m_isRolling = false;

        private void Awake()
        {
            checkMask();
            getChilds(m_mask, m_objects, 2);
            getChilds(m_position, m_positions, 3);
        }

        void Start()
        {
            immediatelyRolling(0);
        }

        private void checkMask()
        {
            if (Logx.isActive)
            {
                RectMask2D mask = m_mask.GetComponent<RectMask2D>();
                Logx.assert(null != mask, "Failed find RectMask2D component");
            }
        }

        private void getChilds(GameObject parent, List<GameObject> list, int checkCount)
        {
            list.Clear();
            for (int i = 0; i < parent.transform.childCount; ++i)
            {
                list.Add(parent.transform.GetChild(i).gameObject);
            }

            if (Logx.isActive)
                Logx.assert(checkCount == list.Count, "Invalid rolling child count");
        }

        public void immediatelyRolling(int objectIndex)
        {
            m_activeObjectIndex = objectIndex;

            int otherObjectIndex = getOtherObjectIndex(objectIndex);
            m_objects[objectIndex].SetActive(true);
            m_objects[otherObjectIndex].SetActive(false);

            m_objects[objectIndex].transform.position = m_positions[1].transform.position;
            m_objects[otherObjectIndex].transform.position = m_positions[2].transform.position;
        }

        private int getOtherObjectIndex(int objectIndex)
        {
            return (0 == objectIndex) ? 1 : 0;
        }

        public void rolling()
        {
            if (m_isRolling)
                return;

            StartCoroutine(coRolling());
        }

        IEnumerator coRolling()
        {
            m_isRolling = true;

            int otherObjectIndex = getOtherObjectIndex(m_activeObjectIndex);

            int rollingCount = 0;
            StartCoroutine(coRolling(m_activeObjectIndex, 0, () => { ++rollingCount; }));
            StartCoroutine(coRolling(otherObjectIndex, 1, () => { ++rollingCount; }));

            while (2 > rollingCount)
            {
                yield return null;
            }

            m_objects[m_activeObjectIndex].SetActive(false);
            m_objects[m_activeObjectIndex].transform.position = m_positions[2].transform.position;
            m_objects[otherObjectIndex].SetActive(true);

            m_isRolling = false;
            m_activeObjectIndex = otherObjectIndex;
        }

        IEnumerator coRolling(int objectIndex, int positionIndex, Action callback)
        {
            GameObject rollingObj = m_objects[objectIndex];
            rollingObj.SetActive(true);

            Vector3 objectPosition = rollingObj.transform.position;
            Vector3 targetPosition = m_positions[positionIndex].transform.position;

            float elapsedTime = 0.0f;
            while (elapsedTime < m_rollingTime)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / m_rollingTime;
                t = Mathf.Min(1.0f, t);

                rollingObj.transform.position = Vector3.Lerp(objectPosition, targetPosition, t);
                yield return null;
            }

            callback();
        }

        public void onClick()
        {
            rolling();
        }
    }
}