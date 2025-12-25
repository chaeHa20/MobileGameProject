using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityHelper;
using System.Collections.Generic;

public class UIRunnerMainBottom : UIComponent
{
    [SerializeField] TMP_Text m_userScore;
    [SerializeField] TMP_Text m_userPlayTime;
    

    [SerializeField] [HideInInspector] private int m_totalPlayTime =0;
    private UpdateTimer m_surviveTimer = new UpdateTimer();

    private bool m_isPlayStartCountScore = false;

    private void Awake()
    {
        registMessage((int)eUIMessage.UpdateScore, updateRunnerScoreCallBack);
        registMessage((int)eUIMessage.PlayStartCountScore, updatePlayStartCallBack);
    }

    private void Start()
    {
        m_totalPlayTime = 0;
        m_isPlayStartCountScore = false;
        m_surviveTimer.initialize(Define.BASE_SECOND, false);
    }

    private void updateRunnerScoreCallBack(List<object> datas)
    {
        BigMoney value = datas[0] as BigMoney;
        if (null != m_userScore && null != value)
            m_userScore.text = value.toString();
    }

    private void updatePlayStartCallBack(List<object> datas)
    {
        m_isPlayStartCountScore = true;
    }

    private void FixedUpdate()
    {
        var dt = Time.deltaTime;
        if (m_surviveTimer.update(dt))
        {
            UpdatePlayTime();
        }
    }

    private void UpdatePlayTime()
    {
        m_totalPlayTime++;
        m_surviveTimer.initialize(Define.BASE_SECOND, false);
        TimeHelper.secondToTime(m_totalPlayTime, out int d, out int h, out int m, out int s);

        if(null != m_userPlayTime)
            m_userPlayTime.text =getFormat(d, h, m, s);

        if (m_isPlayStartCountScore && 0 < m_totalPlayTime && 0 == m_totalPlayTime % Define.SCORE_UPDATE_SECOND)
            updatePlayTimeScore();
    }

    private void updatePlayTimeScore()
    {
        var req = new Req_UpdatePlayScore
        { 
            addScore = Define.ADD_SCORE_PER_PLAYTIME,
            isUpdatePlayTime = true,
            second = m_totalPlayTime,
        };

        GameLocalDataHelper.instance.request<Res_UpdatePlayScore>(req, (res) =>
        {
            if(res.isSuccess)
                sendMessage((int)eUIMessage.UpdateScore, res.scoreResult);
        });
    }


    protected virtual string getFormat(int d, int h, int m, int s)
    {
        return string.Format("{0:00}:{1:00}:{2:00}", h, m, s);
    }
}
