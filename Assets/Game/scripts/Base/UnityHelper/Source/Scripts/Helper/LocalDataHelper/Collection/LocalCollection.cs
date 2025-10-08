using System;
using UnityEngine;

namespace UnityHelper
{
    public interface ICollection
    {
        public int getId();
        public LocalCollection.CollectionType getType();
    }

    [Serializable]
    public class LocalCollection : ICollection
    {
        [Serializable]
        public struct CollectionType
        {
            public int main;
            public eGrade grade;

            public CollectionType(int _main, eGrade _grade)
            {
                main = _main;
                grade = _grade;
            }

            public bool isEqual(CollectionType type)
            {
                return isEqual(type.main, type.grade);
            }
            public bool isEqual(int _main)
            {
                return main == _main;
            }

            public bool isEqual(int _main, eGrade _grade)
            {
                return main == _main && grade == _grade;
            }
        }

        [SerializeField] string m_uuid;
        [SerializeField] long m_createTick;
        [SerializeField] int m_id;
        [SerializeField] CollectionType m_type;
        [SerializeField] int m_level;
        [SerializeField] long m_expireTick;// 기간 한정으로 사용 가능한 콜렉션 전용
        [SerializeField] float m_totalExp;
        [SerializeField] bool m_isNew;


        public string uuid => m_uuid;
        public long createTick => m_createTick;
        public int id => m_id;
        public CollectionType type => m_type;
        public int level { get { return m_level; } set { m_level = value; } }
        public long expireTick { get { return m_expireTick; } set { m_expireTick = value; } }
        public float totalExp { get { return m_totalExp; } set { m_totalExp = value; } }
        public bool isNew { get { return m_isNew; } set { m_isNew = value; } }

        public virtual int maxGrade => 0;

        public static T create<T>(int _id, CollectionType _type, int _level) where T : LocalCollection, new()
        {
            T t = new T()
            {
                m_id = _id,
                m_type = _type,
                m_level = _level,
                m_isNew = true,
        };

            return t;
        }

        public LocalCollection(int _id, CollectionType _type, int _level)
        {
            if (Logx.isActive)
            {
                Logx.assert(0 < _id, "invalid item id {0}", _id);
                Logx.assert(0 <= _level, "invalid level {0}", _level);
            }

            m_uuid = SystemHelper.getNewUuid();
            m_createTick = DateTime.Now.Ticks;
            m_type = _type;
            m_id = _id;
            m_level = _level;
        }

        public LocalCollection()
        {
            m_uuid = SystemHelper.getNewUuid();
            m_createTick = DateTime.Now.Ticks;
            m_type = new CollectionType();
            m_id = 0;
            m_level = 0;
            m_totalExp = 0;
            m_isNew = true;
        }

        public int getId()
        {
            return m_id;
        }

        public CollectionType getType()
        {
            return m_type;
        }

        
        public bool isEqual(CollectionType type)
        {
            return m_type.isEqual(type);
        }


        public bool isExpired()
        {
            if (0 == m_expireTick)
                return false;

            if (TimeHelper.isCoolTime(m_expireTick))
                return false;

            return true;
        }

        public virtual T clone<T>() where T : LocalCollection, new()
        {
            var t = new T
            {
                m_uuid = m_uuid,
                m_createTick = m_createTick,
                m_id = m_id,
                m_type = m_type,
                m_level = m_level,
                m_expireTick = m_expireTick,
            };

            return t;
        }
    }
}