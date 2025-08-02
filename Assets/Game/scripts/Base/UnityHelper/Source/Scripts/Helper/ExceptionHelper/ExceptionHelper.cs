using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityHelper
{
    public abstract class ExceptionHelper<T> : NonMonoSingleton<T> where T : class, new()
    {
        private Crypto m_crypto;
        private string m_mailAddress;

        protected abstract void openSendMailMsgBox(string condition, string stackTrace);
        protected abstract void openEditorMsgBox(string condition, string stackTrace);

        public void initialize(string mailAddress, Crypto crypto)
        {
            m_mailAddress = mailAddress;
            m_crypto = crypto;

            Application.logMessageReceived += logCallback;
        }

        private void logCallback(string condition, string stackTrace, LogType type)
        {
#if UNITY_EDITOR
            if (LogType.Exception == type)
                openEditorMsgBox(condition, stackTrace);
#else
            if (LogType.Exception == type)
                openSendMailMsgBox(condition, stackTrace);
#endif
        }

        protected void sendMail(string condition, string stackTrace, string data = null)
        {
            string body = stackTraceToString(condition, stackTrace);
            SystemHelper.sendExceptionMail(body, m_mailAddress, data);
        }

        protected string stackTraceToString(string condition, string stackTrace)
        {
            var str = Application.productName + "\n";
            str += Application.version + "\n\n";
            str += condition + "\n";
            str += checkStackTrace(stackTrace) + "\n\n";

#if UNITY_ANDROID
            str += "From Android\n";
#elif UNITY_IOS
            str += "From iOS\n";
#endif

            if (m_crypto.isEmpty())
                return str;
            else
                return AES.Encode(str, m_crypto);
        }
        
        private string checkStackTrace(string stackTrace)
        {
            if (string.IsNullOrEmpty(stackTrace))
            {
                System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
                stackTrace = st.ToString();
            }

            return stackTrace;
        }

        protected void quitApplication()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}