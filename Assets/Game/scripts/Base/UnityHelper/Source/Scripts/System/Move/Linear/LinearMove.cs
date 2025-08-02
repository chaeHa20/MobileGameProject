using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public class LinearMove : BaseMove
    {
        protected float m_arriveEpsilon = 0.5f;
        protected MoveHelper.eFixAxis m_fixAxis = MoveHelper.eFixAxis.None;
        protected float m_decelerationBound = 2.0f;

        public float arriveEpsilon { set { m_arriveEpsilon = value; } }
        public float decelerationBound { set { m_decelerationBound = value; } }        
        public MoveHelper.eFixAxis fixAxis { set { m_fixAxis = value; } }

        protected void getMoveDir(Vector3 movePosition, out Vector3 moveDir, out float moveLen)
        {
            MoveHelper.getMoveDir(m_moveObject.transform.position, movePosition, m_fixAxis, out moveDir, out moveLen);
        }

        protected bool setDeceleration(float moveLen, ref float speed)
        {
            if (m_decelerationBound >= moveLen)
            {
                speed = m_speed * (moveLen / m_decelerationBound);
                if (0.0f >= speed)
                    return false;
            }

            return true;
        }

        protected bool isArrive(float moveLen)
        {
            return m_arriveEpsilon >= moveLen;
        }

        /// <param name="callback">t, end</param>
        /// <returns></returns>
        protected IEnumerator coLerpMove(Vector3 startPosition, Vector3 destPosition, float speed, GameObject moveObject, Action<float, bool> callback)
        {
            Vector3 moveLength = destPosition - startPosition;
            float moveTime = moveLength.magnitude / speed;
            float elapsedTime = 0.0f;
            while (elapsedTime < moveTime)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / moveTime;
                if (1.0f < t)
                    t = 1.0f;

                moveObject.transform.position = Vector3.Lerp(startPosition, destPosition, t);

                callback?.Invoke(t, false);

                yield return null;
            }

            callback?.Invoke(1.0f, true);
        }

        static public IEnumerator coSmoothDampMove(Vector3 startPosition, Vector3 destPosition, float smoothTime, GameObject moveObject, Action callback)
        {
            Vector3 moverPosition = startPosition;
            Vector3 smoothVelocity = Vector3.zero;
            while (true)
            {
                moverPosition = Vector3.SmoothDamp(moverPosition, destPosition, ref smoothVelocity, smoothTime);
                moveObject.transform.position = moverPosition;

                Vector3 length = destPosition - moverPosition;
                if (0.001f >= length.sqrMagnitude)
                    break;

                yield return null;
            }

            callback?.Invoke();
        }

        static public IEnumerator coSlerpRotation(Quaternion startRotation, Quaternion destRotation, float rotationSmooth, GameObject moveObject, Action callback)
        {
            Quaternion rotation = startRotation;
            while (true)
            {
                float t = Time.deltaTime * rotationSmooth;
                rotation = Quaternion.Slerp(rotation, destRotation, t);
                moveObject.transform.rotation = rotation;

                float angle = Quaternion.Angle(rotation, destRotation);
                if (0.0f >= angle)
                    break;

                yield return null;
            }

            moveObject.transform.rotation = destRotation;

            callback?.Invoke();
        }
    }
}