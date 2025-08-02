using System;
using UnityHelper;

public class LocalProtocolUseCurrency : GameLocalProtocol
{
    public override void process(Req_LocalData _req, Action<Res_LocalData> callback)
    {
        var req = _req as Req_UseCurrency;

        if (!tryUseCurrency(req.currencyType, req.useValue, out LocalBigMoneyItem resCurrency))
        {
            callback(Res_LocalData.createError<Res_UseCurrency>(getNotEnoughCurrencyError(req.currencyType)));
            return;
        }

        var res = new Res_UseCurrency
        {
            currency = resCurrency,
        };

        callback(res);
    }
}
