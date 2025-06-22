using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    /// <summary>
    /// 상속된 클래스의 직렬화는 안된다..
    /// </summary>
    public class JsonHelper
    {
        public static T fromJson<T>(string json, Crypto crypto)
        {
            string data = (crypto.isEmpty()) ? json : AES.Decode(json, crypto);
            return fromJson<T>(data);
        }

        public static T fromJson<T>(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }

        public static string toJson(object obj, Crypto crypto, bool prettyPrint = false)
        {
            string json = toJson(obj, prettyPrint);
            return (crypto.isEmpty()) ? json : AES.Encode(json, crypto);
        }

        public static string toJson(object obj, bool prettyPrint = false)
        {
            return JsonUtility.ToJson(obj, prettyPrint);
        }
    }
}