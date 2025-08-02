using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace UnityHelper
{
    public class TableRow
    {
        private int m_id = 0;

        public int id { get { return m_id; } }

        public virtual void parse(List<string> cells, List<string> dataTypes, ref int i)
        {
            if (Logx.isActive)
                Logx.assert(null != cells, "cells is null");

            m_id = toInt(cells, ref i);
        }

        protected string toString(List<string> cells, ref int i, bool isReplaceNewLine = false)
        {

            string str = cells[i++];
            if (isReplaceNewLine)
            {
                if (string.IsNullOrEmpty(str))
                    return str;
                else
                    return str.Replace("\\n", "\n");
            }
            else
            {
                return str;
            }
        }

        protected bool toBool(List<string> cells, List<string> dataTypes, ref int i)
        {
            if (dataTypes[i] != "bool" && Logx.isActive)
                Logx.error("id {0}, value Data Type {1} is Not Match", m_id, "bool");

            return 0 != toInt(cells, ref i);
        }

        protected int toInt(List<string> cells, ref int i)
        {
            try
            {
                return Convert.ToInt32(cells[i++], CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                if (Logx.isActive)
                    Logx.error("id {0}, value : {1}, column {2}, {3}", m_id, cells[i - 1], i - 1, e.ToString());
            }

            return 0;
        }

        protected IdValueCount toIdValueCount(List<string> cells,List<string> dataTypes, ref int i)
        {
            if (dataTypes[i] != "idValueCount" && Logx.isActive)
                Logx.error("id {0}, value Data Type {1} is Not Match", m_id, "idValueCount");

            try
            {
                return new IdValueCount(toInt(cells, ref i), toInt(cells, ref i), toInt(cells, ref i));
            }
            catch (Exception e)
            {
                if (Logx.isActive)
                    Logx.error("id {0}, value : {1}, column {2}, {3}", m_id, cells[i - 1], i - 1, e.ToString());
            }

            return new IdValueCount();
        }

        protected IdValue2 toIdValue2(List<string> cells,List<string> dataTypes, ref int i)
        {
            if (dataTypes[i] != "idValue2" && Logx.isActive)
                Logx.error("id {0}, value Data Type {1} is Not Match", m_id, "idValue2");

            try
            {
                return new IdValue2(toInt(cells, ref i), toInt(cells, ref i), toInt(cells, ref i));
            }
            catch (Exception e)
            {
                if (Logx.isActive)
                    Logx.error("id {0}, value : {1}, column {2}, {3}", m_id, cells[i - 1], i - 1, e.ToString());
            }

            return new IdValue2();
        }

        protected IdValueD2 toIdValueD2(List<string> cells, List<string> dataTypes, ref int i)
        {
            if (dataTypes[i] != "idValueD2" && Logx.isActive)
                Logx.error("id {0}, value Data Type {1} is Not Match", m_id, "idValueD2");

            try
            {
                return new IdValueD2(toInt(cells, ref i), toDecimal(cells, ref i), toDecimal(cells, ref i));
            }
            catch (Exception e)
            {
                if (Logx.isActive)
                    Logx.error("id {0}, value : {1}, column {2}, {3}", m_id, cells[i - 1], i - 1, e.ToString());
            }

            return new IdValueD2();
        }

        protected IdValue toIdValue(List<string> cells, ref int i)
        {
            try
            {
                return new IdValue(toInt(cells, ref i), toInt(cells, ref i));
            }
            catch (Exception e)
            {
                if (Logx.isActive)
                    Logx.error("id {0}, value : {1}, column {2}, {3}", m_id, cells[i - 1], i - 1, e.ToString());
            }

            return new IdValue();
        }

        protected IdCount toIdCount(List<string> cells,List<string> dataTypes, ref int i)
        {
            if (dataTypes[i] != "idCount" && Logx.isActive)
                Logx.error("id {0}, value Data Type {1} is Not Match", m_id, "idCount");

            try
            {
                return new IdCount(toInt(cells, ref i), toInt(cells, ref i));
            }
            catch (Exception e)
            {
                if (Logx.isActive)
                    Logx.error("id {0}, value : {1}, column {2}, {3}", m_id, cells[i - 1], i - 1, e.ToString());
            }

            return new IdCount();
        }

        protected long toLong(List<string> cells,List<string> dataTypes, ref int i)
        {
            if (dataTypes[i] != "long" && Logx.isActive)
                Logx.error("id {0}, value Data Type {1} is Not Match", m_id, "long");

            try
            {
                return System.Convert.ToInt64(cells[i++], CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                if (Logx.isActive)
                    Logx.error("id {0}, value : {1}, column {2}, {3}", m_id, cells[i - 1], i - 1, e.ToString());
            }

            return 0;
        }

        protected float toFloat(List<string> cells, List<string> dataTypes, ref int i)
        {
            if (dataTypes[i] != "float" && Logx.isActive)
                Logx.error("id {0}, value Data Type {1} is Not Match", m_id, "float");

            try
            {
                // 소수점 2자리까지 오차 보정
                float v = System.Convert.ToSingle(cells[i++], CultureInfo.InvariantCulture);
                return (float)Math.Truncate((v + 0.000001f) * 100) / 100.0f;
            }
            catch (Exception e)
            {
                if (Logx.isActive)
                    Logx.error("id {0}, value : {1}, column {2}, {3}", m_id, cells[i - 1], i - 1, e.ToString());
            }

            return 0.0f;
        }

        protected double toDouble(List<string> cells, List<string> dataTypes, ref int i)
        {
            if (dataTypes[i] != "double" && Logx.isActive)
                Logx.error("id {0}, value Data Type {1} is Not Match", m_id, "double");

            try
            {
                // 소수점 2자리까지 오차 보정
                float v = System.Convert.ToSingle(cells[i++], CultureInfo.InvariantCulture);
                return (float)Math.Truncate((v + 0.000001) * 100) / 100.0;
            }
            catch (Exception e)
            {
                if (Logx.isActive)
                    Logx.error("id {0}, value : {1}, column {2}, {3}", m_id, cells[i - 1], i - 1, e.ToString());
            }

            return 0.0;
        }

        protected decimal toDecimal(List<string> cells, ref int i)
        {
            try
            {
                return System.Convert.ToDecimal(cells[i++], CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                if (Logx.isActive)
                    Logx.error("id {0}, value : {1}, column {2}, {3}", m_id, cells[i - 1], i - 1, e.ToString());
            }

            return 0;
        }

        protected List<BigMoney> toBigMoneyList(List<string> cells, ref int i, char seperator = ',')
        {
            try
            {
                List<BigMoney> list = new List<BigMoney>();
                List<string> s = SystemHelper.fsplit(cells[i++], seperator);
                for (int c = 0; c < s.Count; ++c)
                {
                    list.Add(new BigMoney(s[c]));
                }

                return list;
            }
            catch (Exception e)
            {
                if (Logx.isActive)
                    Logx.error("id {0}, value : {1}, column {2}, {3}", m_id, cells[i - 1], i - 1, e.ToString());
            }

            return null;
        }

        protected List<T> toList<T>(List<string> cells, ref int i, char seperator = ',')
        {
            try
            {
                List<T> list = new List<T>();
                List<string> s = SystemHelper.fsplit(cells[i++], seperator);
                for (int c = 0; c < s.Count; ++c)
                {
                    list.Add((T)Convert.ChangeType(s[c], typeof(T), CultureInfo.InvariantCulture));
                }

                return list;
            }
            catch (Exception e)
            {
                if (Logx.isActive)
                    Logx.error("id {0}, value : {1}, column {2}, {3}", m_id, cells[i - 1], i - 1, e.ToString());
            }

            return null;
        }

        protected iMinMax toiMinMax(List<string> cells, ref int i, char seperator = ',')
        {
            return new iMinMax(toList<int>(cells, ref i, seperator));
        }

        protected fMinMax tofMinMax(List<string> cells, ref int i, char seperator = ',')
        {
            return new fMinMax(toList<float>(cells, ref i, seperator));
        }

        protected HashSet<T> toHashSet<T>(List<string> cells, ref int i, char seperator = ',')
        {
            try
            {
                HashSet<T> hashSet = new HashSet<T>();
                List<string> s = SystemHelper.fsplit(cells[i++], seperator);
                for (int c = 0; c < s.Count; ++c)
                {
                    hashSet.Add((T)Convert.ChangeType(s[c], typeof(T), CultureInfo.InvariantCulture));
                }

                return hashSet;
            }
            catch (Exception e)
            {
                if (Logx.isActive)
                    Logx.error("id {0}, value : {1}, column {2}, {3}", m_id, cells[i - 1], i - 1, e.ToString());
            }

            return null;
        }

        protected Color toHtmlColor(List<string> cells, ref int i)
        {
            var html = toString(cells, ref i);
            if (ColorUtility.TryParseHtmlString(html, out Color color))
                return color;

            if (Logx.isActive)
                Logx.error("Failed toHtmlColor {0}", html);

            return Color.white;
        }

        protected T toType<T>(List<string> cells, ref int i)
        {
            return (T)Convert.ChangeType(cells[i++], typeof(T), CultureInfo.InvariantCulture);
        }
    }
}