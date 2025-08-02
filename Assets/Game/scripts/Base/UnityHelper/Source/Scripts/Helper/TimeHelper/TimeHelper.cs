using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public partial class TimeHelper
    {
        private static float m_oldTimeScale = -1.0f;
        private static int m_pauseReferenceCount = 0;

        public static float time { get { return Time.time; } }
        public static float deltaTime { get { return Time.deltaTime; } }
        public static float smoothDeltaTime { get { return Time.smoothDeltaTime; } }
        public static float fixedDeltaTime { get { return Time.fixedDeltaTime; } }
        public static float unscaledDeltaTime { get { return Time.unscaledDeltaTime; } }
        public static float timeScale { get { return Time.timeScale; } set { Time.timeScale = value; } }
        public static bool isPause => 0 < m_pauseReferenceCount;

        public static bool isCoolTime(long tick)
        {
            if (0 == tick)
                return false;

            DateTime coolTimeDate = new DateTime(tick);
            return isCoolTime(ref coolTimeDate);
        }

        public static bool isCoolTime(string date)
        {
            if (string.IsNullOrEmpty(date))
                return false;

            if (DateTime.TryParse(date, out DateTime coolTimeDate))
                return isCoolTime(ref coolTimeDate);
            else
                return false;
        }

        public static bool isCoolTime(ref DateTime date)
        {
            return (DateTime.Now <= date);
        }

        public static long getCoolTime(int second)
        {
            if (Logx.isActive)
                Logx.assert(0 <= second, "Invalid second {0}", second);

            return DateTime.Now.AddSeconds(second).Ticks;
        }

        public static long getCoolTime(float second)
        {
            if (Logx.isActive)
                Logx.assert(0 <= second, "Invalid second {0}", second);

            return DateTime.Now.AddSeconds(second).Ticks;
        }

        public static void secondToTime(int totalSecond, out int h, out int m, out int s)
        {
            if (0 > totalSecond)
            {
                h = m = s = 0;
            }
            else
            {
                int totalMinute = secondToMinute(totalSecond);

                h = minuteToHour(totalMinute);
                m = totalMinute - (h * 60);
                s = totalSecond % 60;
            }
        }

        public static void secondToTime(int totalSecond, out int d, out int h, out int m, out int s)
        {
            if (0 > totalSecond)
            {
                d = h = m = s = 0;
            }
            else
            {
                int totalMinute = secondToMinute(totalSecond);
                int totalHour = minuteToHour(totalMinute);

                d = totalHour / 24;
                h = totalHour % 24;
                m = totalMinute - (totalHour * 60);
                s = totalSecond % 60;
            }
        }

        public static string secondToTimeString(int totalSecond)
        {
            int h, m, s;

            if (0 > totalSecond)
            {
                h = m = s = 0;
            }
            else
            {
                int totalMinute = secondToMinute(totalSecond);

                h = minuteToHour(totalMinute);
                m = totalMinute - (h * 60);
                s = totalSecond % 60;
            }

            return string.Format("{0:00}:{1:00}:{2:00}", h, m, s);
        }

        public static int hourToSecond(int hour)
        {
            if (0 > hour)
            {
                if (Logx.isActive)
                    Logx.assert(0 <= hour, "Invalid hour {0}", hour);

                return 0;
            }
            else
            {
                return hour * 60 * 60;
            }
        }

        public static int secondToHour(int second)
        {
            if (0 > second)
            {
                if (Logx.isActive)
                    Logx.assert(0 <= second, "Invalid second {0}", second);

                return 0;
            }
            else
            {
                return second / 3600;
            }
        }

        public static int secondToMinute(int second)
        {
            if (0 > second)
            {
                if (Logx.isActive)
                    Logx.assert(0 <= second, "Invalid second {0}", second);

                return 0;
            }
            else
            {
                return second / 60;
            }
        }

        public static int dayHourToMinute(int day, int hour)
        {
            int m = day * 1440;
            m += hour * 60;

            return m;
        }

        public static int dayToHour(int day)
        {
            int h = day * 24;

            return h;
        }

        public static int minuteToHour(int minute)
        {
            if (0 > minute)
            {
                if (Logx.isActive)
                    Logx.assert(0 <= minute, "Invalid minute {0}", minute);

                return 0;
            }
            else
            {
                return minute / 60;
            }
        }

        public static int timeToSecond(int h, int m, int s)
        {
            var second = h * 3600;
            second += m * 60;
            second += s;

            return second;
        }

        public static long getSecondUntilNow(string date)
        {
            if (Logx.isActive)
                Logx.assert(!string.IsNullOrEmpty(date), "date is null or empty");

            if (DateTime.TryParse(date, out DateTime dateTime))
            {
                return getSecondUntilNow(dateTime);
            }
            else
            {
                return 0;
            }
        }

        public static long getSecondUntilNow(long tick)
        {
            if (Logx.isActive)
                Logx.assert(0 < tick, "Invalid tick {0}", tick);

            return getSecondUntilNow(new DateTime(tick));
        }

        public static long getSecondUntilNow(DateTime dateTime)
        {
            TimeSpan timeSpan = DateTime.Now - dateTime;
            return (long)timeSpan.TotalSeconds;
        }

        public static long getRemainSecond(string date)
        {
            if (Logx.isActive)
                Logx.assert(!string.IsNullOrEmpty(date), "date is null or empty");

            if (DateTime.TryParse(date, out DateTime dateTime))
            {
                return getRemainSecond(dateTime);
            }
            else
            {
                return 0;
            }
        }

        public static long getRemainSecond(DateTime dateTime)
        {
            TimeSpan timeSpan = dateTime - DateTime.Now;
            return (long)timeSpan.TotalSeconds;
        }

        public static void setTimeScale(float timeScale)
        {
            if (0.0f > m_oldTimeScale)
                m_oldTimeScale = Time.timeScale;

            Time.timeScale = timeScale;
        }

        public static void restoreTimeScale()
        {
            if (0.0f > m_oldTimeScale)
                return;

            Time.timeScale = m_oldTimeScale;
            m_oldTimeScale = -1.0f;
        }

        public static void pause()
        {
            ++m_pauseReferenceCount;

            if (1 == m_pauseReferenceCount)
                setTimeScale(0.0f);
        }

        public static void resume()
        {
            m_pauseReferenceCount = Mathf.Max(m_pauseReferenceCount - 1, 0);

            if (0 == m_pauseReferenceCount)
                restoreTimeScale();
        }

        public static void Dispose()
        {
            m_oldTimeScale = -1.0f;
            timeScale = 1.0f;
            m_pauseReferenceCount = 0;
        }

        public static long calcDailyCoolTime(int dailyResetTime)
        {
            var today = DateTime.Now.Date;
            var resetDate = today.AddSeconds(dailyResetTime);

            var now = DateTime.Now;
            if (resetDate < now)
            {
                var tomorrow = today.AddDays(1);
                return tomorrow.AddSeconds(dailyResetTime).Ticks;                
            }
            else
            {
                return resetDate.Ticks;
            }
        }
    }
}