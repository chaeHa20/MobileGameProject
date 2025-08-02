using System;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

[Serializable]
public class LocalAdData : LocalData
{
    [SerializeField] List<long> m_shopCoolTimes = new List<long>();
    [SerializeField] bool m_isNoAds = false;

    public bool isNoAds { get => (bool)m_isNoAds; set => m_isNoAds = value; }

    public override void initialize(string _name, int _id)
    {
        base.initialize(_name, _id);

        // initShopAdCoolTimes();// TODO: 2024-07-23 by pms
    }

    public override void checkValid()
    {
        base.checkValid();
        
    }

    public bool isAvailableAdReward()
    {
        foreach (var coolTime in m_shopCoolTimes)
        {
            if (!TimeHelper.isCoolTime((long)coolTime))
                return true;
        }

        return false;
    }

    public long getAdCoolTime(int adCoolTimeId)
    {
        if (0 >= adCoolTimeId)
            return 0;

        return m_shopCoolTimes[adCoolTimeId - 1];
    }

    public override string toString()
    {
        return string.Format("isNoAds : {0}", m_isNoAds);
    }
}
