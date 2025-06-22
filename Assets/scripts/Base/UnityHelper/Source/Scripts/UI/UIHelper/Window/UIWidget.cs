using System;
using System.Collections;
using UnityEngine;

namespace UnityHelper
{
    public enum eUIRestaurantLayer
    {
        Hud = 0,
        Bottom = 1,
        Main = 2,
        OverCurrency = 3,
        Block = 4,
    }

    public class UIWidgetData
    {
        public string name = null;
        // 만약 0보다 작으면 현재 열려있는 윈도우(또는 위젯과 같은 레이어를 사용한다.)
        public int layer = -1;
        // null이면 자동 설정된다.
        public GameObject parent = null;
        public string resPath = null;
        public Action destroyCallback;
        public SetParentOption setParentOption = SetParentOption.fullAndReset();
    }

    public class UIWidget : UIComponent
    {
        private Action m_destroyCallback = null;
        private int m_layer = 0;

        public int layer => m_layer;

        public virtual void initialize(UIWidgetData data)
        {
            m_destroyCallback = data.destroyCallback;
            m_layer = data.layer;
        }

        /// <summary>
        /// 초기화 후에 설정 되는 경우도 있어서 따로 함수를 만듬
        /// </summary>
        public void setDestroyCallback(Action destroyCallback)
        {
            m_destroyCallback = destroyCallback;
        }

        public virtual void open()
        {
            gameObject.SetActive(true);
        }

        public virtual void onClose()
        {
        }

        public virtual void onClose(float delay)
        {
            if (0.0f < delay)
            {
                StartCoroutine(coClose(delay));
            }
            else
            {
                onClose();
            }
        }

        IEnumerator coClose(float delay)
        {
            yield return new WaitForSeconds(delay);   

            onClose();
        }

        public virtual void refresh()
        {

        }

        public virtual void onAnimationOpenEndEvent()
        {

        }

        public virtual void onAnimationActiveContentEvent()
        {
        }

        public virtual void onAnimationCloseEndEvent()
        {

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                m_destroyCallback?.Invoke();

                GameObject.Destroy(gameObject);
            }
        }
    }
}