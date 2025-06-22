using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class GameCoroutineHelper : CoroutineHelper
{
    public static GameCoroutineHelper getInstance()
    {
        return getInstance<GameCoroutineHelper>();
    }

    private Vector3 getMovePosition(in Vector3 start, in Vector3 dest, AnimationCurve curve, float t)
    {
        var e = curve.Evaluate(t);
        return Vector3.Lerp(start, dest, e);
    }

    /// <param name="updateCallback">time</param>
    public IEnumerator coMoveToTarget(Entity source, Transform target, AnimationCurve moveCurve, float moveTime,
                                             bool isLocal, bool isSetModelForward, float forwardingSpeed,
                                             Action<float> updateCallback, Action endCallback)
    {
        var curPosition = isLocal ? source.localPosition3 : source.position3;
        var targetPosition = isLocal ? target.localPosition : target.position;

        // 전체 커브 시간이 1이 아닐 수도 있어서 
        var totalMoveCurveTime = moveCurve.keys[moveCurve.length - 1].time;
        var targetModelForward = targetPosition - curPosition;
        targetModelForward.Normalize();

        int currentKeyIndex = 0;

        var currentKeyTime = moveCurve.keys[currentKeyIndex].time;
        var nextKeyTime = moveCurve.keys[currentKeyIndex + 1].time;

        var elapsedTime = 0.0f;
        while (elapsedTime < moveTime)
        {
            elapsedTime += Time.deltaTime;
            var t = Mathf.Min(1.0f, elapsedTime / moveTime) * totalMoveCurveTime;
            if (isLocal)
                source.localPosition3 = getMovePosition(in curPosition, in targetPosition, moveCurve, t);
            else
                source.position3 = getMovePosition(in curPosition, in targetPosition, moveCurve, t);

            if (isSetModelForward)
                source.setForward(Vector3.Lerp(source.forward, targetModelForward, Time.deltaTime * forwardingSpeed));

            var oldKeyIndex = currentKeyIndex;
            if (moveCurve.length > currentKeyIndex + 1)
            {
                if (moveCurve.keys[currentKeyIndex + 1].time <= t)
                {
                    ++currentKeyIndex;

                    currentKeyTime = nextKeyTime;
                    if (moveCurve.length <= currentKeyIndex + 1)
                        nextKeyTime = 1.0f;
                    else
                        nextKeyTime = moveCurve.keys[currentKeyIndex + 1].time;
                }
            }

            updateCallback?.Invoke(t);

            yield return null;
        }

        endCallback();
    }

    public IEnumerator coMoveToDest(Transform moveTransform, Vector3 destPosition, float moveSpeed, Action callback)
    {
        var curPosition = moveTransform.position;
        var moveDir = destPosition - curPosition;
        var distance = moveDir.magnitude;
        moveDir.Normalize();

        var elapsedLength = 0.0f;
        while (true)
        {
            var s = moveSpeed * TimeHelper.deltaTime;
            elapsedLength += s;

            if (elapsedLength >= distance)
            {
                moveTransform.position = destPosition;
                break;
            }
            else
            {
                moveTransform.position += s * moveDir;
            }

            yield return null;
        }

        callback?.Invoke();
    }

    public IEnumerator coTurnInOppositeDirection(Transform trans, float turnTime)
    {
        var changeType = createTimeType(turnTime);
        var curEulerAngles = trans.rotation.eulerAngles;
        var oppositeEulerAngles = curEulerAngles;
        oppositeEulerAngles.y -= 180.0f;

        yield return StartCoroutine(coChangeVector(curEulerAngles, oppositeEulerAngles, changeType, (value, done) =>
        {
            trans.rotation = Quaternion.Euler(value);
        }));
    }

    public Coroutine playLoadGauge(UIGauge gauge, float loadTime, Action callback)
    {
        gauge.gameObject.SetActive(true);
        gauge.setValue(0.0f, 1.0f);

        var changeType = createTimeType(loadTime);
        var coroutine = StartCoroutine(coChangeValue(0.0f, 1.0f, changeType, (value, done) =>
        {
            if (null == gauge)
                return;

            gauge.setValue(value);

            if (done)
            {
                gauge.gameObject.SetActive(false);
                callback?.Invoke();
            }
        }));

        return coroutine;
    }

    public void waitOneFrame(Action callback)
    {
        StartCoroutine(coWait(callback));
    }

    public IEnumerator coWait(Action callback)
    {
        yield return null;
        callback?.Invoke();
    }
}
