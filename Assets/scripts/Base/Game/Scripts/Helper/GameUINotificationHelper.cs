using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class GameUINotificationHelper : UINotificationHelper<eNotification>
{
    public static GameUINotificationHelper getInstance()
    {
        return getInstance<GameUINotificationHelper>();
    }
}
