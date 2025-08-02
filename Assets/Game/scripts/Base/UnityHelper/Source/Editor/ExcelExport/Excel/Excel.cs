using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using OfficeOpenXml;

public class Excel
{
    public List <ExcelTable> tables = new List<ExcelTable>();

    public Excel()
    {
		
    }

    public Excel(ExcelWorkbook wb)
    {
        for (int i = 0; i < wb.Worksheets.Count; i++)
        {
            ExcelWorksheet sheet = wb.Worksheets[i];
            ExcelTable table = new ExcelTable(sheet);
            tables.Add(table);
        }
    }

    public void ShowLog() {
        for (int i = 0; i < tables.Count; i++)
        {
            tables[i].ShowLog();
        }
    }

    public void AddTable(string name) {
        ExcelTable table = new ExcelTable();
        table.TableName = name;
        tables.Add(table);
    }

}
#endif