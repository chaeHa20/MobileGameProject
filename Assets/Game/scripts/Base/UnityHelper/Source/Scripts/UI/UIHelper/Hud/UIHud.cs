using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public class UIHud : UIComponent
    {
        [SerializeField] Vector3 m_localOffset = Vector3.zero;

        private GameObject m_positionObject = null;
        private Coroutine m_coSyncPosition = null;

        protected GameObject positionObject => m_positionObject;

        /// <param name="camera">main camera</param>
        public void initialize(GameObject positionObject, Canvas canvas, Camera camera, bool isCheckParentBound = false)
        {   
            startSyncPosition(positionObject, canvas, camera, isCheckParentBound);

            UIHelper.instance.hudHelper.regist(this);
        }

        protected void startSyncPosition(GameObject positionObject, Canvas canvas, Camera camera, bool isCheckParentBound)
        {
            stopSyncPosition();

            if (null != positionObject)
            {
                m_positionObject = positionObject;
                transform.localScale = positionObject.transform.localScale;

                m_coSyncPosition = StartCoroutine(coSyncPosition(positionObject, canvas, camera, isCheckParentBound));
            }
        }

        IEnumerator coSyncPosition(GameObject positionObject, Canvas canvas, Camera camera, bool isCheckParentBound)
        {
            RectTransform rtCanvas = canvas.GetComponent<RectTransform>();
            RectTransform rt = GetComponent<RectTransform>();
            var halfSize = rt.sizeDelta * 0.5f;

            var parentRt = transform.parent.GetComponent<RectTransform>();
            var parentHalfSize = parentRt.rect.size * 0.5f;

            while (true)
            {
                if (null == positionObject)
                    break;

                var worldPosition = positionObject.transform.position;

                UIHelper.instance.worldToCanvasPosition(camera, rtCanvas, worldPosition, rt);

                transform.localPosition = transform.localPosition + m_localOffset;

                if (isCheckParentBound)
                {
                    checkParentBound(ref halfSize, ref parentHalfSize);
                }

                updateSyncPosition(positionObject);

                yield return null;
            }

            m_coSyncPosition = null;
        }

        /// <summary>
        /// // pivot은 0.5, 0.5라고 여기자
        /// </summary>
        private void checkParentBound(ref Vector2 halfSize, ref Vector2 parentHalfSize)
        {
            bool isUpdate = false;
            var localPosition = transform.localPosition;

            if (localPosition.x - halfSize.x < -parentHalfSize.x)
            {
                localPosition.x = -parentHalfSize.x + halfSize.x;
                isUpdate = true;
            }
            else if (localPosition.x + halfSize.x > parentHalfSize.x)
            {
                localPosition.x = parentHalfSize.x - halfSize.x;
                isUpdate = true;
            }

            if (localPosition.y - halfSize.y < -parentHalfSize.y)
            {
                localPosition.y = -parentHalfSize.y + halfSize.y;
                isUpdate = true;
            }
            else if (localPosition.y + halfSize.y > parentHalfSize.y)
            {
                localPosition.y = parentHalfSize.y - halfSize.y;
                isUpdate = true;
            }

            if (isUpdate)
                transform.localPosition = localPosition;
        }

        public void stopSyncPosition()
        {
            if (null != m_coSyncPosition)
            {
                StopCoroutine(m_coSyncPosition);
                m_coSyncPosition = null;
            }
        }

        protected virtual void updateSyncPosition(GameObject positionObject)
        {

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                StopAllCoroutines();
                UIHelper.instance.hudHelper.unregist(this);
            }
        }
    }
}