using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;
using System.IO;

namespace UnityHelper
{
    public static class Logx
    {
        public enum eLevel
        {
            Off,
            Error,
            Warn,
            Trace,
            All
        }

        private static eLevel m_level = eLevel.All;
        private static string m_filter = null;

        public static string filename { get; set; }
        public static bool isActive { get { return eLevel.Off != m_level; } }
        public static eLevel level { get { return m_level; } set { m_level = value; } }
        public static string filter { get { return m_filter; } set { m_filter = value; } }

        private static void writerLine(string str)
        {
            if (string.IsNullOrEmpty(filename))
                return;

            using (var wr = File.AppendText(filename))
            {
                wr.WriteLine(string.Format(str));
            }
        }

        private static void writeLine(string format, params object[] args)
        {
            writerLine(string.Format(format, args));
        }

        private static bool isEnable(object message, eLevel level)
        {
            if (null == message)
                return false;
            if (isFiltered(message))
                return true;
            if (m_level >= level)
                return true;

            return false;
        }

        private static bool isFiltered(object message)
        {
            if (null == message)
                return false;
            if (string.IsNullOrEmpty(m_filter))
                return false;

            return message.ToString().Contains(m_filter);
        }

        public static void trace(object message)
        {
            if (!isEnable(message, eLevel.Trace))
                return;

            UnityEngine.Debug.Log(message);
            writerLine((string)message);
        }

        public static void trace(string message)
        {
            if (!isEnable(message, eLevel.Trace))
                return;

            UnityEngine.Debug.Log(message);
            writerLine(message);
        }

        public static void trace(DateTime message)
        {
            trace(message.ToString());
        }

        public static void trace(int message)
        {
            trace(message.ToString());
        }

        public static void traceColor(string message, string color)
        {
            if (!isEnable(message, eLevel.Trace))
                return;

            trace("<color={0}>{1}</color>", color, message);
        }

        public static void traceColor(string format, object message, string color)
        {
            if (!isEnable(format, eLevel.Trace))
                return;
            if (!isEnable(message, eLevel.Trace))
                return;

            trace("<color={0}>{1}</color>", color, string.Format(format, message));
        }

        public static void traceColor(string format, in string color, params object[] args)
        {
            if (!isEnable(format, eLevel.Trace))
                return;

            UnityEngine.Debug.LogFormat(string.Format("<color={0}>{1}</color>", color, format), args);
            writeLine(format, args);
        }

        public static void trace(string format, params object[] args)
        {
            if (!isEnable(format, eLevel.Trace))
                return;

            UnityEngine.Debug.LogFormat(format, args);
            writeLine(format, args);
        }
        
        public static void warn(object message)
        {
            if (!isEnable(message, eLevel.Warn))
                return;

            UnityEngine.Debug.LogWarning(message);
            writeLine((string)message);
        }

        public static void warn(string message)
        {
            if (!isEnable(message, eLevel.Warn))
                return;

            UnityEngine.Debug.LogWarning(message);
            writeLine(message);
        }

        public static void warn(string format, params object[] args)
        {
            if (!isEnable(format, eLevel.Warn))
                return;

            UnityEngine.Debug.LogWarningFormat(format, args);
            writeLine(format, args);
        }

        public static void error(object message)
        {
            if (!isEnable(message, eLevel.Error))
                return;

            UnityEngine.Debug.LogError(message);
            writeLine((string)message);
        }

        public static void error(string message)
        {
            if (!isEnable(message, eLevel.Error))
                return;

            UnityEngine.Debug.LogError(message);
            writeLine(message);
        }

        public static void error(string format, params object[] args)
        {
            if (!isEnable(format, eLevel.Error))
                return;

            UnityEngine.Debug.LogErrorFormat(format, args);
            writeLine(format, args);
        }

        public static void exception(Exception e)
        {
            if (!isEnable(e.ToString(), eLevel.Error))
                return;

            error("exception: {0}", e.ToString());
            UnityEngine.Debug.LogException(e);
        }

        public static void assert(bool condition)
        {
            assert(condition, "Assertion failed");
        }

        public static void assert(bool condition, string format)
        {
            if (!condition)
            {
                error(format);
            }
        }

        public static void assert(bool condition, string format, params object[] args)
        {
            if (!condition)
            {
                error(format, args);
            }
        }
    }
}