using System;
using UnityEngine;

namespace MoreMountains.NiceVibrations
{
    public enum HapticTypes { Selection, Success, Warning, Failure, LightImpact, MediumImpact, HeavyImpact, None }

    /// <summary>
    /// This class will allow you to trigger vibrations and haptic feedbacks on both iOS and Android, 
    /// or on each specific platform independently.
    /// 
    /// For haptics patterns, it takes inspiration from the iOS guidelines : 
    /// https://developer.apple.com/ios/human-interface-guidelines/user-interaction/feedback
    /// Of course the iOS haptics are called directly as they are, and they're crudely reproduced on Android.
    /// Feel free to tweak the patterns or create your own.
    /// 
    /// Here's a brief overview of the patterns :
    /// 
    /// - selection : light
    /// - success : light / heavy
    /// - warning : heavy / medium
    /// - failure : medium / medium / heavy / light
    /// - light 
    /// - medium 
    /// - heavy  
    /// 
    /// </summary>
    public static class MMVibrationManager
    {
        // INTERFACE ---------------------------------------------------------------------------------------------------------

        public static long LightDuration = 20;
        public static long MediumDuration = 40;
        public static long HeavyDuration = 80;
        public static int LightAmplitude = 40;
        public static int MediumAmplitude = 120;
        public static int HeavyAmplitude = 255;
        private static int _sdkVersion = -1;
        private static long[] _lightimpactPattern = { 0, LightDuration };
        private static int[] _lightimpactPatternAmplitude = { 0, LightAmplitude };
        private static long[] _mediumimpactPattern = { 0, MediumDuration };
        private static int[] _mediumimpactPatternAmplitude = { 0, MediumAmplitude };
        private static long[] _HeavyimpactPattern = { 0, HeavyDuration };
        private static int[] _HeavyimpactPatternAmplitude = { 0, HeavyAmplitude };
        private static long[] _successPattern = { 0, LightDuration, LightDuration, HeavyDuration };
        private static int[] _successPatternAmplitude = { 0, LightAmplitude, 0, HeavyAmplitude };
        private static long[] _warningPattern = { 0, HeavyDuration, LightDuration, MediumDuration };
        private static int[] _warningPatternAmplitude = { 0, HeavyAmplitude, 0, MediumAmplitude };
        private static long[] _failurePattern = { 0, MediumDuration, LightDuration, MediumDuration, LightDuration, HeavyDuration, LightDuration, LightDuration };
        private static int[] _failurePatternAmplitude = { 0, MediumAmplitude, 0, MediumAmplitude, 0, HeavyAmplitude, 0, LightAmplitude };


        /// <summary>
        /// Returns true if the current platform is Android, false otherwise.
        /// </summary>
        public static bool Android()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
				return true;
#else
            return false;
#endif
        }

        /// <summary>
        /// Triggers a simple vibration
        /// </summary>
        public static void Vibrate()
        {
            if (Android())
            {
                AndroidVibrate(MediumDuration);
            }
        }

        /// <summary>
        /// Triggers a haptic feedback of the specified type
        /// </summary>
        /// <param name="type">Type.</param>
        public static void Haptic(HapticTypes type, bool defaultToRegularVibrate = false)
        {
            if (defaultToRegularVibrate)
            {
#if UNITY_ANDROID
                Handheld.Vibrate();
#endif
                return;
            }

            if (Android())
            {
                switch (type)
                {
                    case HapticTypes.None:
                        // do nothing
                        break;
                    case HapticTypes.Selection:
                        AndroidVibrate(LightDuration, LightAmplitude);
                        break;

                    case HapticTypes.Success:
                        AndroidVibrate(_successPattern, _successPatternAmplitude, -1);
                        break;

                    case HapticTypes.Warning:
                        AndroidVibrate(_warningPattern, _warningPatternAmplitude, -1);
                        break;

                    case HapticTypes.Failure:
                        AndroidVibrate(_failurePattern, _failurePatternAmplitude, -1);
                        break;

                    case HapticTypes.LightImpact:
                        AndroidVibrate(_lightimpactPattern, _lightimpactPatternAmplitude, -1);
                        break;

                    case HapticTypes.MediumImpact:
                        AndroidVibrate(_mediumimpactPattern, _mediumimpactPatternAmplitude, -1);
                        break;

                    case HapticTypes.HeavyImpact:
                        AndroidVibrate(_HeavyimpactPattern, _HeavyimpactPatternAmplitude, -1);
                        break;
                }
            }
        }

        // INTERFACE END ---------------------------------------------------------------------------------------------------------



        // Android ---------------------------------------------------------------------------------------------------------

        // Android Vibration reference can be found at :
        // https://developer.android.com/reference/android/os/Vibrator.html
        // And there starting v26, with support for amplitude :
        // https://developer.android.com/reference/android/os/VibrationEffect.html

#if UNITY_ANDROID && !UNITY_EDITOR
			private static AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			private static AndroidJavaObject CurrentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
			private static AndroidJavaObject AndroidVibrator = CurrentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
			private static AndroidJavaClass VibrationEffectClass;
			private static AndroidJavaObject VibrationEffect;
			private static int DefaultAmplitude;
            private static IntPtr AndroidVibrateMethodRawClass = AndroidJNIHelper.GetMethodID(AndroidVibrator.GetRawClass(), "vibrate", "(J)V", false);
            private static jvalue[] AndroidVibrateMethodRawClassParameters = new jvalue[1];
#else
        private static AndroidJavaClass UnityPlayer;
        private static AndroidJavaObject CurrentActivity;
        private static AndroidJavaObject AndroidVibrator = null;
        private static AndroidJavaClass VibrationEffectClass = null;
        private static AndroidJavaObject VibrationEffect;
        private static int DefaultAmplitude;
        private static IntPtr AndroidVibrateMethodRawClass = IntPtr.Zero;
        private static jvalue[] AndroidVibrateMethodRawClassParameters = null;
#endif

        /// <summary>
        /// Requests a default vibration on Android, for the specified duration, in milliseconds
        /// </summary>
        /// <param name="milliseconds">Milliseconds.</param>
        public static void AndroidVibrate(long milliseconds)
        {
            if (!Android()) { return; }
            AndroidVibrateMethodRawClassParameters[0].j = milliseconds;
            AndroidJNI.CallVoidMethod(AndroidVibrator.GetRawObject(), AndroidVibrateMethodRawClass, AndroidVibrateMethodRawClassParameters);
        }

        /// <summary>
        /// Requests a vibration of the specified amplitude and duration. If amplitude is not supported by the device's SDK, a default vibration will be requested
        /// </summary>
        /// <param name="milliseconds">Milliseconds.</param>
        /// <param name="amplitude">Amplitude.</param>
        public static void AndroidVibrate(long milliseconds, int amplitude)
        {
            if (!Android()) { return; }
            // amplitude is only supported after API26
            if ((AndroidSDKVersion() < 26))
            {
                AndroidVibrate(milliseconds);
            }
            else
            {
                VibrationEffectClassInitialization();
                VibrationEffect = VibrationEffectClass.CallStatic<AndroidJavaObject>("createOneShot", new object[] { milliseconds, amplitude });
                AndroidVibrator.Call("vibrate", VibrationEffect);
            }
        }

        // Requests a vibration on Android for the specified pattern and optional repeat
        // Straight out of the Android documentation :
        // Pass in an array of ints that are the durations for which to turn on or off the vibrator in milliseconds. 
        // The first value indicates the number of milliseconds to wait before turning the vibrator on. 
        // The next value indicates the number of milliseconds for which to keep the vibrator on before turning it off. 
        // Subsequent values alternate between durations in milliseconds to turn the vibrator off or to turn the vibrator on.
        // repeat:  the index into pattern at which to repeat, or -1 if you don't want to repeat.
        public static void AndroidVibrate(long[] pattern, int repeat)
        {
            if (!Android()) { return; }
            if ((AndroidSDKVersion() < 26))
            {
                AndroidVibrator.Call("vibrate", pattern, repeat);
            }
            else
            {
                VibrationEffectClassInitialization();
                VibrationEffect = VibrationEffectClass.CallStatic<AndroidJavaObject>("createWaveform", new object[] { pattern, repeat });
                AndroidVibrator.Call("vibrate", VibrationEffect);
            }
        }

        /// <summary>
        /// Requests a vibration on Android for the specified pattern, amplitude and optional repeat
        /// </summary>
        /// <param name="pattern">Pattern.</param>
        /// <param name="amplitudes">Amplitudes.</param>
        /// <param name="repeat">Repeat.</param>
        public static void AndroidVibrate(long[] pattern, int[] amplitudes, int repeat)
        {
            if (!Android()) { return; }
            if ((AndroidSDKVersion() < 26))
            {
                AndroidVibrator.Call("vibrate", pattern, repeat);
            }
            else
            {
                VibrationEffectClassInitialization();
                VibrationEffect = VibrationEffectClass.CallStatic<AndroidJavaObject>("createWaveform", new object[] { pattern, amplitudes, repeat });
                AndroidVibrator.Call("vibrate", VibrationEffect);
            }
        }

        /// <summary>
        /// Stops all Android vibrations that may be active
        /// </summary>
        public static void AndroidCancelVibrations()
        {
            if (!Android()) { return; }
            AndroidVibrator.Call("cancel");
        }

        /// <summary>
        /// Initializes the VibrationEffectClass if needed.
        /// </summary>
        private static void VibrationEffectClassInitialization()
        {
            if (VibrationEffectClass == null)
            {
                VibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect");
            }
        }

        /// <summary>
        /// Returns the current Android SDK version as an int
        /// </summary>
        /// <returns>The SDK version.</returns>
        public static int AndroidSDKVersion()
        {
            if (_sdkVersion == -1)
            {
                int apiLevel = int.Parse(SystemInfo.operatingSystem.Substring(SystemInfo.operatingSystem.IndexOf("-") + 1, 3));
                _sdkVersion = apiLevel;
                return apiLevel;
            }
            else
            {
                return _sdkVersion;
            }
        }

        // Android End ---------------------------------------------------------------------------------------------------------

    }
}