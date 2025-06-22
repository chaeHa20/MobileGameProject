using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;

public class StringRow : TableRow
{
    private string m_code;
    private string m_text;

    public string code { get { return m_code; } }
    public string text { get { return m_text; } }

    public override void parse(List<string> cells, List<string> dataTypes, ref int i)
    {
        base.parse(cells, dataTypes, ref i);

        m_code = toString(cells, ref i);

        try
        {
            i += (int)LanguageHelper.language;
            m_text = toString(cells, ref i, true);
        }
        catch (System.Exception e)
        {
            if (Logx.isActive)
            {
                Logx.error("Failed parse string table, id {0}, code {1}", id, m_code);
                Logx.exception(e);
            }
        }

#if UNITY_EDITOR
        checkValidTag(m_text);
#endif
    }

#if UNITY_EDITOR
    private void checkValidTag(string text)
    {
        int s_index = text.IndexOf("<color");
        int e_index = text.IndexOf("</color");
        if (0 > s_index && 0 > e_index)
            return;

        if (0 < s_index && 0 > e_index || 0 > s_index && 0 < e_index)
        {
            if (Logx.isActive)
                Logx.warn("Invalid string {0}", id);
            return;
        }

        s_index = 0;
        while (true)
        {
            s_index = text.IndexOf("<color", s_index);
            if (0 > s_index)
                return;

            e_index = text.IndexOf("</color", s_index);

            if (0 > s_index && 0 > e_index)
                return;

            if (0 > s_index || 0 > e_index)
            {
                if (Logx.isActive)
                    Logx.warn("Invalid string {0}", id);
                return;
            }

            ++s_index;
        }
    }
#endif
}

public class StringTable : Table<StringRow>
{
    private Dictionary<string, StringRow> m_rowsByCode = new Dictionary<string, StringRow>();

    protected override void loadDone()
    {
        base.loadDone();

        setRowsByCode();
    }

    private void setRowsByCode()
    {
        var e = getEnumerator();
        while (e.MoveNext())
        {
            var row = e.Current.Value as StringRow;

            m_rowsByCode.Add(row.code, row);
        }
    }

    public string getString(string code)
    {
        if (Logx.isActive)
            Logx.assert(null != code, "Null string code");

        if (m_rowsByCode.TryGetValue(code, out StringRow row))
            return row.text;

        if (Logx.isActive)
            Logx.error("Failed find string code {0}", code);

        return "";
    }
}
