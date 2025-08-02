using UnityHelper;

public class GameNotificationHelper : NotificationHelper
{
    private bool m_isNotSendNotification = false;

    public bool isNotSendNotification { set { m_isNotSendNotification = value; } }

    public static GameNotificationHelper getInstance()
    {
        return getInstance<GameNotificationHelper>();
    }

    private bool isEnableSendScene()
    {
        return false;
    }

    protected override void sendNotifications()
    {
        if (!isEnableSendScene())
            return;

        if (m_isNotSendNotification)
        {
            if (Logx.isActive)
                Logx.trace("Ignore sendNotifications, m_isNotSendNotification is true");

            m_isNotSendNotification = false;
            return;
        }

        base.sendNotifications();

        //var pushNotificatonTable = GameTableHelper.instance.getTable<PushNotificationTable>((int)eTable.PushNotification);
        //var e = pushNotificatonTable.getEnumerator();
        //while (e.MoveNext())
        //{
        //    var row = e.Current.Value as PushNotificationRow;
        //    var stringId = row.stringId;
        //    var isRepeat = row.isRepeat;

        //    row.forEach(hour =>
        //    {
        //        switch (row.hourType)
        //        {
        //            case PushNotificationRow.eHourType.AbsoluteTime: setAbsoluteTimeNotification(stringId, hour, isRepeat); break;
        //            case PushNotificationRow.eHourType.AfterTime: setAfterTimeNotification(stringId, hour, isRepeat); break;
        //        }
        //    });
        //}
    }

    private void setAbsoluteTimeNotification(int stringId, int hour, bool isRepeat)
    {
        if (Logx.isActive)
            Logx.trace("call AbsoluteTimeNotification");

        var delay = getRemainTime(hour, 0);
        if (Logx.isActive)
            Logx.trace("setNotification delay {0},", delay);

        if (0 < delay)
            sendNotification(stringId, delay, isRepeat);
    }

    private void setAfterTimeNotification(int stringId, int hour, bool isRepeat)
    {
        if (Logx.isActive)
            Logx.trace("call AfterTimeNotification");

        var delay = TimeHelper.hourToSecond(hour);

        if (0 < delay)
            sendNotification(stringId, delay, isRepeat);
    }

    private void setOfflineGoldNotification(int stringId, long delay, bool isRepeat)
    {
        if (Logx.isActive)
            Logx.trace("call OfflineGoldNotification");

        if (0 < delay)
            sendNotification(stringId, delay, isRepeat);
    }
    private void sendNotification(int stringId, long delay, bool isRepeat)
    {
        var title = StringHelper.get("game_title");
        var msg = StringHelper.get(stringId);
        sendNotification(title, msg, delay, isRepeat);
    }
}
