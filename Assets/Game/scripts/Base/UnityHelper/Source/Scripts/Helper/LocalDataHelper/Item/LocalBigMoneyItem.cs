using System;
using System.Numerics;
using UnityEngine;

namespace UnityHelper
{
    [Serializable]
    public class LocalBigMoneyItem : ILocalItem
    {
        [SerializeField] int m_id;
        [SerializeField] LocalItem.Type m_type;
        [SerializeField] BigMoney m_count = null;

        public int id => (int)m_id;
        public LocalItem.Type type => m_type;
        public BigMoney count { get => m_count; set => m_count = value; }

        public static LocalBigMoneyItem create(int _id, LocalItem.Type _type, BigInteger _count)
        {
            var t = new LocalBigMoneyItem()
            {
                m_id = _id,
                m_type = _type,
                m_count = new BigMoney(_count),
            };

            return t;
        }

        public LocalBigMoneyItem(int _id, LocalItem.Type _type, BigInteger _count)
        {
            m_type = _type;
            m_id = _id;
            m_count = new BigMoney(_count);
        }

        public LocalBigMoneyItem()
        {
            m_type = new LocalItem.Type();
            m_id = 0;
            m_count = new BigMoney(0);
        }

        public int getId()
        {
            return m_id;
        }

        public LocalItem.Type getType()
        {
            return m_type;
        }

        public bool use(BigInteger _count)
        {
            if (Logx.isActive)
                Logx.assert(0 <= _count, "invalid count {0}", _count);

            if (m_count.value < _count)
                return false;

            m_count -= _count;
            return true;
        }

        public bool isEnough(BigInteger _count)
        {
            if (Logx.isActive)
                Logx.assert(0 <= _count, "invalid count {0}", _count);

            return m_count.value >= _count;
        }

        public bool isEmpty()
        {
            return (0 == m_count.value);
        }

        public bool isEqual(LocalItem.Type type)
        {
            return m_type.isEqual(type);
        }

        public void clearItemCount(int intiCount)
        {
            m_count.value = intiCount + 1;
        }

        public void serialize()
        {
            m_count.serialize();
        }

        public void deserialize()
        {
            m_count.deserialize();
        }
    }
}