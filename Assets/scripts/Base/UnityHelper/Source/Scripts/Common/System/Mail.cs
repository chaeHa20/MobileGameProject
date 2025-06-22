using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace UnityHelper
{
    public class Mail
    {
        public void send(string mailto, string subject, string body, string bodyData = null)
        {
            string _subject = escapeUrl(subject);
            string _body = escapeUrl(body);

            if (!string.IsNullOrEmpty(bodyData))
            {
                _body += escapeUrl("\n\n");
                _body += bodyData;
            }

            Application.OpenURL("mailto:" + mailto + "?subject=" + _subject + "&body=" + _body);
        }

        private string escapeUrl(string url)
        {
            // 왜 replace를 하나?
            return UnityWebRequest.EscapeURL(url).Replace("+", "%20");
        }
    }
}