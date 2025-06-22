using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    /// <summary>
    /// BezierMove를 쓰는 경우에는 Canvas Render Mode가 Screen Space - Camera로 되어야지 잘 나온다.
    /// (다른 렌더 모드에서는 m_centerCurveScale를 꽤 크게 해야 된다.
    /// </summary>
    public abstract class UIGetItemEvent : UIEvent
    {
        private GameObject m_startObject = null;
        private GameObject m_destObject = null;
        private Vector2 m_runDelay = Vector2.zero;
        private int m_eventCount = 0;
        private int m_completeEventCount = 0;
        private Action m_completeCallback = null;
        private Action m_runDelayCallback = null;
        // event count
        private Action<int> m_oneEventEndCallback = null;

        public GameObject startObject { set { m_startObject = value; } }
        public GameObject destObject { set { m_destObject = value; } }
        public Vector2 runDelay { set { m_runDelay = value; } }
        public int eventCount { set { m_eventCount = value; } }
        public Action completeCallback { set { m_completeCallback = value; } }
        public Action runDelayCallback { set { m_runDelayCallback = value; } }
        public Action<int> oneEventEndCallback { set { m_oneEventEndCallback = value; } }

        protected abstract void createEventObject(Action<Disposable> callback);
        protected abstract BaseMove createMover();

        public void run()
        {
            CoroutineHelper.Data coData = new CoroutineHelper.Data
            {
                enumerator = coRuns()
            };
            CoroutineHelper.instance.start(ref coData);
        }

        IEnumerator coRuns()
        {
            for (int i = 0; i < m_eventCount; ++i)
            {
                CoroutineHelper.Data coData = new CoroutineHelper.Data
                {
                    enumerator = coRun()
                };
                CoroutineHelper.instance.start(ref coData);
            }

            WaitForSeconds ws = new WaitForSeconds(0.1f);
            while (m_completeEventCount < m_eventCount)
            {
                yield return ws;
            }

            m_completeCallback?.Invoke();
        }

        private float getRunDelay()
        {
            return UnityEngine.Random.Range(m_runDelay.x, m_runDelay.y);
        }

        IEnumerator coRun()
        {
            //  대기하는 동안 m_startObject가 없어질 수도 있어서 미리 갖고 있는다.
            Vector3 startPosition = m_startObject.transform.position;
            Vector3 destPosition = m_destObject.transform.position;
            Vector3 startSizeDelta = m_startObject.GetComponent<RectTransform>().sizeDelta;
            Vector3 destSizeDelta = m_destObject.GetComponent<RectTransform>().sizeDelta;

            destSizeDelta.x *= m_destObject.transform.localScale.x;
            destSizeDelta.y *= m_destObject.transform.localScale.y;
            destSizeDelta.z *= m_destObject.transform.localScale.z;

            yield return new WaitForSeconds(getRunDelay());
            m_runDelayCallback?.Invoke();

            createEventObject((eventObject) =>
            {
                if (null != eventObject)
                {
                    RectTransform rt_pool = eventObject.GetComponent<RectTransform>();
                    Vector2 oriPoolSizeDelta = rt_pool.sizeDelta;

                    BaseMove mover = createMover();
                    mover.moveObject = eventObject.gameObject;
                    mover.startPosition = startPosition;
                    mover.destPosition = destPosition;
                    mover.moveCallback = (t, oldMovePosition) =>
                    {
                        if (null != rt_pool)
                        {
                            rt_pool.sizeDelta = Vector2.Lerp(oriPoolSizeDelta, destSizeDelta, t);
                        }
                    };
                    mover.arriveCallback = () =>
                    {
                        ++m_completeEventCount;

                        rt_pool.sizeDelta = oriPoolSizeDelta;
                        eventObject.Dispose();

                        m_oneEventEndCallback?.Invoke(m_completeEventCount);
                    };
                    mover.move();
                }
            });
            
        }
    }
}