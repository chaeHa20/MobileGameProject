using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public abstract class LocalContainerData : LocalData
    {
        private LocalDataHelper m_dataHelper = null;

        protected abstract string getSubDataName(int subId);

        public override void addSubDatas(LocalDataHelper dataHelper)
        {
            m_dataHelper = dataHelper;
        }

        protected virtual List<string> getSubDataNames()
        {
            return null;
        }

        public virtual List<DATA> getSubDatas<DATA>() where DATA : LocalData
        {
            var datas = new List<DATA>();

            var subDataNames = getSubDataNames();
            foreach(var subDataName in subDataNames)
            {
                datas.Add(m_dataHelper.getData<DATA>(subDataName));
            }

            return datas;
        }

        public DATA addSubData<DATA>(int subId) where DATA : LocalData, new()
        {
            var dataName = getSubDataName(subId);
            return m_dataHelper.addData<DATA>(dataName, subId);
        }

        public DATA getSubData<DATA>(int subId) where DATA : LocalData
        {
            var dataName = getSubDataName(subId);
            return m_dataHelper.getData<DATA>(dataName);
        }

        public bool isExistSubData(int subId)
        {
            var subData = getSubData<LocalData>(subId);
            return null != subData;
        }
    }
}