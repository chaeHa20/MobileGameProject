using System;
using UnityEngine;

namespace UnityHelper
{
    public class UICoolTime : MonoBehaviour
    {
        [SerializeField] TextSelector m_coolTimeText = new TextSelector();
        [SerializeField] UIGaugeImage m_coolTimeGauge = null;
        [SerializeField] bool m_isOneSecondWait = true;
        [SerializeField] bool m_isCheckOnEnable = true;

        private long m_coolTime = 0;
        private Action m_endCallback = null;
        private Func<int, int, int, int, string> m_formatter = null;
        private string m_defaultText = "";

        public TextSelector coolTimeText => m_coolTimeText;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coolTimeDate"></param>
        /// <param name="callback">end callback</param>
        /// <param name="formatter"></param>
        public void start(DateTime coolTimeDate, Action endCallback, Func<int, int, int, int, string> formatter = null)
        {
            start(coolTimeDate.Ticks, endCallback);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coolTime"></param>
        /// <param name="endCallback">end callback</param>
        /// <param name="formatter">d, h, m, s, return</param>
        public void start(long coolTime, Action endCallback, Func<int, int, int, int, string> formatter = null)
        {
            stop();

            m_coolTime = coolTime;
            m_endCallback = endCallback;
            m_formatter = formatter;

            if (TimeHelper.isCoolTime(coolTime))
            {
                if (gameObject.activeInHierarchy)
                {
                    if (CoroutineHelper.isNullInstance())
                        return;

                    DateTime coolTimeDate = new DateTime(coolTime);

                    TimeSpan span = coolTimeDate - DateTime.Now;
                    var totalSeconds = span.TotalSeconds;

                    StartCoroutine(CoroutineHelper.instance.coUpdateCoolTime(coolTimeDate, m_isOneSecondWait, (isEnd, second) =>
                    {
                        if (null != m_coolTimeText)
                        {
                            TimeHelper.secondToTime((int)second, out int d, out int h, out int m, out int s);
                            if (null == formatter)
                                m_coolTimeText.text = getFormat(d, h, m, s);
                            else
                                m_coolTimeText.text = formatter(d, h, m, s);
                        }

                        if (null != m_coolTimeGauge)
                        {
                            if (second < 1)
                                m_coolTimeGauge.fillAmount = 0.0f;
                            else
                                m_coolTimeGauge.fillAmount = (float)(second / totalSeconds);
                        }// TODO : 2024-07-05 by pms

                        if (isEnd)
                        {
                            endCallback?.Invoke();
                        }
                    }));
                }
            }
            else
            {
                if (null != m_coolTimeText)
                    m_coolTimeText.text = m_defaultText;
                if (null != m_coolTimeGauge)
                    m_coolTimeGauge.fillAmount = 0.0f;

                endCallback?.Invoke();
            }
        }

        protected virtual string getFormat(int d, int h, int m, int s)
        {
            return string.Format("{0:00}:{1:00}:{2:00}", h, m, s);
        }

        public void stop()
        {
            StopAllCoroutines();
        }

        public void setDefaultText(string defaultText)
        {
            m_defaultText = defaultText;
        }
        // TODO : 2024-04-02 by pms
        public void setDefaultCoolTimeText()
        {
            m_coolTimeText.text = m_defaultText;
        }
        //
        protected virtual void OnEnable()
        {
            if (m_isCheckOnEnable)
                start(m_coolTime, m_endCallback, m_formatter);
        }

        public bool isCoolTime()
        {
            return TimeHelper.isCoolTime(m_coolTime);
        }

        public void clear()
        {
            stop();
            setDefaultText("");
            setDefaultCoolTimeText();
            m_coolTime = 0;
            if (null != m_coolTimeGauge)
                m_coolTimeGauge.fillAmount = 0.0f;
        }
    }
}