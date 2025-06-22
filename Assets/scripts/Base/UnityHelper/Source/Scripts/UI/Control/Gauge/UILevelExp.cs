using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace UnityHelper
{
    public class UILevelExp : UIComponent
    {
        public struct Data
        {
            public LevelExp levelExp;
            public long maxExp;
        }

        [SerializeField] float m_aniTime = 0.5f;
        [SerializeField] TextSelector m_levelText = new TextSelector();
        [SerializeField] List<UIGauge> m_exp = new List<UIGauge>();

        private int m_level = 0;
        private Coroutine m_coAnimation = null;

        public Data getCurData()
        {
            Data data = new Data
            {
                levelExp = new LevelExp()
            };

            data.levelExp.level = m_level;
            if (0 < m_exp.Count)
            {
                data.levelExp.exp = (long)m_exp[0].curValue;
                data.maxExp = (long)m_exp[0].maxValue;
            }

            return data;
        }

        public Data getLastData(LevelExp levelExp, long maxExp)
        {
            Data data = new Data
            {
                levelExp = levelExp,
                maxExp = maxExp
            };

            return data;
        }

        public Data getMediumData(LevelExp levelExp)
        {
            Data data = new Data
            {
                levelExp = levelExp,
                maxExp = levelExp.exp
            };

            return data;
        }

        public void setImmediately(LevelExp levelExp, long maxExp)
        {
            setLevel(levelExp.level);
            setExp(levelExp.exp, maxExp);
        }

        public List<Data> makeDatas(LevelExp levelExp, long maxExp)
        {
            var curData = getCurData();
            var lastData = getLastData(levelExp, maxExp);

            List<Data> datas = new List<Data>();
            datas.Add(curData);

            for (int i = curData.levelExp.level + 1; i < lastData.levelExp.level; ++i)
            {
                datas.Add(getMediumData(new LevelExp(i, 100)));
            }            

            datas.Add(lastData);

            return datas;
        }

        public void setAnimation(LevelExp levelExp, long maxExp, Action callback)
        {
            var datas = makeDatas(levelExp, maxExp);
            setAnimation(datas, callback);
        }

        public void setAnimation(List<Data> datas, Action callback)
        {
            if (Logx.isActive)
                Logx.assert(0 < datas.Count, "Data is empty");

            if (null != m_coAnimation)
            {
                StopCoroutine(m_coAnimation);
                m_coAnimation = null;
            }

            if (gameObject.activeInHierarchy)
            {
            m_coAnimation = StartCoroutine(coAnimation(datas, callback));
        }
            else
            {
                Data data = datas[datas.Count - 1];
                setImmediately(data.levelExp, data.maxExp);
            }
        }

        IEnumerator coAnimation(List<Data> datas, Action callback)
        {
            Data lastData;
            if (1 < datas.Count)
                lastData = datas[datas.Count - 1];
            else
                lastData = datas[0];

            float exp = datas[0].levelExp.exp;
            for (int i = 0; i < datas.Count; ++i)
            {
                Data data = datas[i];
                setLevel(data.levelExp.level);

                long targetExp = (data.levelExp.level < lastData.levelExp.level) ? data.maxExp : data.levelExp.exp;
                float speed = calcSpeed((long)exp, targetExp, data.maxExp);

                while (exp < targetExp)
                {
                    exp += Time.deltaTime * speed;
                    if (exp > targetExp)
                    {
                        setExp(data);
                        exp = 0;
                        break;
                    }
                    else
                    {
                        setExp((long)exp, data.maxExp);
                    }
                    yield return null;
                }
            }

            setExp(lastData);

            callback?.Invoke();
        }

        private float calcSpeed(long exp, long targetExp, long maxExp)
        {
            float lastExp = targetExp - exp;
            float t = m_aniTime * (lastExp / maxExp);
            return lastExp / t;
        }

        protected virtual void setLevel(int level)
        {
            m_level = level;
            if (null != m_levelText)
                m_levelText.text = getLevelTextFormat(level);
        }

        protected virtual string getLevelTextFormat(int level)
        {
            return string.Format("Lv {0}", level);
        }

        private void setExp(long curExp, long maxExp)
        {
            foreach (var exp in m_exp)
            {
                exp.setValue(curExp, maxExp);
            }
        }

        private void setExp(Data data)
        {
            setExp(data.levelExp.exp, data.maxExp);
        }
    }
}