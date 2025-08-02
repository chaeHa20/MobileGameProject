using UnityEngine;
using System.Collections;
using System.Security.Cryptography;

#if UNITY_EDITOR
using UnityEditor;

namespace UnityHelper
{
    public class AESEditor
    {
        [MenuItem("Settings/AESSettings/Generate Key")]
        public static void GenerateAESKey()
        {
            using (RijndaelManaged myRijindael = new RijndaelManaged())
            {
                myRijindael.GenerateKey();
                myRijindael.GenerateIV();

                byte[] rijindaelKey = myRijindael.Key;
                byte[] rijindaelIV = myRijindael.IV;
                string key = System.Convert.ToBase64String(rijindaelKey);
                string iv = System.Convert.ToBase64String(rijindaelIV);

                string data = string.Format("key : {0}\n {1}\niv : {2}\n {3}", key, rijindaelKey.ToString(), iv, rijindaelIV.ToString());

                Debug.LogFormat("[AES key]    \n{0}", data);

                EditorUtility.DisplayDialog("AES KEY", data, "확인");
            }
        }
    }
}
#endif