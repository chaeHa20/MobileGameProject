using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace UnityHelper
{
    public interface ITable
    {
        void load(string filename, Crypto crypto);
        TableRow getRow(int row);
        R getRow<R>(int row) where R : TableRow;
        bool existRow(int row);
        int rowCount { get; }
        int[] getSortIds();
        Dictionary<int, TableRow>.Enumerator getEnumerator();
    }

    public class Table<T> : ITable where T : TableRow, new()
    {
        private int m_firstRowId = 0;
        private int m_lastRowId = 0;
        private Dictionary<int, TableRow> m_rows = new Dictionary<int, TableRow>();

        public int rowCount => m_rows.Count;
        public int firstRowId => m_firstRowId;
        public int lastRowId => m_lastRowId;

        public Dictionary<int, TableRow>.Enumerator getEnumerator()
        {
            return m_rows.GetEnumerator();
        }

        protected List<T> toList()
        {
            var list = (from r in m_rows
                        select r.Value as T).ToList();

            return list;
        }

        public int[] getIds()
        {
            int[] ids = new int[m_rows.Count];
            m_rows.Keys.CopyTo(ids, 0);
            return ids;
        }

        public int[] getSortIds()
        {
            int[] ids = getIds();
            Array.Sort(ids);

            return ids;
        }

        public int[] getReverseIds()
        {
            int[] ids = getIds();
            Array.Reverse(ids);

            return ids;
        }

        public void load(string filename, Crypto crypto)
        {
            if (Logx.isActive)
                Logx.assert(!string.IsNullOrEmpty(filename), "filename is null or empty");

            TextAsset text = Resources.Load<TextAsset>(filename);

            if (Logx.isActive)
                Logx.assert(null != text, "text is null, not found {0}", filename);

            string strTable;
            if (crypto.isEmpty())
                strTable = text.text;
            else
                strTable = AES.Decode(text.text, crypto);

            List<string> lines = SystemHelper.fsplit(strTable, new string[] { "\r\n", "\r", "\n" });
            List<string> dataTypes = SystemHelper.fsplit(lines[0], '\t');

            // line 0,1,2는 가각 colum설명, 자료형, 타이틀이기 때문에 건너 뛴다.
            for (int line = 2; line < lines.Count; ++line)
            {
                if (string.IsNullOrEmpty(lines[line]))
                    continue;

                List<string> cells = SystemHelper.fsplit(lines[line], '\t');
                if (1 >= cells.Count)
                    continue;

                int i = 0;

                T row = new T();
                row.parse(cells, dataTypes, ref i);

                if (Debugx.isActive)
                {
                    if (m_rows.ContainsKey(row.id))
                    {
                        if (Logx.isActive)
                            Logx.error("{0} duplicated id {1}", filename, row.id);
                    }
                }

                m_rows.Add(row.id, row);

                if (0 == m_firstRowId)
                    m_firstRowId = row.id;

                m_lastRowId = row.id;
            }

            loadDone();
        }

        protected virtual void loadDone()
        {

        }

        public TableRow getRow(int row)
        {
            if (Logx.isActive)
                Logx.assert(0 < row, "Invalid row {0}", row);

            if (!m_rows.TryGetValue(row, out TableRow tableRow))
            {
                if (Logx.isActive)
                    Logx.error("Failed {0} getRow(), {1} is not found", GetType().Name, row);

                return null;
            }

            return tableRow;
        }

        public R getRow<R>(int row) where R : TableRow
        {
            if (Logx.isActive)
                Logx.assert(0 < row, "Invalid row {0}", row);

            if (!m_rows.TryGetValue(row, out TableRow tableRow))
            {
                if (Logx.isActive)
                    Logx.error("Failed getRow(), {0} is not found", row);

                return null;
            }

            return tableRow as R;
        }

        public R getLastRow<R>() where R : TableRow
        {
            return getRow<R>(m_lastRowId);
        }

        public bool existRow(int row)
        {
            if (Logx.isActive)
                Logx.assert(0 < row, "Invalid row {0}", row);

            return m_rows.ContainsKey(row);
        }

        /// <param name="callback">row</param>
        public void forEach<R>(Action<R> callback) where R : TableRow
        {
            if (Logx.isActive)
                Logx.assert(null != callback, "callback is null");

            foreach (var r in m_rows)
            {
                callback(r.Value as R);
            }
        }

        public int getRandId()
        {
            int r_index = UnityEngine.Random.Range(0, rowCount);
            int[] ids = getIds();
            return ids[r_index];
        }

        public R findRow<R>(Func<R, bool> callback) where R : TableRow
        {
            if (Logx.isActive)
                Logx.assert(null != callback, "callback is null");

            var e = getEnumerator();
            while (e.MoveNext())
            {
                R row = e.Current.Value as R;

                if (callback(row))
                    return row;
            }

            return null;
        }

        public R findRowLinq<R>(Func<R, bool> callback) where R : TableRow
        {
            var row = m_rows.FirstOrDefault(x => callback(x.Value as R));
            if (null == row.Value)
                return null;

            return row.Value as R;
        }

        public List<R> findRowsLinq<R>(Func<R, bool> callback) where R : TableRow
        {
            var rows = (from pair in m_rows
                        where callback(pair.Value as R)
                        select pair.Value as R).ToList();

            return rows;
        }

        public List<R> sortIdByAscending<R>(List<R> rows) where R : TableRow
        {
            var result = (from r in rows
                          orderby r.id ascending
                          select r).ToList();

            return result;
        }

        public List<R> toList<R>() where R : TableRow
        {
            var list = (from r in m_rows
                        select r.Value as R).ToList();

            return list;
        }

    }
}