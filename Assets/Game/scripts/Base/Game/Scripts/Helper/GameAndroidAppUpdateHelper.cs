using UnityEngine;
using UnityHelper;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

public class GameAndroidAppUpdateHelper
{
    public static void check(MonoBehaviour mono, Action notUpdateCallback)
    {
#if UNITY_ANDROID && _ANDROID_APP_UPDATE_
        var updateHelper = new AndroidAppUpdateHelper();
        updateHelper.check(mono, (isUpdateAvailable) =>
        {
            if (isUpdateAvailable)
            {
                openUpdateNoticeWindow();
            }
            else
            {
                notUpdateCallback();
            }
        });
#else
        notUpdateCallback();
#endif
    }

//    private static void openUpdateNoticeWindow()
//    {
//#if UNITY_EDITOR
//        EditorApplication.isPlaying = false;
//#endif
//        var data = new UIAppUpdateNoticeWindowData
//        {
//            resourceId = eResource.UIAppUpdateNoticeWindow,
//            layer = (int)eUIRestaurantLayer.Main,
//            inactiveCurrent = UIWindowData.eInactiveCurrent.None,
//            destroyCallback = () =>
//            {
//                SystemHelper.openMarket(Application.identifier);
//            }
//        };
//        GameUIHelper.getInstance().openGameWindow<UIAppUpdateNoticeWindow>(data, true);
//    }
}
