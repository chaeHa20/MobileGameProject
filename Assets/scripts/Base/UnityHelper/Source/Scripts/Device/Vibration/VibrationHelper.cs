using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;

namespace UnityHelper
{
    public class VibrationHelper : MonoSingleton<VibrationHelper>
    {
        /// <summary>
        /// vibration id, coroutine
        /// </summary>
        private Dictionary<eVibration, Coroutine> m_loopVibrations = new Dictionary<eVibration, Coroutine>();
        private Coroutine m_coroutine = null;


        public virtual void initialize()
        {
            
        }

        /// <param name="count">0보다 작으면 루프</param>
        public void vibrate(eVibration vibrationId, HapticTypes hapticTypes, float interval, int count)
        {
            if (Logx.isActive)
                Logx.trace("vibrate {0}", vibrationId);

            if (!isEnable())
                return;

            if (0 > count)
            {
                if (isAlreadyLooping(vibrationId))
                    return;
            }
            if(null != m_coroutine)
                StopCoroutine(m_coroutine);

            m_coroutine = StartCoroutine(coVibrate(vibrationId, hapticTypes, interval, count));
            if (0 > count)
                addLoopVibration(vibrationId, m_coroutine);
        }

        IEnumerator coVibrate(eVibration vibrationId, HapticTypes hapticTypes, float interval, int count)
        {
            if (0 < count)
            {
                for (int i = 0; i < count; ++i)
                {
                    yield return StartCoroutine(coVibrate(vibrationId, hapticTypes, interval));
                }
            }
            else
            {
                while (true)
                {
                    yield return StartCoroutine(coVibrate(vibrationId, hapticTypes, interval));
                }
            }
        }

        IEnumerator coVibrate(eVibration vibrationId, HapticTypes hapticTypes, float interval)
        {
            MMVibrationManager.Haptic(hapticTypes);

            if (0.0f < interval)
            {
                yield return new WaitForSeconds(interval);
            }
            else
            {
                yield return null;
            }
        }

        private void addLoopVibration(eVibration vibrationId, Coroutine coroutine)
        {
            if (isAlreadyLooping(vibrationId))
                return;

            m_loopVibrations.Add(vibrationId, coroutine);
        }

        public void stopLoopVibration(eVibration vibrationId)
        {
            if (Logx.isActive)
                Logx.trace("stopLoopVibration {0}", vibrationId);

            if (!m_loopVibrations.TryGetValue(vibrationId, out Coroutine coroutine))
                return;

            StopCoroutine(coroutine);
            m_loopVibrations.Remove(vibrationId);
        }

        private bool isAlreadyLooping(eVibration vibrationId)
        {
            return m_loopVibrations.ContainsKey(vibrationId);
        }

        private bool isEnable()
        {
            return GameOptionHelper.instance.isActiveViveration;
        }

        public void Dispose()
        {
            clearLoopVibrations();
        }

        private void clearLoopVibrations()
        {
            foreach (var pair in m_loopVibrations)
            {
                StopCoroutine(pair.Value);
            }
            m_loopVibrations.Clear();
        }
    }
}