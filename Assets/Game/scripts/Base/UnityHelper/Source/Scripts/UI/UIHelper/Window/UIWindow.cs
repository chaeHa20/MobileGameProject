namespace UnityHelper
{
    public class UIWindowData : UIWidgetData
    {
        public enum eAddPosition { First, Last }
        public enum eInactiveCurrent
        {
            None,
            Lazy,
            Immediately
        }

        public eAddPosition addPosition = eAddPosition.First;
        public eInactiveCurrent inactiveCurrent = eInactiveCurrent.Immediately;
        /// <summary>
        /// 자동으로 현재 윈도우가 설정 된다.
        /// </summary>
        public bool isMsgBox = false;
        public bool isDontDestroy = false;
    }

    public class UIWindow : UIWidget
    {
        private bool m_isClosing = false;
        private UIWindowData.eInactiveCurrent m_inactiveCurrent = UIWindowData.eInactiveCurrent.Immediately;

        public bool isClosing { get { return m_isClosing; } }

        public override void initialize(UIWidgetData data)
        {
            initMessage();

            base.initialize(data);

            var d = data as UIWindowData;
            m_inactiveCurrent = d.inactiveCurrent;
        }

        protected virtual void initMessage()
        {

        }

        public override void open()
        {
            base.open();
        }

        public override void onClose()
        {
            m_isClosing = true;
            StopAllCoroutines();

            UIHelper.instance.disposeWindow(name);
        }

        public void onCloseImmediately()
        {
            m_isClosing = true;
            StopAllCoroutines();

            UIHelper.instance.disposeWindow(name);
        }

        public virtual void resume(UIWindowData data)
        {
            gameObject.SetActive(true);

            // onAnimationOpenEndEvent가 호출이 안되서 block이 안풀린다. 우선 주석처리,
            // 왜 onAnimationOpenEndEvent 호출이 안되지
            /*
            if (isAnimator)
            {
                if (!isMotion((int)eMotion.Open))
                {
                    setActiveBlock(true);
                    setMotion((int)eMotion.Open);
                }
            }
            */
        }

        public virtual void suspend()
        {

        }

        /// <summary>
        /// close 될 때 navigation에 남아 있거나 isDontDestroy 상태 일 경우에 destroy 되지 않고 keep 함수가 호출 된다.
        /// </summary>
        public virtual void keep()
        {
            gameObject.SetActive(false);
        }

        public override void onAnimationOpenEndEvent()
        {
        }

        public override void onAnimationCloseEndEvent()
        {
            UIHelper.instance.disposeWindow(name);
        }

        public virtual void back()
        {
            onClose();
        }
    }
}