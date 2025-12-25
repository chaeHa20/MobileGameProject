using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class LocalProtocolUpdateScore : GameLocalProtocol
{
    public override void process(Req_LocalData _req, Action<Res_LocalData> callback)
    {
        var req = _req as Req_UpdatePlayScore;
        var data = getData<LocalScoreData>(eLocalData.Score);

        if(null == data)
        {
            callback(Res_LocalData.createError<Res_UpdatePlayScore>((int)eLocalProtocolError.NotExistData));
            return;
        }

        var score = data.GetCurrentScore();
        score.value += req.addScore;

        data.updateScore(score);

        if (req.isUpdatePlayTime)
            data.updatePlayTime(req.second);


        var res = new Res_UpdatePlayScore
        {
            scoreResult = score,
        };

        callback(res);
    }
}
