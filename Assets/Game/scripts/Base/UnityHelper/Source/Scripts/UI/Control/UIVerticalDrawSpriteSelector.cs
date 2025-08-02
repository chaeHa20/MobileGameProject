using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace UnityHelper
{
    public class UIVerticalDrawSpriteSelector : UIComponent
    {
        private enum eStep { None, Acceleration, Constant, Deceleration, Select }

        private class Items
        {
            private int m_popIndex = 0;
            private List<Sprite> m_sprites = new List<Sprite>();

            public void setSprites(List<Sprite> sprites)
            {
                m_sprites.Clear();

                foreach(var sprite in sprites)
                {
                    m_sprites.Add(sprite);
                }
            }

            public Sprite peek()
            {
                if (0 == m_sprites.Count)
                    return null;

                Sprite sprite = m_sprites[m_popIndex];

                ++m_popIndex;
                if (m_sprites.Count <= m_popIndex)
                    m_popIndex = 0;

                return sprite;
            }
        }

        [SerializeField] List<Image> m_scrollNodes = new List<Image>();
        [SerializeField] float m_initA = 2000.0f;
        [SerializeField] float m_maxV = 2000.0f;
        [SerializeField] float m_constantStepTime = 1.0f;
        [SerializeField] float m_decelerationThrashHold = 200.0f;

        private LinkedList<Image> m_scrollLinkedNodes= new LinkedList<Image>();
        private LinkedListNode<Image> m_selectScrollLinkedNode = null;
        private Vector2 m_scrollNodeSize = Vector2.zero;
        private Vector3 m_lastScrollNodePos = Vector3.zero;        
        private float m_a = 0.0f;
        private float m_v = 0.0f;
        private UpdateTimer m_constantTimer = new UpdateTimer();        
        private StateFunction m_stateFunction = new StateFunction();
        private Items m_items = new Items();
        private Sprite m_selectSprite = null;
        private Action m_selectCallback = null;

        public bool isRunning { get { return eStep.None != (eStep)m_stateFunction.curType; } }

        public void initialize(List<Sprite> sprites)
        {
            m_items.setSprites(sprites);
            makeScrollLinkedNodes();
            setIScrollNodeSize();
            initScrollNodeSprite();
            initScrollNodePositions();

            initStateFunction();
            m_stateFunction.setCase((int)eStep.None);
        }

        private void initStateFunction()
        {
            m_stateFunction.addCase((int)eStep.None, new StateFunctionCase(null, null, null));
            m_stateFunction.addCase((int)eStep.Acceleration, new StateFunctionCase(beginAcceleration, updateAcceleration, null));
            m_stateFunction.addCase((int)eStep.Constant, new StateFunctionCase(begiConstant, updateConstant, null));
            m_stateFunction.addCase((int)eStep.Deceleration, new StateFunctionCase(beginDeceleration, updateDeceleration, null));
            m_stateFunction.addCase((int)eStep.Select, new StateFunctionCase(beginSelect, updateSelect, null));
        }

        private void makeScrollLinkedNodes()
        {
            m_scrollLinkedNodes = new LinkedList<Image>();

            foreach (var node in m_scrollNodes)
            {
                m_scrollLinkedNodes.AddFirst(node);
            }
        }

        private void setIScrollNodeSize()
        {
            RectTransform rt = m_scrollLinkedNodes.First.Value.GetComponent<RectTransform>();
            m_scrollNodeSize = new Vector2(rt.rect.width, rt.rect.height);
        }

        private void initScrollNodeSprite()
        {
            var e = m_scrollLinkedNodes.GetEnumerator();
            while (e.MoveNext())
            {
                Image image = e.Current;
                image.sprite = m_items.peek();
            }
        }

        private void initScrollNodePositions()
        {
            int halfCount = m_scrollLinkedNodes.Count / 2;
            float initLocalY = (float)halfCount * m_scrollNodeSize.y;

            float localY = initLocalY;
            var e = m_scrollLinkedNodes.GetEnumerator();
            while (e.MoveNext())
            {
                Image image = e.Current;
                image.transform.localPosition = new Vector3(0.0f, localY, 0.0f);

                localY -= m_scrollNodeSize.y;
            }

            m_lastScrollNodePos = m_scrollLinkedNodes.Last.Value.transform.localPosition;
        }

        public void run(Sprite selectSprite, Action selectCallback)
        {
            m_selectSprite = selectSprite;
            m_selectCallback = selectCallback;

            m_stateFunction.setCase((int)eStep.Acceleration);
        }

        void Update()
        {
            m_stateFunction.update(TimeHelper.deltaTime);
        }

        private void beginAcceleration(params object[] args)
        {
            m_v = 0.0f;
            m_a = m_initA;
        }

        private void updateAcceleration(float dt)
        {
            m_v += m_a * dt;
            if (m_v >= m_maxV)
            {
                m_v = m_maxV;
                m_stateFunction.setCase((int)eStep.Constant);
            }

            float s = m_v * dt;

            move(s);
        }

        private void begiConstant(params object[] args)
        {
            m_constantTimer.initialize(m_constantStepTime, false);
        }

        private void updateConstant(float dt)
        {
            if (m_constantTimer.update(dt))
            {
                m_stateFunction.setCase((int)eStep.Deceleration);
            }

            float s = m_v * dt;
            move(s, true);
        }

        private void beginDeceleration(params object[] args)
        {

        }

        private void updateDeceleration(float dt)
        {
            m_v -= m_a * dt;
            if (m_decelerationThrashHold >= m_v)
            {
                m_stateFunction.setCase((int)eStep.Select);
                return;
            }

            float s = m_v * dt;

            move(s);
        }

        private void beginSelect(params object[] args)
        {
            findSelectScrollNode();
        }

        private void findSelectScrollNode()
        {
            m_selectScrollLinkedNode = null;

            float halfNodeSizeY = m_scrollNodeSize.y * 0.5f;
            var e = m_scrollLinkedNodes.GetEnumerator();
            while (e.MoveNext())
            {
                Image image = e.Current;
                float y = image.transform.localPosition.y;

                if (-halfNodeSizeY < y && halfNodeSizeY > y)
                {
                    m_selectScrollLinkedNode = m_scrollLinkedNodes.Find(image);
                    if (null != m_selectScrollLinkedNode.Previous)
                        m_selectScrollLinkedNode = m_selectScrollLinkedNode.Previous;

                    if (null != m_selectScrollLinkedNode)
                    {
                        m_selectScrollLinkedNode.Value.sprite = m_selectSprite;
                    }

                    break;
                }
            }
        }

        private void updateSelect(float dt)
        {
            if (null == m_selectScrollLinkedNode)
            {
                m_stateFunction.setCase((int)eStep.None);
                return;
            }

            float s = m_v * dt;

            float selectY = m_selectScrollLinkedNode.Value.transform.localPosition.y;
            if (0.0f >= selectY - s)
            {
                m_stateFunction.setCase((int)eStep.None);
                s = selectY;

                if (null != m_selectCallback)
                    m_selectCallback();
            }

            move(s);
        }

        private void move(float offsetY, bool changeLastSprite = false)
        {
            var e = m_scrollLinkedNodes.GetEnumerator();
            while (e.MoveNext())
            {
                Image image = e.Current;
                Vector3 localPos = image.transform.localPosition;
                localPos.y -= offsetY;


                if (localPos.y <= m_lastScrollNodePos.y)
                {
                    localPos = m_scrollLinkedNodes.First.Value.transform.localPosition;
                    localPos.y += m_scrollNodeSize.y;
                    image.transform.localPosition = localPos;

                    if (changeLastSprite)
                        image.sprite = m_items.peek();

                    m_scrollLinkedNodes.Remove(image);
                    m_scrollLinkedNodes.AddFirst(image);

                    break;
                }
                else
                {
                    image.transform.localPosition = localPos;
                }
            }
        }
    }
}