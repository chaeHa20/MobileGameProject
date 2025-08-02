using UnityEngine;
using System.Collections;
using System.IO;

#if UNITY_EDITOR
using OfficeOpenXml;

public class ExcelHelper
{
    public static Excel LoadExcel(string path)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        FileInfo file = new FileInfo(path);
        ExcelPackage ep = new ExcelPackage(file);
        Excel xls = new Excel(ep.Workbook);
        return xls;
    }

	public static Excel CreateExcel(string path) 
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        ExcelPackage ep = new ExcelPackage ();
		ep.Workbook.Worksheets.Add ("sheet");
		Excel xls = new Excel(ep.Workbook);
		SaveExcel (xls, path);
		return xls;
	}

    public static void SaveExcel(Excel xls, string path)
    {
        FileInfo output = new FileInfo(path);
        ExcelPackage ep = new ExcelPackage();
        for (int i = 0; i < xls.tables.Count; i++)
        {
            ExcelTable table = xls.tables[i];
            ExcelWorksheet sheet = ep.Workbook.Worksheets.Add(table.TableName);
            for (int row = 1; row <= table.NumberOfRows; row++) {
                for (int column = 1; column <= table.NumberOfColumns; column++) {
                    sheet.Cells[row, column].Value = table.GetValue(row, column);
                }
            }
        }
        ep.SaveAs(output);
    }
}
#endif