using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public enum eDevice
    {
        Android,
        Ios
    }

    public enum eLanguage
    {
        Kr,
        En,
        Jp,
        Cn,
        Tw,
        Vn,
        Th,
        Ru,
        Id,
        Es,
        Pt,
        De,
        Tr,
        Hi,
        Fr,
        My,
        It,

        None = 100,
    }

    public enum eSocialMedia
    {
        Editor,
        Google,
        Apple,
        Guest,
    }

    public enum eAdFormat
    {
        Banner,
        Interstitial,
        Reward,
        Native,
    }

    public enum eAdNetwork
    {
        AdMob,
        UnityAds,
        AppLovin,
    }

    public enum eAdResult
    {
        Exhausted,              // 소진됬음
        InternetNotRechable,    // 인터넷이 연결 안되어 있음

        Closed,     // 닫기
        Rewarded,   // 보상 받음
        Fail,       // 실패
        Stop,       // 보다가 멈춤
    }
}