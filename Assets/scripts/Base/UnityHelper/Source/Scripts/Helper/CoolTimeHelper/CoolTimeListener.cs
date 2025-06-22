using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;
using System;

namespace UnityHelper
{
    public class CoolTimeListener : Disposable
    {
        public struct RegistCallback
        {
            public long id;
            public Action<long> start;
            public Action<long> end;
            public Action<long, int, int, int> update;
        }

        /// <summary>
        /// id, h, m, s
        /// </summary>
        private Action<long, int, int, int> m_updateCallback = null;
        private Action<long> m_endCallback = null;
        private Action<long> m_startCallback = null;
        private long m_id = 0;
        private long m_coolTime = 0;
        private int m_interval = 0;
        private bool m_isLoop = false;
        private Coroutine m_coCoolTime = null;

        public long coolTime { get { return m_coolTime; } }
        public int interval { get { return m_interval; } }
        public bool isLoop { get { return m_isLoop; } }
        
        public virtual void initialize(long _id, long _coolTime, int _interval, bool _isLoop)
        {
            m_id = _id;
            m_coolTime = _coolTime;
            m_interval = _interval;
            m_isLoop = _isLoop;

            if (0 < m_coolTime)
                startCoolTimeCoroutine(new DateTime(m_coolTime));
        }

        public void regist(RegistCallback callback)
        {
            if (null != callback.start)
                m_startCallback += callback.start;
            if (null != callback.end)
                m_endCallback += callback.end;
            if (null != callback.update)
                m_updateCallback += callback.update;
        }

        public void unregist(RegistCallback callback)
        {
            if (null != callback.start)
                m_startCallback -= callback.start;
            if (null != callback.end)
                m_endCallback -= callback.end;
            if (null != callback.update)
                m_updateCallback -= callback.update;
        }

        public void start()
        {
            if (TimeHelper.isCoolTime(m_coolTime))
                return;

            DateTime coolTimeDate = DateTime.Now.AddSeconds(m_interval);
            m_coolTime = coolTimeDate.Ticks;
            startCoolTimeCoroutine(coolTimeDate); 
        }

        public void restart(long coolTime)
        {
            m_coolTime = coolTime;
            stopCoolTimeCoroutine();

            if (0 < coolTime)
                startCoolTimeCoroutine(new DateTime(coolTime));
        }

        public void stop()
        {
            stopCoolTimeCoroutine();

            m_coolTime = 0;

            if (null != m_endCallback)
            {
                if (Logx.isActive)
                    Logx.trace("CoolTime end, id {0}", m_id);

                m_endCallback(m_id);
            }
        }

        private void startCoolTimeCoroutine(DateTime coolTimeDate)
        {
            if (null != m_coCoolTime)
                StopCoroutine(m_coCoolTime);

            m_startCallback?.Invoke(m_id);

            if (null == m_updateCallback)
                m_coCoolTime = StartCoroutine(coEndCoolTime(coolTimeDate));
            else
                m_coCoolTime = StartCoroutine(coUpdateCoolTime(coolTimeDate));
        }

        private void stopCoolTimeCoroutine()
        {
            if (null != m_coCoolTime)
            {
                StopCoroutine(m_coCoolTime);
                m_coCoolTime = null;
            }
        }

        IEnumerator coUpdateCoolTime(DateTime coolTimeDate)
        {
            bool isLoop = true;
            while (isLoop)
            {
                yield return CoroutineHelper.instance.coUpdateCoolTime(coolTimeDate, true, (isEnd, second) =>
            {
                if (isEnd)
                {
                    m_coolTime = 0;
                    if (null != m_endCallback)
                    {
                        if (Logx.isActive)
                            Logx.trace("CoolTime end, id {0}", m_id);
                        m_endCallback(m_id);
                    }

                    if (m_isLoop)
                    {
                        coolTimeDate = DateTime.Now.AddSeconds(m_interval);
                        m_coolTime = coolTimeDate.Ticks;
                    }
                    else
                    {
                        isLoop = false;
                    }
                }
                else
                {
                    if (null != m_updateCallback)
                    {
                        TimeHelper.secondToTime((int)second, out int h, out int m, out int s);
                        m_updateCallback(m_id, h, m, s);
                    }
                }
            });
            }
        }

        IEnumerator coEndCoolTime(DateTime coolTimeDate)
        {
            while (true)
            {
                TimeSpan ts = coolTimeDate - DateTime.Now;
                yield return new WaitForSeconds((float)ts.TotalSeconds);

                m_coolTime = 0;
                if (null != m_endCallback)
                {
                    if (Logx.isActive)
                        Logx.trace("CoolTime end, id {0}", m_id);
                    m_endCallback(m_id);
                }

                if (m_isLoop)
                {
                    coolTimeDate = DateTime.Now.AddSeconds(m_interval);
                    m_coolTime = coolTimeDate.Ticks;
                }
                else
                {
                    break;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                stopCoolTimeCoroutine();
            }
        }
    }
}