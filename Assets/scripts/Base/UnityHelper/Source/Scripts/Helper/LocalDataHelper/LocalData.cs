using UnityEngine;
using System;

namespace UnityHelper
{
    [Serializable]
    public abstract class LocalData
    {
        [SerializeField] bool m_isUsed = false;
        [SerializeField] string m_name = null;
        [SerializeField] int m_id = 0;

        public bool isUsed 
        { 
            get => (bool)m_isUsed; 
            set => m_isUsed = value; 
        }
        public string name => m_name;
        public int id => (int)m_id;

        public virtual void initialize(string _name, int _id)
        {
            m_name = _name;
            m_id = _id;
        }

        public virtual void checkValid()
        {

        }

        public virtual void serialize()
        {

        }

        public virtual void deserialize()
        {

        }

        public virtual void addSubDatas(LocalDataHelper dataHelper)
        {

        }

        public virtual void setDailyReset()
        {

        }

        public virtual string toString()
        {
            return "";
        }
    }
}