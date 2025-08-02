using System;
using UnityEngine;

namespace UnityHelper
{
    public class UISceneLoader : MonoBehaviour
    {
        [Serializable]
        public class ProgressRotator
        {
            public Transform transform = null;
            public float speed = 1.0f;
        }

        [Serializable]
        public class FadeInOut
        {
            public float speed = 2.0f;
            public bool isInverse = false;
            public bool isImmediately = false;
        }

        [SerializeField] TextSelector m_title = new TextSelector();
        [SerializeField] UIGauge m_progressGauge = null;
        [SerializeField] ProgressRotator m_progressRotator = new ProgressRotator();
        [SerializeField] CanvasGroup m_canvasGroup = null;
        [SerializeField] FadeInOut m_fadeIn = new FadeInOut();
        [SerializeField] FadeInOut m_fadeOut = new FadeInOut();
        [SerializeField] RectTransform m_bg = null;

        private CoroutineHelper.Data m_moveBgData;

        protected float fadeInSpeed => m_fadeIn.speed;
        protected float fadeOutSpeed => m_fadeOut.speed;
        protected RectTransform bg => m_bg;

        public static UISceneLoader create(string path)
        {
            if (Logx.isActive)
                Logx.assert(!string.IsNullOrEmpty(path), "Invalid UISceneLoader.create paramter, path is null or empty");

            UISceneLoader loaderGameObject = Resources.Load<UISceneLoader>(path);
            if (Logx.isActive)
                Logx.assert(null != loaderGameObject, "Failed UISceneLoader create(), loaderGameObject is null");

            UISceneLoader loaderInstance = GameObject.Instantiate<UISceneLoader>(loaderGameObject);
            if (Logx.isActive)
                Logx.assert(null != loaderInstance, "Failed UISceneLoader create(), loaderInstance is null");

            return loaderInstance;
        }

        public void moveBg()
        {
            if (null == m_bg)
                return;

            var startPosition = m_bg.anchoredPosition;
            var targetPosition = new Vector2(m_bg.anchoredPosition.x + 1440, m_bg.anchoredPosition.y - 1440);
            var changeType = CoroutineHelper.createTimeType(10.0f);
            m_moveBgData = CoroutineHelper.instance.start(CoroutineHelper.instance.coChangeUIVector(startPosition, targetPosition, changeType, (v, done) =>
            {
                m_bg.anchoredPosition = v;

                if (done)
                {
                    m_bg.anchoredPosition = startPosition;
                }
            }));
        }

        public virtual void fadeOut(Action callback)
        {
            if (Logx.isActive)
                Logx.assert(null != callback, "Invalid fadeOut parameter, callback is null");

            setProgress(0.0f);
            setTitle("");

            var start = m_fadeOut.isInverse ? 1.0f : 0.0f;
            var dest = m_fadeOut.isInverse ? 0.0f : 1.0f;

            if (m_fadeOut.isImmediately)
            {
                setCanvasAlpha(dest);
                callback();
            }
            else
            {
                var changeType = CoroutineHelper.createSpeedType(m_fadeOut.speed);
                CoroutineHelper.instance.changeValue(start, dest, changeType, (v, done) =>
                {
                    setCanvasAlpha(v);

                    if (done)
                        callback();
                });
            }
        }

        public virtual void fadeIn()
        {
            var start = m_fadeIn.isInverse ? 0.0f : 1.0f;
            var dest = m_fadeIn.isInverse ? 1.0f : 0.0f;

            if (m_fadeIn.isImmediately)
            {
                setCanvasAlpha(dest);
                destroy();
            }
            else
            {
                var changeType = CoroutineHelper.createSpeedType(m_fadeIn.speed);
                CoroutineHelper.instance.changeValue(start, dest, changeType, (v, done) =>
                {
                    setCanvasAlpha(v);

                    if (done)
                        destroy();
                });
            }
        }

        public void setProgress(float p)
        {
            if (null == m_progressGauge)
                return;

            m_progressGauge.setValue(p, 1.0f);
        }

        public void setTitle(string title)
        {
            if (null == m_title)
                return;

            m_title.text = title;
        }

        private void setCanvasAlpha(float a)
        {
            if (null == m_canvasGroup)
                return;

            m_canvasGroup.alpha = a;
        }

        void Update()
        {
            updateProgressRotator();
        }

        private void updateProgressRotator()
        {
            if (null == m_progressRotator.transform)
                return;

            var euler = m_progressRotator.transform.rotation.eulerAngles;
            euler.z -= Time.deltaTime * m_progressRotator.speed;
            m_progressRotator.transform.rotation = Quaternion.Euler(euler);
        }

        protected virtual void destroy()
        {
            m_moveBgData.stop();
            GameObject.Destroy(gameObject);
        }
    }
}