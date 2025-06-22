using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public class PoolObject : Disposable
    {
        private string m_poolName = "";
        private long m_poolUuid = 0;
        private int m_tempLayer = -1;

        public string poolName { get { return m_poolName; } }
        public long poolUuid { get { return m_poolUuid; } }

        public virtual void initializePool(long _poolUuid, string _poolName)
        {
            if (Logx.isActive)
                Logx.assert(!string.IsNullOrEmpty(_poolName), "poolName is null or empty");
            m_poolUuid = _poolUuid;
            m_poolName = _poolName;
        }

        public void setTempLayer(int layer)
        {
            m_tempLayer = gameObject.layer;
            GraphicHelper.setLayer(gameObject, (int)layer);
        }

        public void restoreTempLayer()
        {
            GraphicHelper.setLayer(gameObject, m_tempLayer);
            m_tempLayer = -1;
        }

        public virtual void pushRestore()
        {
            if (0 <= m_tempLayer)
                restoreTempLayer();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                if (!PoolHelper.isNullInstance())
                    PoolHelper.instance.push(this);
            }
        }
    }
}