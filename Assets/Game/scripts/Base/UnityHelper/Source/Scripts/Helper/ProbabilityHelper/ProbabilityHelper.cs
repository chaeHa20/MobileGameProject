using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public  class ProbabilityHelper
    {
        public class Selector
        {
            public struct Data
            {
                public int id;
                public float probability;

                public Data(int _id, float _probability)
                {
                    id = _id;
                    probability = _probability;
                }
            }

            private List<Data> m_datas = new List<Data>();

            public void clear()
            {
                m_datas.Clear();
            }

            public void add(int id, float probability)
            {
                m_datas.Add(new Data(id, probability));
            }

            public void add(Selector selector)
            {
                for (int i = 0; i < selector.m_datas.Count; ++i)
                {
                    m_datas.Add(selector.m_datas[i]);
                }
            }

            private float getTotalProbability()
            {
                float totalProbability = 0.0f;
                m_datas.ForEach(x => totalProbability += x.probability);
                return totalProbability;
            }

            public int selectId()
            {
                float totalProbability = getTotalProbability();

                float r = Random.Range(0.0f, totalProbability);

                SystemHelper.shuffle(m_datas);

                // 처음에 0.0f도 포함시키기 위해서
                float s = -1.0f;
                float e = 0.0f;
                for (int i = 0; i < m_datas.Count; ++i)
                {
                    e += m_datas[i].probability;

                    if (s < r && e >= r)
                        return m_datas[i].id;

                    s = e;
                }

                return 0;
            }

            public List<Data>.Enumerator getDataEnumerator()
            {
                return m_datas.GetEnumerator();
            }
        }

        /// <param name="p">0.0f ~ 1.0f</param>
        public static bool isIn(float p)
        {
            return isIn(p, 1.0f);
        }

        public static bool isIn(float p, float max)
        {
            return Random.Range(0.0f, max) <= p;
        }

        public static T getRand<T>(List<T> list)
        {
            if (null == list || 0 == list.Count)
            {
                if (Logx.isActive)
                    Logx.warn("list is null or empty");
                return default;
            }

            int r = Random.Range(0, list.Count);
            return list[r];
        }

        public static Vector3 getRand(Vector3 a, Vector3 b)
        {
            float x = Random.Range(a.x, b.x);
            float y = Random.Range(a.y, b.y);
            float z = Random.Range(a.z, b.z);
            return new Vector3(x, y, z);
        }

        public static int getRandIndex(int exceptIndex, int totalCount)
        {
            int index = Random.Range(0, totalCount);
            if (exceptIndex == index)
            {
                if (0 == exceptIndex)
                {
                    index = Random.Range(1, totalCount);
                }
                else if (totalCount - 1 == exceptIndex)
                {
                    index = Random.Range(0, totalCount - 1);
                }
                else if (totalCount / 2 > exceptIndex)
                {
                    index = Random.Range(0, exceptIndex);
                }
                else
                {
                    index = Random.Range(exceptIndex + 1, totalCount);
                }
            }

            return index;
        }

        public static int selectNormalDistributionValue(List<int> values, float sigma, float mu)
        {
            if (Logx.isActive)
                Logx.trace(">>>>>>>>>>>>> selectNormalDistributionValue {0}", values.ToString());
            
            List<float> probabilities = new List<float>();
            for (int x = 0; x < values.Count; ++x)
            {
                float p = MathHelper.getNormalDistribution(sigma, x, mu);
                probabilities.Add(p);

                if (Logx.isActive)
                    Logx.trace("Normal Distribution, sigma {0}, mu {1}, x {2}, p {3}", sigma, mu, x, p);
            }

            Selector selector = new Selector();
            for (int x = 0; x < probabilities.Count; ++x)
            {
                selector.add(x, probabilities[x]);
            }

            int index = selector.selectId();
            return values[index];
        }

        public static int selectNormalDistributionValue(int firstValue, int lastValue, float sigma, float mu)
        {
            List<int> values = new List<int>();
            for (int i = firstValue; i <= lastValue; ++i)
            {
                values.Add(i);
            }

            return selectNormalDistributionValue(values, sigma, mu);
        }
    }
}