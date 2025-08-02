using System;
using UnityEngine;

namespace UnityHelper
{
    public interface ILocalItem
    {
        public int getId();
        public LocalItem.Type getType();
    }

    [Serializable]
    public class LocalItem : ILocalItem
    {
        [Serializable]
        public struct Type
        {
            public int main;
            public int sub;

            public Type(int _main, int _sub)
            {
                main = _main;
                sub = _sub;
            }

            public bool isEqual(Type type)
            {
                return isEqual(type.main, type.sub);
            }

            public bool isEqual(int _main, int _sub)
            {
                return main == _main && sub == _sub;
            }
        }

        [SerializeField] string m_uuid;
        [SerializeField] string m_uuid4;
        [SerializeField] long m_createTick;
        [SerializeField] int m_id;
        [SerializeField] Type m_type;
        [SerializeField] long m_count;
        [SerializeField] int m_level;
        [SerializeField] int m_grade;
        [SerializeField] long m_expireTick;
        [SerializeField] int m_curDurability;
        [SerializeField] int m_maxDurability;
        [SerializeField] float m_totalExp;
        [SerializeField] bool m_isNew;


        public string uuid => m_uuid;
        public string uuid4 => m_uuid4;
        public long createTick => m_createTick;
        public int id => m_id;
        public Type type => m_type;
        public long count { get { return m_count; } set { m_count = value; } }
        public int level { get { return m_level; } set { m_level = value; } }
        public int grade { get { return m_grade; } set { m_grade = value; } }
        public long expireTick { get { return m_expireTick; } set { m_expireTick = value; } }
        public int maxDurability { get { return m_maxDurability; } }
        public float totalExp { get { return m_totalExp; } set { m_totalExp = value; } }
        public bool isNew { get { return m_isNew; } set { m_isNew = value; } }

        public int curDurability
        {
            get { return m_curDurability; }
            set
            {
                m_curDurability = value;
                if (Logx.isActive)
                    Logx.assert(0 <= m_curDurability, "Invalid cur durability {0}", m_curDurability);
            }
        }
        public virtual int maxGrade => 0;

        public static T create<T>(int _id, Type _type, long _count, int _level, int _durability) where T : LocalItem, new()
        {
            T t = new T()
            {
                m_id = _id,
                m_type = _type,
                m_count = _count,
                m_level = _level,
                m_curDurability = _durability,
                m_maxDurability = _durability,
                m_isNew = true,
        };

            return t;
        }

        public LocalItem(int _id, Type _type, long _count, int _level, int _durability, int _grade)
        {
            if (Logx.isActive)
            {
                Logx.assert(0 < _id, "invalid item id {0}", _id);
                Logx.assert(0 <= _count, "invalid count {0}", _count);
                Logx.assert(0 <= _level, "invalid level {0}", _level);
                Logx.assert(0 <= _durability, "invalid durability {0}", _durability);
            }

            m_uuid = SystemHelper.getNewUuid();
            m_uuid4 = SystemHelper.getNewUuid2(4);
            m_createTick = DateTime.Now.Ticks;
            m_type = _type;
            m_id = _id;
            m_count = _count;
            m_level = _level;
            m_grade = _grade;
            m_curDurability = _durability;
            m_maxDurability = _durability;
        }

        public LocalItem()
        {
            m_uuid = SystemHelper.getNewUuid();
            m_uuid4 = SystemHelper.getNewUuid2(4);
            m_createTick = DateTime.Now.Ticks;
            m_type = new Type();
            m_id = 0;
            m_count = 0;
            m_level = 0;
            m_grade = 0;
            m_curDurability = 0;
            m_maxDurability = 0;
            m_totalExp = 0;
            m_isNew = true;
        }

        public int getId()
        {
            return m_id;
        }

        public Type getType()
        {
            return m_type;
        }

        public bool use(long _count)
        {
            if (Logx.isActive)
                Logx.assert(0 <= _count, "invalid count {0}", _count);

            if (m_count < _count)
                return false;

            m_count -= _count;

            return true;
        }

        public bool isEnough(long _count)
        {
            if (Logx.isActive)
                Logx.assert(0 <= _count, "invalid count {0}", _count);

            return count >= _count;
        }

        public bool isEmpty()
        {
            return (0 == m_count);
        }

        public bool isEqual(Type type)
        {
            return m_type.isEqual(type);
        }

        public float getDurabilityPercent()
        {
            if (0 == m_maxDurability)
                return 0.0f;

            return ((float)m_curDurability / m_maxDurability) * 100.0f;
        }

        public void repair()
        {
            m_curDurability = m_maxDurability;
        }

        public bool isExpired()
        {
            if (0 == m_expireTick)
                return false;

            if (TimeHelper.isCoolTime(m_expireTick))
                return false;

            return true;
        }

        public virtual T clone<T>() where T : LocalItem, new()
        {
            var t = new T
            {
                m_uuid = m_uuid,
                m_createTick = m_createTick,
                m_id = m_id,
                m_type = m_type,
                m_count = m_count,
                m_level = m_level,
                m_grade = m_grade,
                m_expireTick = m_expireTick,
                m_curDurability = m_curDurability,
                m_maxDurability = m_maxDurability,
            };

            return t;
        }
    }
}