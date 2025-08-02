using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Linq;
using System.Globalization;
using System.Text.RegularExpressions;

namespace UnityHelper
{
    public partial class SystemHelper
    {
        public static string getNewUuid()
        {
            return Guid.NewGuid().ToString();
        }

        /// <param name="length">min : 4, max : 12</param>
        /// <returns></returns>
        public static string getNewUuid2(int length, bool isUpper = false)
        {
            StringBuilder builder = new StringBuilder();
            Enumerable
               .Range(65, 26)
                .Select(e => ((char)e).ToString())
                .Concat(Enumerable.Range(97, 26).Select(e => ((char)e).ToString()))
                .Concat(Enumerable.Range(0, 10).Select(e => e.ToString()))
                .OrderBy(e => Guid.NewGuid())
                .Take(12)
                .ToList().ForEach(e => builder.Append(e));
            string id = builder.ToString();

            length = Mathf.Max(4, length);
            length = Mathf.Min(12, length);
            var r = id.Substring(0, length);

            return (isUpper) ? r.ToUpper() : r;
        }

        public static void swap<T>(ref T t1, ref T t2)
        {
            T temp = t1;
            t1 = t2;
            t2 = temp;
        }

        public static bool isInternetReachable()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }

        public static void shuffle<T>(List<T> list)
        {
            int count = list.Count;
            for (int i = 0; i < count; ++i)
            {
                int r_index = UnityEngine.Random.Range(0, count);

                T temp = list[i];
                list[i] = list[r_index];
                list[r_index] = temp;
            }
        }

        public static void shuffle<T>(T[] list)
        {
            int count = list.Length;
            for (int i = 0; i < count; ++i)
            {
                int r_index = UnityEngine.Random.Range(0, count);

                T temp = list[i];
                list[i] = list[r_index];
                list[r_index] = temp;
            }
        }

        public static void openMarket(string packageName)
        {
            string marketUrl = "market://details?id=" + packageName;
            openUrl(marketUrl);
        }

        public static void openUrl(string url)
        {
            Application.OpenURL(url);
        }

        public static void sendExceptionMail(string body, string mailAddress, string data = null)
        {
            Mail mail = new Mail();
            string subject = string.Format("{0}({1}) exception", Application.productName, Application.version);
            mail.send(mailAddress, subject, body, data);
        }

        public static void sendHelpMail(string mailAddress, string data = null)
        {
            Mail mail = new Mail();
            string subject = "";
            string body = string.Format("{0}({1})\n\n", Application.productName, Application.version);
            mail.send(mailAddress, subject, body, data);
        }

        /// <summary>
        /// https://stackoverflow.com/questions/568968/does-any-one-know-of-a-faster-method-to-do-string-split
        /// </summary>
        public static List<string> fsplit(string src, char delim)
        {
            if (Logx.isActive)
                Logx.assert(!string.IsNullOrEmpty(src), "src is null");

            List<string> output = new List<string>();
            int startIndex = 0;
            int index;
            while ((index = src.IndexOf(delim, startIndex)) != -1)
            {
                int subLength = index - startIndex;
                output.Add(src.Substring(startIndex, subLength));
                startIndex = index + 1;
            }
            output.Add(src.Substring(startIndex));
            return output;
        }

        public static List<string> fsplit(string src, string delim)
        {
            if (Logx.isActive)
            {
                Logx.assert(!string.IsNullOrEmpty(src), "src is null");
                Logx.assert(!string.IsNullOrEmpty(delim), "delim is null");
            }

            List<string> output = new List<string>();
            int startIndex = 0;
            int index;
            while ((index = src.IndexOf(delim, startIndex)) != -1)
            {
                int subLength = index - startIndex;
                output.Add(src.Substring(startIndex, subLength));
                startIndex = index + 1;
            }
            output.Add(src.Substring(startIndex));
            return output;
        }

        public static List<string> fsplit(string src, string[] delims)
        {
            if (Logx.isActive)
            {
                Logx.assert(!string.IsNullOrEmpty(src), "src is null");
                Logx.assert(null != delims, "delims is null");
            }

            List<string> output = new List<string>();
            int index = 0;
            int startIndex = 0;
            while (true)
            {
                for (int i = 0; i < delims.Length; ++i)
                {
                    index = src.IndexOf(delims[i], startIndex);
                    if (0 <= index)
                        break;
                }

                if (0 > index)
                    break;

                int subLength = index - startIndex;
                output.Add(src.Substring(startIndex, subLength));
                startIndex = index + 1;
            }
            output.Add(src.Substring(startIndex));
            return output;
        }

        public static void forEachEnum<T>(Action<T> callback) where T : struct
        {
            if (Logx.isActive)
                Logx.assert(null != callback, "callback is null");

            foreach (T t in getEnumValues<T>())
            {
                callback(t);
            }
        }

        public static string[] getEnumStrings<T>() where T : struct
        {
            return Enum.GetNames(typeof(T));
        }

        public static T[] getEnumValues<T>() where T : struct
        {
            return (T[])Enum.GetValues(typeof(T));
        }

        public static bool exist<T>(List<T> list, T value)
        {
            if (null == list || 0 == list.Count)
            {
                if (Logx.isActive)
                    Logx.warn("list is null or empty");
                return false;
            }

            int index = list.IndexOf(value);
            return (0 <= index);
        }

        public static string listToString(List<int> list)
        {
            var strings = list.ConvertAll<string>(x => x.ToString());
            return string.Join(", ", strings);
        }

        /// <summary>
        /// https://learn.microsoft.com/ko-kr/dotnet/standard/base-types/how-to-verify-that-strings-are-in-valid-email-format
        /// </summary>
        public static bool isValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                if (Logx.isActive)
                    Logx.exception(e);

                return false;
            }
            catch (ArgumentException e)
            {
                if (Logx.isActive)
                    Logx.exception(e);

                return false;
            }

            try
            {
                return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}