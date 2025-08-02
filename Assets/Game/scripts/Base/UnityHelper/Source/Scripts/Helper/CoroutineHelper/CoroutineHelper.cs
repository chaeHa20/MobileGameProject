using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace UnityHelper
{
    public class CoroutineHelper : MonoSingleton<CoroutineHelper>, IDisposable
    {
        public struct ChangeType
        {
            public enum eType { Time, Speed }
            public eType type;
            public float value;
            public bool isUnscaled;

            public ChangeType(eType _type, float _value, bool _isUnscaled)
            {
                type = _type;
                value = _value;
                isUnscaled = _isUnscaled;
            }

            public static ChangeType timeType(float _value, bool _isUnscaled = false)
            {
                return new ChangeType(eType.Time, _value, _isUnscaled);
            }

            public static ChangeType speedType(float _value, bool _isUnscaled = false)
            {
                return new ChangeType(eType.Speed, _value, _isUnscaled);
            }
        }

        public struct Data
        {
            public MonoBehaviour mono;
            public IEnumerator enumerator;
            public Coroutine coroutine;

            public void stop()
            {
                if (null == coroutine)
                    return;

                if (null != mono)
                {
                    mono.StopCoroutine(coroutine);
                    coroutine = null;
                }
                else
                {
                    CoroutineHelper.instance.stop(ref this);
                }
            }
        }

        public static ChangeType createTimeType(float value, bool isUnscaled = false)
        {
            return new ChangeType(ChangeType.eType.Time, value, isUnscaled);
        }

        public static ChangeType createSpeedType(float value, bool isUnscaled = false)
        {
            return new ChangeType(ChangeType.eType.Speed, value, isUnscaled);
        }

        /// <param name="callback">value, done</param>
        public void changeValue(float start, float dest, ChangeType changeType, Action<float, bool> callback)
        {
            if (Logx.isActive)
                Logx.assert(null != callback, "Invalid changeValue parameter, callback is null");

            StartCoroutine(coChangeValue(start, dest, changeType, callback));
        }

        /// <param name="callback">value, done</param>
        public IEnumerator coChangeValue(float start, float dest, ChangeType changeType, Action<float, bool> callback)
        {
            if (0.0f < changeType.value)
            {
                float changeTime;
                if (ChangeType.eType.Time == changeType.type)
                {
                    changeTime = changeType.value;
                }
                else
                {
                    float s = Mathf.Abs(start - dest);
                    changeTime = s / changeType.value;
                }
                float elapsedTime = 0.0f;

                while (elapsedTime < changeTime)
                {
                    elapsedTime += changeType.isUnscaled ? TimeHelper.unscaledDeltaTime : TimeHelper.deltaTime;
                    float t = Mathf.Min(elapsedTime / changeTime, 1.0f);
                    float v = Mathf.Lerp(start, dest, t);

                    callback(v, false);

                    yield return null;
                }
            }

            callback(dest, true);
        }

        /// <param name="callback">value, done</param>
        public IEnumerator coChangeVector(Vector3 start, Vector3 dest, ChangeType changeType, Action<Vector3, bool> callback)
        {
            if (0.0f < changeType.value)
            {
                float changeTime;
                if (ChangeType.eType.Time == changeType.type)
                {
                    changeTime = changeType.value;
                }
                else
                {
                    float s = Vector3.Distance(start, dest);
                    changeTime = s / changeType.value;
                }
                float elapsedTime = 0.0f;

                while (elapsedTime < changeTime)
                {
                    elapsedTime += changeType.isUnscaled ? TimeHelper.unscaledDeltaTime : TimeHelper.deltaTime;
                    float t = Mathf.Min(elapsedTime / changeTime, 1.0f);
                    var v = Vector3.Lerp(start, dest, t);

                    callback(v, false);

                    yield return null;
                }
            }

            callback(dest, true);
        }

        public IEnumerator coChangeUIVector(Vector2 start, Vector2 dest, ChangeType changeType, Action<Vector2, bool> callback)
        {
            if (0.0f < changeType.value)
            {
                float changeTime;
                if (ChangeType.eType.Time == changeType.type)
                {
                    changeTime = changeType.value;
                }
                else
                {
                    float s = Vector2.Distance(start, dest);
                    changeTime = s / changeType.value;
                }
                float elapsedTime = 0.0f;

                while (elapsedTime < changeTime)
                {
                    elapsedTime += changeType.isUnscaled ? TimeHelper.unscaledDeltaTime : TimeHelper.deltaTime;
                    float t = Mathf.Min(elapsedTime / changeTime, 1.0f);
                    var v = Vector2.Lerp(start, dest, t);

                    callback(v, false);

                    yield return null;
                }
            }

            callback(dest, true);
        }

        /// <param name="callback">value, done</param>
        public void changeBigValue(System.Numerics.BigInteger start, System.Numerics.BigInteger dest, float changeTime, bool isUnscaled, Action<System.Numerics.BigInteger, bool> callback)
        {
            if (Logx.isActive)
                Logx.assert(null != callback, "Invalid changeValue parameter, callback is null");

            StartCoroutine(coChangeBigValue(start, dest, changeTime, isUnscaled, callback));
        }

        /// <param name="callback">value, done</param>
        public IEnumerator coChangeBigValue(System.Numerics.BigInteger start, System.Numerics.BigInteger dest, float changeTime, bool isUnscaled, Action<System.Numerics.BigInteger, bool> callback)
        {
            float elapsedTime = 0.0f;
            while (elapsedTime < changeTime)
            {
                elapsedTime += isUnscaled ? TimeHelper.unscaledDeltaTime : TimeHelper.deltaTime;

                var t = elapsedTime / changeTime;
                if (1.0f < t)
                    t = 1.0f;

                var v = MathHelper.lerp(start, dest, t);
                callback(v, false);

                yield return null;
            }

            callback(dest, true);
        }

        /// <param name="callback">value, t, done</param>
        public IEnumerator coChangePosition(GameObject start, GameObject dest, ChangeType changeType, Action<UnityEngine.Vector3, float, bool> callback)
        {
            if (0.0f < changeType.value)
            {
                float changeTime;
                if (ChangeType.eType.Time == changeType.type)
                {
                    changeTime = changeType.value;
                }
                else
                {
                    var s = UnityEngine.Vector3.Distance(start.transform.position, dest.transform.position);
                    changeTime = s / changeType.value;
                }
                float elapsedTime = 0.0f;

                while (elapsedTime < changeTime)
                {
                    elapsedTime += changeType.isUnscaled ? TimeHelper.unscaledDeltaTime : TimeHelper.deltaTime;
                    float t = Mathf.Min(elapsedTime / changeTime, 1.0f);
                    var v = UnityEngine.Vector3.Lerp(start.transform.position, dest.transform.position, t);

                    callback(v, t, false);

                    yield return null;
                }
            }

            callback(dest.transform.position, 1.0f, true);
        }

        public void pingPongValue(float curValue, float maxValue, ChangeType changeType, bool isLoop, Action<float, bool> updateCallback, Action endCallback)
        {
            if(Logx.isActive)
            {
                Logx.assert(null != updateCallback, "Invalid pingPongValue parameter, updateCallback is null");
            }

            StartCoroutine(coPingPongValue(this, curValue, maxValue, changeType, isLoop, updateCallback, endCallback));
        }

        /// <param name="updateCallback">value, end</param>
        /// <returns></returns>
        public IEnumerator coPingPongValue(MonoBehaviour mono, float curValue, float maxValue, ChangeType changeType, bool isLoop, Action<float, bool> updateCallback, Action endCallback)
        {
            if (isLoop)
            {
                while (true)
                {
                    yield return mono.StartCoroutine(coChangeValue(curValue, maxValue, changeType, updateCallback));
                    yield return mono.StartCoroutine(coChangeValue(maxValue, curValue, changeType, updateCallback));

                    endCallback?.Invoke();
                }
            }
            else
            {
                yield return mono.StartCoroutine(coChangeValue(curValue, maxValue, changeType, updateCallback));
                yield return mono.StartCoroutine(coChangeValue(maxValue, curValue, changeType, updateCallback));

                endCallback?.Invoke();
            }
        }

        public void fadeInToOut(Image image, ChangeType changeType, float waitTime, Action callback)
        {
            StartCoroutine(coFadeInToOut(image, changeType, waitTime, callback));
        }

        /// <param name="callback">alpha, done</param>
        public void fadeInToOut(ChangeType changeType, float waitTime, Action<float, bool> callback)
        {
            StartCoroutine(coFadeInToOut(changeType, waitTime, callback));
        }

        public void fadeIn(Image image, ChangeType changeType, Action callback)
        {
            StartCoroutine(coFadeIn(image, changeType, callback));
        }

        public void fadeIn(List<Image> images, ChangeType changeType, Action callback)
        {
            StartCoroutine(coFadeIn(images, changeType, callback));
        }

        /// <param name="callback">alpha, done</param>
        public void fadeIn(ChangeType changeType, Action<float, bool> callback)
        {
            StartCoroutine(coFadeIn(changeType, callback));
        }

        public void fadeOut(Image image, ChangeType changeType, Action callback)
        {
            StartCoroutine(coFadeOut(image, changeType, callback));
        }

        public void fadeOut(List<Image> images, ChangeType changeType, Action callback)
        {
            StartCoroutine(coFadeOut(images, changeType, callback));
        }

        /// <param name="callback">alpha, done</param>
        public void fadeOut(ChangeType changeType, Action<float, bool> callback)
        {
            StartCoroutine(coFadeOut(changeType, callback));
        }

        public IEnumerator coFadeInToOut(Image image, ChangeType changeType, float waitTime, Action callback)
        {
            yield return StartCoroutine(coFadeOut(image, changeType, null));            
            if (0.0f < waitTime) yield return new WaitForSeconds(waitTime);
            yield return StartCoroutine(coFadeIn(image, changeType, null));

            if (null == image)
                yield break;

            callback?.Invoke();
        }

        /// <param name="callback">alpha, done</param>
        private IEnumerator coFadeInToOut(ChangeType changeType, float waitTime, Action<float, bool> callback)
        {
            yield return StartCoroutine(coFadeOut(changeType, (value, done) => { callback?.Invoke(value, false); }));            
            if (0.0f < waitTime) yield return new WaitForSeconds(waitTime);
            yield return StartCoroutine(coFadeIn(changeType, (value, done) => { callback?.Invoke(value, false); }));

            callback?.Invoke(0.0f, true);
        }

        private IEnumerator coFadeIn(Image image, ChangeType changeType, Action callback)
        {
            Color color = image.color;
            yield return StartCoroutine(coChangeValue(1.0f, 0.0f, changeType, (value, done) =>
            {
                if (image == null)
                    return;

                color.a = value;
                image.color = color;
            }));

            callback?.Invoke();
        }

        private IEnumerator coFadeIn(List<Image> images, ChangeType changeType, Action callback)
        {
            yield return StartCoroutine(coChangeValue(1.0f, 0.0f, changeType, (value, done) =>
            {
                for (int i = 0; i < images.Count; ++i)
                {
                    Image image = images[i];
                    image.color = new Color(image.color.r, image.color.g, image.color.b, value);
                }
            }));

            callback?.Invoke();
        }

        public IEnumerator coFadeIn(ChangeType changeType, Action<float, bool> callback)
        {
            yield return StartCoroutine(coChangeValue(1.0f, 0.0f, changeType, (value, done) =>
            {
                callback?.Invoke(value, done);
            }));
        }

        private IEnumerator coFadeOut(Image image, ChangeType changeType, Action callback)
        {
            Color color = image.color;
            yield return StartCoroutine(coChangeValue(0.0f, 1.0f, changeType, (value, done) =>
            {
                if (image == null)
                    return;

                color.a = value;
                image.color = color;
            }));

            callback?.Invoke();
        }

        private IEnumerator coFadeOut(List<Image> images, ChangeType changeType, Action callback)
        {
            yield return StartCoroutine(coChangeValue(0.0f, 1.0f, changeType, (value, done) =>
            {
                for (int i = 0; i < images.Count; ++i)
                {
                    Image image = images[i];
                    image.color = new Color(image.color.r, image.color.g, image.color.b, value);
                }
            }));

            callback?.Invoke();
        }

        /// <param name="callback">value, done</param>
        public IEnumerator coFadeOut(ChangeType changeType, Action<float, bool> callback)
        {
            yield return StartCoroutine(coChangeValue(0.0f, 1.0f, changeType, (value, done) =>
            {
                callback?.Invoke(value, done);
            }));
        }

        public void fadeInToOut(SpriteRenderer spriteRenderer, ChangeType changeType, Action callback)
        {
            StartCoroutine(coFadeInToOut(spriteRenderer, changeType, callback));
        }

        public void fadeIn(SpriteRenderer spriteRenderer, ChangeType changeType, Action callback)
        {
            StartCoroutine(coFadeIn(spriteRenderer, changeType, callback));
        }

        public void fadeIn(List<SpriteRenderer> spriteRenderers, ChangeType changeType, Action callback)
        {
            StartCoroutine(coFadeIn(spriteRenderers, changeType, callback));
        }

        public void fadeOut(SpriteRenderer spriteRenderer, ChangeType changeType, Action callback)
        {
            StartCoroutine(coFadeOut(spriteRenderer, changeType, callback));
        }

        public void fadeOut(List<SpriteRenderer> spriteRenderers, ChangeType changeType, Action callback)
        {
            StartCoroutine(coFadeOut(spriteRenderers, changeType, callback));
        }

        public IEnumerator coFadeInToOut(SpriteRenderer spriteRenderer, ChangeType changeType, Action callback)
        {
            yield return StartCoroutine(coFadeOut(spriteRenderer, changeType, null));
            yield return StartCoroutine(coFadeIn(spriteRenderer, changeType, null));            

            callback?.Invoke();
        }

        public IEnumerator coFadeIn(SpriteRenderer spriteRenderer, ChangeType changeType, Action callback)
        {
            Color color = spriteRenderer.color;
            yield return StartCoroutine(coChangeValue(0.0f, 1.0f, changeType, (value, done) =>
            {
                color.a = value;
                spriteRenderer.color = color;
            }));

            callback?.Invoke();
        }

        public IEnumerator coFadeIn(List<SpriteRenderer> spriteRenderers, ChangeType changeType, Action callback)
        {
            if (0 < spriteRenderers.Count)
            {
                yield return StartCoroutine(coChangeValue(1.0f, 0.0f, changeType, (value, done) =>
                {
                    for (int i = 0; i < spriteRenderers.Count; ++i)
                    {
                        SpriteRenderer sr = spriteRenderers[i];
                        if (null != sr)
                            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, value);
                    }
                }));
            }

            callback?.Invoke();
        }

        public IEnumerator coFadeOut(SpriteRenderer spriteRenderer, ChangeType changeType, Action callback)
        {
            Color color = spriteRenderer.color;
            yield return StartCoroutine(coChangeValue(0.0f, 1.0f, changeType, (value, done) =>
            {
                color.a = value;
                spriteRenderer.color = color;
            }));

            callback?.Invoke();
        }

        public IEnumerator coFadeOut(List<SpriteRenderer> spriteRenderers, ChangeType changeType, Action callback)
        {
            if (0 < spriteRenderers.Count)
            {
                yield return StartCoroutine(coChangeValue(0.0f, 1.0f, changeType, (value, done) =>
                {
                    for (int i = 0; i < spriteRenderers.Count; ++i)
                    {
                        SpriteRenderer sr = spriteRenderers[i];
                        if (null != sr)
                            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, value);
                    }
                }));
            }

            callback?.Invoke();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coolTime"></param>
        /// <param name="callback">isEnd, second</param>
        public void updateCoolTime(long coolTime, bool isOnSecondWait, Action<bool, double> callback)
        {
            if (Logx.isActive)
                Logx.assert(0 <= coolTime, "Invalid coolTime {0}", coolTime);

            DateTime coolTimeDate = new DateTime(coolTime);
            StartCoroutine(coUpdateCoolTime(coolTimeDate, isOnSecondWait, callback));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coolTime"></param>
        /// <param name="callback">isEnd, second</param>
        public void updateCoolTime(DateTime coolTimeDate, bool isOnSecondWait, Action<bool, double> callback)
        {
            StartCoroutine(coUpdateCoolTime(coolTimeDate, isOnSecondWait, callback));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coolTimeDate"></param>
        /// <param name="callback">isEnd, second</param>
        public IEnumerator coUpdateCoolTime(DateTime coolTimeDate, bool isOnSecondWait, Action<bool, double> callback)
        {
            var wfs = new WaitForSeconds(1.0f);

            while (true)
            {
                TimeSpan span = coolTimeDate - DateTime.Now;
                var second = span.TotalSeconds;
                if (0 >= second)
                    break;

                callback?.Invoke(false, second);

                if (isOnSecondWait)
                    yield return wfs;
                else
                    yield return null;
            }

            callback?.Invoke(true, 0);
        }

        /// <summary>
        /// z축을 기준으로 흔들 흔들
        /// </summary>
        /// <param name="callback">rotation z angle</param>
        public IEnumerator coRattling(fMinMax startAngleRange, fMinMax destAngleRange, float t, int count, float delay, Action<float> callback)
        {
            float startAngle = startAngleRange.value;
            float destAngle = destAngleRange.value;

            for (int i = 0; i < count; ++i)
            {
                yield return StartCoroutine(coRattling(startAngle, destAngle, t, callback));
                SystemHelper.swap(ref startAngleRange, ref destAngleRange);

                startAngle = destAngle;
                destAngle = destAngleRange.value;
            }

            if (0.0f < delay)
                yield return new WaitForSeconds(delay);
        }

        /// <param name="callback">rotation z angle</param>
        IEnumerator coRattling(float startAngle, float destAngle, float t, Action<float> callback)
        {
            float elapsedTime = 0.0f;
            while (elapsedTime < t)
            {
                elapsedTime = Mathf.Min(elapsedTime + TimeHelper.deltaTime, t);

                float a = elapsedTime / t;
                float angle = Mathf.Lerp(startAngle, destAngle, a);

                callback?.Invoke(angle);

                yield return null;
            }
        }

        public IEnumerator coUpdateTime(float updateTime, Action<float> callback)
        {
            float elapsedTime = 0.0f;
            while (elapsedTime < updateTime)
            {
                elapsedTime += TimeHelper.smoothDeltaTime;
                if (updateTime < elapsedTime)
                    elapsedTime = updateTime;

                float t = elapsedTime / updateTime;
                callback(t);

                yield return null;
            }
        }

        public IEnumerator coUpdateUnscaledTime(float updateTime, Action<float> callback)
        {
            float elapsedTime = 0.0f;
            while (elapsedTime < updateTime)
            {
                elapsedTime += TimeHelper.unscaledDeltaTime;
                if (updateTime < elapsedTime)
                    elapsedTime = updateTime;

                float t = elapsedTime / updateTime;
                callback(t);

                yield return null;
            }
        }

        public void start(ref Data data)
        {
            data.coroutine = StartCoroutine(data.enumerator);
        }

        public Data start(IEnumerator enumerator)
        {
            Data data = new Data
            {
                enumerator = enumerator
            };

            start(ref data);
            return data;
        }

        public void stop(ref Data data)
        {
            if (null == data.coroutine)
                return;

            StopCoroutine(data.coroutine);
            data.coroutine = null;
        }

        private void setParticleStartColor(List<ParticleSystem> particleSystems, float alpha)
        {
            foreach (var ps in particleSystems)
            {
                var main = ps.main;
                var startColor = main.startColor;

                var minColor = main.startColor.colorMin;
                minColor.a = alpha;
                startColor.colorMin = minColor;

                var maxColor = main.startColor.colorMax;
                maxColor.a = alpha;
                startColor.colorMax = maxColor;

                main.startColor = startColor;
            }
        }

        public IEnumerator coParticleFadeOut(List<ParticleSystem> particleSystems, float fadeOutTime, Action callback)
        {
            var elapsedTime = 0.0f;
            while (elapsedTime < fadeOutTime)
            {
                elapsedTime += Time.deltaTime;
                var t = Mathf.Min(1.0f, elapsedTime / fadeOutTime);
                setParticleStartColor(particleSystems, 1.0f - t);

                yield return null;
            }

            callback?.Invoke();
        }

        public IEnumerator coParticleFadeIn(List<ParticleSystem> particleSystems, float fadeInTime, Action callback)
        {
            var elapsedTime = 0.0f;
            while (elapsedTime < fadeInTime)
            {
                elapsedTime += Time.deltaTime;
                var t = Mathf.Min(elapsedTime / fadeInTime, 1.0f);
                setParticleStartColor(particleSystems, 1.0f - t);

                yield return null;
            }

            callback?.Invoke();
        }

        /// <param name="callback">animation curve value, is done</param>
        public IEnumerator coAnimationCurve(AnimationCurve animationCurve, ChangeType changeType, Action<float, bool> callback)
        {
            if (0.0f < changeType.value)
            {
                float changeTime;
                if (ChangeType.eType.Time == changeType.type)
                {
                    changeTime = changeType.value;
                }
                else
                {
                    changeTime = 1.0f / changeType.value;
                }

                float elapsedTime = 0.0f;
                while (elapsedTime < changeTime)
                {
                    elapsedTime += changeType.isUnscaled ? TimeHelper.unscaledDeltaTime : TimeHelper.deltaTime;
                    float t = Mathf.Min(elapsedTime / changeTime, 1.0f);
                    var v = animationCurve.Evaluate(t);

                    callback(v, false);

                    yield return null;
                }
            }

            callback(animationCurve.Evaluate(1.0f), true);
        }

        public void wait(float delay, Action callback)
        {
            StartCoroutine(coWait(delay, callback));
        }

        IEnumerator coWait(float delay, Action callback)
        {
            yield return new WaitForSeconds(delay);

            callback();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                StopAllCoroutines();
            }
        }
    }
}