using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public class UITextFadeColor : MonoBehaviour
    {
        [SerializeField] TextSelector m_text = new TextSelector();
        [SerializeField] Color m_sourceColor = Color.white;
        [SerializeField] Color m_destColor = Color.white;
        [SerializeField] float m_fadeInOutTime = 0.5f;
        [Tooltip("-1ÀÌ¸י Loop")]
        [SerializeField] int m_fadeInOutCount = 1;

        public void run()
        {
            stop();

            StartCoroutine(coRun());
        }

        public void stop()
        {
            StopAllCoroutines();
        }

        IEnumerator coRun()
        {
            bool isLoop = 0 > m_fadeInOutCount;
            var changeType = CoroutineHelper.createTimeType(m_fadeInOutTime);

            if (isLoop)
            {
                yield return StartCoroutine(CoroutineHelper.instance.coPingPongValue(this, 0.0f, 1.0f, changeType, true, (value, end) =>
                {
                    m_text.color = Color.Lerp(m_sourceColor, m_destColor, value);
                }, null));
            }
            else
            {
                for (int i = 0; i < m_fadeInOutCount; ++i)
                {
                    yield return StartCoroutine(CoroutineHelper.instance.coPingPongValue(this, 0.0f, 1.0f, changeType, false, (value, end) =>
                    {
                        m_text.color = Color.Lerp(m_sourceColor, m_destColor, value);
                    }, null));
                }
            }
        }
    }
}