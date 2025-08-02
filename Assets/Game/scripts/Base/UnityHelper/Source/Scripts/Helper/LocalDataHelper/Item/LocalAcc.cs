using System;
using UnityEngine;

namespace UnityHelper
{
    public interface ILocalAcc
    {
        public int getId();
        public LocalAcc.Type getType();
    }

    [Serializable]
    public class LocalAcc : ILocalAcc
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
        [SerializeField] eParts m_partsType;
        [SerializeField] int m_level;
        [SerializeField] int m_grade;
        [SerializeField] long m_expireTick;

        public string uuid => m_uuid;
        public string uuid4 => m_uuid4;
        public long createTick => m_createTick;
        public int id => m_id;
        public Type type => m_type;
        public int level { get { return m_level; } set { m_level = value; } }
        public int grade { get { return m_grade; } set { m_grade = value; } }
        public long expireTick { get { return m_expireTick; } set { m_expireTick = value; } }

        public virtual int maxGrade => 0;

        public static T create<T>(int _id, Type _type, eParts _parts, int _level, int _grade) where T : LocalAcc, new()
        {
            T t = new T()
            {
                m_id = _id,
                m_type = _type,
                m_partsType = _parts,
                m_level = _level,
                m_grade = _grade,
            };

            return t;
        }

        public LocalAcc(int _id, Type _type, eParts _parts, int _level, int _grade)
        {
            if (Logx.isActive)
            {
                Logx.assert(0 < _id, "invalid item id {0}", _id);
                Logx.assert(0 <= _parts, "invalid count {0}", _parts);
                Logx.assert(0 <= _level, "invalid level {0}", _level);
            }

            m_uuid = SystemHelper.getNewUuid();
            m_uuid4 = SystemHelper.getNewUuid2(4);
            m_createTick = DateTime.Now.Ticks;
            m_type = _type;
            m_id = _id;
            m_partsType = _parts;
            m_level = _level;
            m_grade = _grade;
        }

        public LocalAcc()
        {
            m_uuid = SystemHelper.getNewUuid();
            m_uuid4 = SystemHelper.getNewUuid2(4);
            m_createTick = DateTime.Now.Ticks;
            m_type = new Type();
            m_id = 0;
            m_partsType = eParts.None;
            m_level = 0;
            m_grade = 0;
        }

        public int getId()
        {
            return m_id;
        }

        public Type getType()
        {
            return m_type;
        }


        public bool isEqual(Type type)
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

        public virtual T clone<T>() where T : LocalAcc, new()
        {
            var t = new T
            {
                m_uuid = m_uuid,
                m_createTick = m_createTick,
                m_id = m_id,
                m_type = m_type,
                m_partsType = m_partsType,
                m_level = m_level,
                m_grade = m_grade,
                m_expireTick = m_expireTick,
            };

            return t;
        }

    }
}