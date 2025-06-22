using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityHelper
{
    [Serializable]
    public struct SetParentOption
    {
        public bool isFullScreen;
        public bool resetLocalPosition;
        public bool resetLocalScale;
        public bool resetLocalRotation;

        public static SetParentOption create(bool _isFullScreen, bool _resetLocalPosition, bool _resetLocalScale, bool _resetLocalRotation)
        {
            SetParentOption option = new SetParentOption
            {
                isFullScreen = _isFullScreen,
                resetLocalPosition = _resetLocalPosition,
                resetLocalScale = _resetLocalScale,
                resetLocalRotation = _resetLocalRotation
            };
            return option;
        }

        public static SetParentOption notFullAndNotReset()
        {
            return create(false, false, false, false);
        }

        public static SetParentOption notFullAndReset()
        {
            return create(false, true, true, true);
        }

        public static SetParentOption fullAndReset()
        {
            return create(true, true, true, true);
        }
    }

    public class UIHelper : MonoSingleton<UIHelper>, IDisposable
    {
        private class Root
        {
            public Canvas canvas = null;
            public Dictionary<int, GameObject> safeAreas = null;
            public int lastLayer = 0;
        }

        private UIWindowHelper m_windowHelper = null;
        private UIPanelHelper m_panelHelper = null;
        private UIHudHelper m_hudHelper = null;
        private UICanvasGroup m_canvasGroup = null;
        private Camera m_uiCamera = null;
        private UIHelperMsg m_msg = new UIHelperMsg();

        public bool isNotShowPauseMsg { private get; set; }
        public int windowCount { get { return m_windowHelper.instanceCount; } }
        public int mainWindowCount { get { return m_windowHelper.mainInstanceCount; } }
        public UIHudHelper hudHelper { get { return m_hudHelper; } }
        public Camera uiCamera => m_uiCamera;
        public UICanvasGroup canvasGroup => m_canvasGroup;
        public Action openWindowCallback { get => m_windowHelper.openCallback; set => m_windowHelper.openCallback = value; }
        public Action disposeWindowCallback { get => m_windowHelper.disposeCallback; set => m_windowHelper.disposeCallback = value; }

        protected override void Awake()
        {
            base.Awake();

            createWindowHelper();
            createPanelHelper();
            createHudHelper();
        }

        protected virtual void createWindowHelper()
        {
            m_windowHelper = new UIWindowHelper();
        }

        protected virtual void createPanelHelper()
        {
            m_panelHelper = new UIPanelHelper();
        }

        protected virtual void createHudHelper()
        {
            m_hudHelper = new UIHudHelper();
        }

        public void initCanvas(UICanvasGroup canvasGroup)
        {
            setCanvasGroup(canvasGroup);
            setUICamera();
        }

        public virtual void initScene()
        {

        }

        private void setCanvasGroup(UICanvasGroup canvasGroup)
        {
            m_canvasGroup = canvasGroup;
            m_canvasGroup.initialize();
        }

        private Root findRoot(string tag)
        {
            Root root = new Root();
            GameObject obj = GameObject.FindGameObjectWithTag(tag);
            if (null == obj)
            {
                if (Logx.isActive)
                    Logx.warn("Failed find tag {0} object", tag);
                return root;
            }

            root.canvas = obj.GetComponent<Canvas>();

            var safeAreas = obj.transform.GetComponentsInChildren<Crystal.SafeArea>();
            if (null != safeAreas)
            {
                root.safeAreas = new Dictionary<int, GameObject>();
                for (int layer = 0; layer < safeAreas.Length; ++layer)
                {
                    root.safeAreas.Add(layer, safeAreas[layer].gameObject);
                    if (layer > root.lastLayer)
                        root.lastLayer = layer;
                }
            }

            return root;
        }

        private void setUICamera()
        {
            GameObject obj = GameObject.FindGameObjectWithTag(Tag.UICamera);
            if (null == obj)
                obj = GameObject.FindGameObjectWithTag(Tag.MainCamera);

            if (null == obj)
                return;

            m_uiCamera = obj.GetComponent<Camera>();
        }

        public W openWindow<W>(UIWindowData data, bool isMain) where W : UIWindow
        {
            if (0 > data.layer)
            {
                var currentWindow = getCurrentWindow<UIWindow>();
                if (null != currentWindow)
                    data.layer = currentWindow.layer;
            }

            data.parent = data.parent ?? m_canvasGroup.getSafeArea(data.layer);
            if (null == data.parent)
                return null;

            return m_windowHelper.open<W>(data, isMain);
        }

        public W openPanel<W>(UIPanelData data) where W : UIPanel
        {
            if (Logx.isActive)
                Logx.assert(null != data, "data is null");

            if (0 > data.layer)
            {
                var currentPanel = getCurrentPanel<UIPanel>();
                if (null != currentPanel)
                    data.layer = currentPanel.layer;
            }

            data.parent = data.parent ?? m_canvasGroup.getSafeArea(data.layer);
            if (null == data.parent)
                return null;

            return m_panelHelper.open<W>(data);
        }

        public W getWindow<W>(string name) where W : UIWindow
        {
            return m_windowHelper.get<W>(name);
        }

        public W getCurrentWindow<W>() where W : UIWindow
        {
            return m_windowHelper.getCurrent<W>();
        }

        public bool isCurrentWindow(string name)
        {
            return m_windowHelper.isCurrent(name);
        }

        public bool isCurrentMsgBox()
        {
            return m_windowHelper.isCurrentMsgBox();
        }

        public bool isOpenWindow(string name)
        {
            return m_windowHelper.isOpen(name);
        }

        public void disposeWindow(string name)
        {
            m_windowHelper.dispose(name);
        }

        public void disposeCurrentWindow()
        {
            m_windowHelper.disposeCurrent();
        }

        public void closeWindow(string name, float delay)
        {
            m_windowHelper.close(name, delay);
        }

        public void closeCurrentWindow(float delay)
        {
            m_windowHelper.closeCurrent(delay);
        }

        public void closeUntilTargetWindow(string name)
        {
            m_windowHelper.closeUntilTarget(name);
        }

        public void closeAllWindow()
        {
            m_windowHelper.closeAll();
        }

        public W getPanel<W>(string name) where W : UIPanel
        {
            return m_panelHelper.get<W>(name);
        }

        public W getCurrentPanel<W>() where W : UIPanel
        {
            return m_panelHelper.getCurrent<W>();
        }

        public void closePanel(string name)
        {
            m_panelHelper.close(name);
        }

        public bool existPanel(string name)
        {
            return m_panelHelper.exist(name);
        }

        public void setParent(GameObject parent, GameObject child, SetParentOption option)
        {
            child.transform.SetParent(parent.transform);

            if (option.resetLocalPosition)
                child.transform.localPosition = Vector3.zero;
            if (option.resetLocalScale)
                child.transform.localScale = Vector3.one;
            if (option.resetLocalRotation)
                child.transform.localRotation = Quaternion.identity;

            if (option.isFullScreen)
                setFullScreen(child);
        }



        public void setFullScreen(GameObject obj)
        {
            if (Logx.isActive)
                Logx.assert(null != obj, "Invalid UIHelper.setFullScreen parameter, obj is null");

            RectTransform rt = obj.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMax = Vector2.zero;
            rt.offsetMin = Vector2.zero;
        }

        public virtual bool backWindow()
        {
            return m_windowHelper.back();
        }

        public bool existWindow(string name)
        {
            return m_windowHelper.exist(name);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_msg.Dispose();
                m_windowHelper.Dispose();
                m_panelHelper.Dispose();
                m_hudHelper.Dispose();
            }
        }

        protected bool checkAndClearNotShowPauseFlag()
        {
            if (isNotShowPauseMsg)
            {
                isNotShowPauseMsg = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        protected virtual void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                if (checkAndClearNotShowPauseFlag())
                {
                    return;
                }

                openPauseMsgBox();
            }
        }

        protected virtual void openPauseMsgBox()
        {

        }

        public void fitAspectRatioWithScreen(RectTransform rt)
        {
            if (Logx.isActive)
                Logx.assert(null != rt, "rt is null");

            Vector2 rtSizeDelta = rt.sizeDelta;
            float imageRate = rt.sizeDelta.x / rt.sizeDelta.y;
            float screenRate = (float)Screen.width / (float)Screen.height;
            float widthRate = screenRate / imageRate;
            rtSizeDelta.x *= widthRate;
            rt.sizeDelta = rtSizeDelta;
        }

        /// <summary>
        /// https://answers.unity.com/questions/921726/how-to-get-the-size-of-a-unityengineuitext-for-whi.html
        /// 잘 안될 때는 여길 참고 -> https://answers.unity.com/questions/898770/how-to-get-the-width-of-ui-text-with-horizontal-ov.html
        /// 
        /// </summary>
        public Vector2 getTextPreferredSize(Text text, string str, Vector2 margin)
        {
            TextGenerator textGen = new TextGenerator();
            TextGenerationSettings genSettings = text.GetGenerationSettings(text.rectTransform.rect.size);
            float width = textGen.GetPreferredWidth(str, genSettings);
            float height = textGen.GetPreferredHeight(str, genSettings);

            width = Mathf.Min(width, text.rectTransform.sizeDelta.x) + margin.x;
            height = Mathf.Min(height, text.rectTransform.sizeDelta.y) + margin.y;

            return new Vector2(width, height);
        }

        public void worldToCanvasPosition(Camera camera, RectTransform rtCanvas, Vector3 worldPosition, RectTransform setObject)
        {
            if (Logx.isActive)
            {
                Logx.assert(null != camera, "camera is null");
                Logx.assert(null != rtCanvas, "rtCanvas is null");
                Logx.assert(null != setObject, "setObject is null");
            }

            setObject.anchoredPosition = worldToCanvasPosition(camera, rtCanvas, worldPosition);
        }

        public Vector2 worldToCanvasPosition(Camera camera, RectTransform rtCanvas, Vector3 worldPosition)
        {
            if (Logx.isActive)
            {
                Logx.assert(null != camera, "camera is null");
                Logx.assert(null != rtCanvas, "rtCanvas is null");
            }

            var viewportPoint = camera.WorldToViewportPoint(worldPosition);

            Vector2 screenPoint = new Vector2((viewportPoint.x * rtCanvas.sizeDelta.x) - (rtCanvas.sizeDelta.x * rtCanvas.pivot.x),
                                              (viewportPoint.y * rtCanvas.sizeDelta.y) - (rtCanvas.sizeDelta.y * rtCanvas.pivot.y));

            // 흠,,잘 안 맞는다..
            if (0.0f > viewportPoint.z)
            {
                screenPoint.x *= -5.0f;
                screenPoint.y *= -5.0f;
            }

            return screenPoint;
        }
        /// <summary>
        /// https://gist.github.com/FlaShG/ac3afac0ef65d98411401f2b4d8a43a5
        /// </summary>
        public Vector3 screenToCanvasPosition(RectTransform rtCanvas, Vector3 screenPosition)
        {
            if (Logx.isActive)
                Logx.assert(null != rtCanvas, "rtCanvas is null");

            var viewportPosition = new Vector3(screenPosition.x / Screen.width,
                                               screenPosition.y / Screen.height,
                                               0);
            return viewportToCanvasPosition(rtCanvas, viewportPosition);
        }

        /// <summary>
        /// https://gist.github.com/FlaShG/ac3afac0ef65d98411401f2b4d8a43a5
        /// </summary>
        public Vector3 viewportToCanvasPosition(RectTransform rtCanvas, Vector3 viewportPosition)
        {
            if (Logx.isActive)
                Logx.assert(null != rtCanvas, "rtCanvas is null");

            var centerBasedViewPortPosition = viewportPosition - new Vector3(0.5f, 0.5f, 0);
            var scale = rtCanvas.sizeDelta;
            return Vector3.Scale(centerBasedViewPortPosition, scale);
        }

        /// <summary>
        /// 명시적으로 unregistMessage를 꼭 호출해줘야 된다.
        /// </summary>
        public void registMessage(int messageId, UIComponentCallback callback)
        {
            m_msg.regist(messageId, callback);
        }

        public void unregistMessage(int messageId, UIComponentCallback callback)
        {
            m_msg.unregist(messageId, callback);
        }

        public void sendMessage(int messageId)
        {
            //if (Logx.isActive)
            //    Logx.trace("UI Send Msg {0}", messageIdToStr(messageId));

            m_msg.send(messageId);
        }

        public void sendMessage(int messageId, object data)
        {
            //if (Logx.isActive)
            //    Logx.trace("UI Send Msg {0}", messageIdToStr(messageId));

            m_msg.send(messageId, data);
        }

        public void sendMessage(int messageId, object data1, object data2)
        {
            //if (Logx.isActive)
            //    Logx.trace("UI Send Msg {0}", messageIdToStr(messageId));

            m_msg.send(messageId, data1, data2);
        }

        public void sendMessage(int messageId, object data1, object data2, object data3)
        {
            //if (Logx.isActive)
            //    Logx.trace("UI Send Msg {0}", messageIdToStr(messageId));

            m_msg.send(messageId, data1, data2, data3);
        }

        public void sendMessage(int messageId, object data1, object data2, object data3, object data4)
        {
            //if (Logx.isActive)
            //    Logx.trace("UI Send Msg {0}", messageIdToStr(messageId));

            m_msg.send(messageId, data1, data2, data3, data4);
        }

        public void sendMessage(int messageId, List<object> datas)
        {
            //if (Logx.isActive)
            //    Logx.trace("UI Send Msg {0}", messageIdToStr(messageId));

            m_msg.send(messageId, datas);
        }

        public static EventTrigger.Entry addEventTrigger(EventTrigger trigger, EventTriggerType eventID, Action listener)
        {
            var entry = new EventTrigger.Entry();
            entry.eventID = eventID;
            entry.callback.AddListener((eventData) => { listener(); });
            trigger.triggers.Add(entry);

            return entry;
        }

        public static void removeEventTrigger(EventTrigger trigger, EventTrigger.Entry entry)
        {
            trigger.triggers.Remove(entry);
        }

        protected virtual string messageIdToStr(int messageId)
        {
            return messageId.ToString();
        }
    }
}