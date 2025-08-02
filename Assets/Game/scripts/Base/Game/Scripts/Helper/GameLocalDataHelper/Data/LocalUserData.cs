using UnityEngine;
using UnityHelper;
using System;

[Serializable]
public class LocalUserData : LocalData
{
    [SerializeField] long m_lastLoginTime = 0;
    [SerializeField] bool m_isFirstAppStart = false;

    public override void initialize(string _name, int _id)
    {
        base.initialize(_name, _id);

        m_isFirstAppStart = true;
        setLastLoginTime();
    }

    public void login(out bool isFirstAppStart)
    {
        isFirstAppStart = m_isFirstAppStart;
        m_isFirstAppStart = false;

        setLastLoginTime();
    }

    private void setLastLoginTime()
    {
        m_lastLoginTime = DateTime.Now.Ticks;
    }

    public bool isDailyLogin(int dailyResetTime)
    {
        if (0 == m_lastLoginTime)
            return false;

        // var resetDate = UnbiasedTime.Instance.Now().Date.AddSeconds(dailyResetTime);
        // TODO : �ð� ġ�� ������ asset�ε� �̰Ŵ� �ð��� ��� ������ ��쿡�� ���
        var resetDate = DateTime.Today.AddSeconds(dailyResetTime);
        var lastLoginTime = new DateTime(m_lastLoginTime);

        if (lastLoginTime < resetDate && DateTime.Now > resetDate)
            return true;

        return false;
    }
}
