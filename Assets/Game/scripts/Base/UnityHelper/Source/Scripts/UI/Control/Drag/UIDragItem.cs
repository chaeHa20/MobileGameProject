using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityHelper
{
    public class UIDragItem : PoolObject
    {
        private RectTransform m_rtCanvas = null;

        public virtual void initialize()
        {
            ///m_rtCanvas = UIHelper.instance.mainCanvas.GetComponent<RectTransform>();
            m_rtCanvas = UIHelper.instance.canvasGroup.getLastCanvas().rtCanvas;

            //GameObject parent = UIHelper.instance.getMainLastSafeArea();
            GameObject parent = UIHelper.instance.canvasGroup.getLastCanvas().safeArea;
            UIHelper.instance.setParent(parent, gameObject, SetParentOption.notFullAndReset());

            transform.localPosition = getCanvasMousePosition();
        }

        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                transform.localPosition = getCanvasMousePosition();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Dispose();
            }
        }

        private Vector3 getCanvasMousePosition()
        {
            Vector3 screenPosition = Input.mousePosition;
            return UIHelper.instance.screenToCanvasPosition(m_rtCanvas, screenPosition);
        }
    }
}