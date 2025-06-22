using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using System;
using System.IO;

namespace UnityHelper
{
    [Serializable]
    public struct Crypto
    {
        public string iv;
        public string key;

        public Crypto(string _iv, string _key)
        {
            iv = _iv;
            key = _key;
        }

        public bool isEmpty()
        {
            if (string.IsNullOrEmpty(iv) || string.IsNullOrEmpty(key))
                return true;

            return false;
        }

        public void set(Crypto crypto)
        {
            iv = crypto.iv;
            key = crypto.key;
        }

        public static Crypto empty()
        {
            return new Crypto();
        }
    }

    public class AES
    {
        public static string Encode(string strCryptoText, Crypto crypto, bool isUseBom = true)
        {
            string strRet;
            byte[] byText;

            if (string.IsNullOrEmpty(strCryptoText) || string.IsNullOrEmpty(strCryptoText))
                return "";

            try
            {
                if (isUseBom)
                {
                    byText = UTF8Encoding.UTF8.GetBytes(strCryptoText);
                }
                else
                {
                    Encoding encoding = new UTF8Encoding(false);
                    byText = encoding.GetBytes(strCryptoText);
                }

                using (RijndaelManaged rijndael = new RijndaelManaged())
                {
                    rijndael.Mode = CipherMode.CBC;
                    rijndael.IV = Convert.FromBase64String(crypto.iv);
                    rijndael.Key = Convert.FromBase64String(crypto.key);

                    ICryptoTransform encryptor = rijndael.CreateEncryptor(rijndael.Key, rijndael.IV);

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(byText, 0, byText.Length);
                            cryptoStream.FlushFinalBlock();

                            byte[] byChiperText = memoryStream.ToArray();

                            strRet = Convert.ToBase64String(byChiperText);
                        }
                    }
                }
            }
            catch
            {
                strRet = "";
            }

            return strRet;
        }

        static public string Decode(string strCryptoText, Crypto crypto, bool isUseBom = true)
        {
            string strRet;
            byte[] byText;

            if (string.IsNullOrEmpty(strCryptoText) || string.IsNullOrEmpty(strCryptoText))
                return "";

            try
            {
                byText = Convert.FromBase64String(strCryptoText);

                using (RijndaelManaged rijndael = new RijndaelManaged())
                {
                    rijndael.Mode = CipherMode.CBC;
                    rijndael.IV = Convert.FromBase64String(crypto.iv);
                    rijndael.Key = Convert.FromBase64String(crypto.key);

                    ICryptoTransform decryptor = rijndael.CreateDecryptor(rijndael.Key, rijndael.IV);

                    using (MemoryStream memoryStream = new MemoryStream(byText))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            byte[] byChiperText = new byte[byText.Length];

                            int nDecryptLength = cryptoStream.Read(byChiperText, 0, byChiperText.Length);

                            if (isUseBom)
                            {
                                strRet = UTF8Encoding.UTF8.GetString(byChiperText, 0, nDecryptLength);
                            }
                            else
                            {
                                Encoding encoding = new UTF8Encoding(false);
                                strRet = encoding.GetString(byChiperText, 0, nDecryptLength);
                            }
                        }
                    }
                }
            }
            catch
            {
                strRet = "";
            }

            return strRet;
        }
    }
}