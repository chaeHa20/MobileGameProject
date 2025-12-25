using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;
using System;
using System.Numerics;

[Serializable]
public class LocalScoreData : LocalData
{
    [SerializeField] BigInteger m_currenctPlayScore = 0;
    [SerializeField] Dictionary<eBestScore, BigInteger> m_bestScoreDic = new Dictionary<eBestScore, BigInteger>();
    [SerializeField] Dictionary<eBestScore, int> m_bestPlayTimeDic = new Dictionary<eBestScore, int>();

    public override void initialize(string _name, int _id)
    {
        base.initialize(_name, _id);
        initCurrentScore();
        initBestScores();
        initBestPlayTime();
    }

    private void initCurrentScore()
    {
        m_currenctPlayScore = 0;
    }

    private void initBestScores()
    {
        var list = Enum.GetValues(typeof(eBestScore));
        foreach(eBestScore type in list)
            resetBestScore(type);
    }

    private void initBestPlayTime()
    {
        var list = Enum.GetValues(typeof(eBestScore));
        foreach (eBestScore type in list)
            resetBestPlayTime(type);
    }

    private void updateBestScore()
    {
        var e = m_bestScoreDic.GetEnumerator();
        while(e.MoveNext())
        {
            var cur = e.Current;
            if (cur.Value < m_currenctPlayScore)
                m_bestScoreDic[cur.Key] = m_currenctPlayScore;
        }
    }

    public void updateScore(BigMoney newScore)
    {
        m_currenctPlayScore = newScore.value;
        updateBestScore();
    }

    public void updatePlayTime(int second)
    {
        var e = m_bestPlayTimeDic.GetEnumerator();
        while (e.MoveNext())
        {
            var cur = e.Current;
            if (cur.Value < second)
                m_bestPlayTimeDic[cur.Key] = second;
        }
    }

    public BigMoney GetBestScore(eBestScore scoreType)
    {
        if (m_bestScoreDic.TryGetValue(scoreType, out BigInteger value))
            return new BigMoney(value);
        else
            return new BigMoney(-1);
    }

    public int GetBestPlayTime(eBestScore scoreType)
    {
        if (m_bestPlayTimeDic.TryGetValue(scoreType, out int value))
            return value;
        else
            return -1;
    }

    public BigMoney GetCurrentScore()
    {
        return new BigMoney(m_currenctPlayScore);
    }

    public void resetBestScore(eBestScore scoreType)
    {
        if (m_bestScoreDic.ContainsKey(scoreType))
            m_bestScoreDic[scoreType] = 0;
        else
            m_bestScoreDic.Add(scoreType, 0);
    }

    public void resetBestPlayTime(eBestScore scoreType)
    {
        if (m_bestPlayTimeDic.ContainsKey(scoreType))
            m_bestPlayTimeDic[scoreType] = 0;
        else
            m_bestPlayTimeDic.Add(scoreType, 0);
    }

    public void resetCurrentScore()
    {
        m_currenctPlayScore = 0;
    }

    public override void checkValid()
    {
        base.checkValid();
    }
}
