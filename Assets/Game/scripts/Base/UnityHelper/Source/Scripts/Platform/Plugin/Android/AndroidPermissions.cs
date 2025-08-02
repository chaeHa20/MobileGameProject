using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public class AndroidPermissions : MonoBehaviour
    {
        public static void check(Action<bool> callback)
        {
            if (Logx.isActive)
                Logx.assert(null != callback, "callback is null");

#if UNITY_ANDROID
            GameObject obj = new GameObject
            {
                name = "AndroidPermissions"
            };
            AndroidPermissions plugin = obj.AddComponent<AndroidPermissions>();
            plugin.call(callback);
#elif UNITY_IOS
        callback(true);
#endif
        }

#if UNITY_ANDROID
        private enum AndroidNativePermission
        {
            WRITE_EXTERNAL_STORAGE,
            CAMERA,
        }

        private const string PERMISSION_GRANTED = "Granted";
        private const string PERMISSION_DENIED = "Denied";
        private Action<bool> m_callback = null;

        private void call(Action<bool> callback)
        {
            m_callback = callback;

#if UNITY_EDITOR
            PermissionRequestCallbackInternal(PERMISSION_GRANTED);
#else
            AndroidJavaClass unityActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityActivity.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaClass jc = new AndroidJavaClass("com.nexelon.androidpermissions.AndroidPermissionsActivity");
            AndroidJavaObject jo = jc.CallStatic<AndroidJavaObject>("instance");

            string message = "CameraPermission";
            string okText = "Ok";
            string grantSettingText = "PermissionSetting";
            jo.Call("isGranted", currentActivity, (int)AndroidNativePermission.CAMERA, message, okText, grantSettingText);
#endif
        }

        private void PermissionRequestCallbackInternal(string message)
        {
            bool isPermission = false;
            if (message == PERMISSION_GRANTED)
            {
                isPermission = true;
            }
            else if (message == PERMISSION_DENIED)
            {
                isPermission = false;
            }

            m_callback(isPermission);

            GameObject.Destroy(gameObject);
        }
#endif
    }
}