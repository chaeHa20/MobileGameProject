using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text;

namespace UnityHelper
{
    /// <summary>
    /// http://wiki.unity3d.com/index.php/Load_Data_from_Excel_2003
    /// </summary>
    public class ExcelExporter
    {
        public struct Data
        {
            public string excelPath;
            public string tablePath;
            public string localeStringPath;
            public bool showDoneDialog;
            public Crypto crypto;
            public bool isEnableExportLocaleString;
        }

        public static bool export(Data data)
        {
            if (string.IsNullOrEmpty(data.excelPath))
                return false;
            if (string.IsNullOrEmpty(data.tablePath))
                return false;

            string filename = Path.GetFileName(data.excelPath);
            string exportPath = Path.Combine(Path.GetDirectoryName(data.excelPath), "Export/");

            checkDirectoryAndCreate(exportPath);

            bool isCrypto = !data.crypto.isEmpty();
            bool success = false;
            try
            {
                Excel xlsx = ExcelHelper.LoadExcel(data.excelPath);
                //xlsx.ShowLog();

                //string tablePath = Application.dataPath + "/Resources/Table/";

                foreach (ExcelTable table in xlsx.tables)
                {
                    StreamWriter tableSw = new StreamWriter(data.tablePath + "/" + table.TableName + "Table.txt");
                    StreamWriter exportSw = new StreamWriter(exportPath + "/" + table.TableName + "Table.txt");

                    findReservedKeywords(table, out int r, out Dictionary<string, int> symbols);
                    findColumnRange(table, r, out int sc, out int ec);
                    var init_r = r;

                    //Debug.LogFormat("table {0}, sc {1}, ec {2}", table.TableName, sc, ec);

                    StringBuilder sbStream = new StringBuilder();

                    for (; r <= table.NumberOfRows; ++r)
                    {
                        // 첫 컬럼값이 비어 있으면 이하는 무시하자
                        if (string.IsNullOrEmpty(table.GetCell(r, sc).Value))
                            break;

                        StringBuilder sbRow = new StringBuilder();

                        for (int c = sc; c <= ec; ++c)
                        {
                            ExcelTableCell cell = table.GetCell(r, c);

                            if (string.IsNullOrEmpty(cell.Value))
                                break;

                            sbRow.Append(parseSymbol(symbols, cell.Value));
                            if (c != ec)//table.NumberOfColumns)
                                sbRow.Append("\t");
                        }

                        string value = sbRow.ToString();

                        sbStream.AppendLine(value);
                        exportSw.WriteLine(value);
                    }

                    string strStream = sbStream.ToString();
                    if (isCrypto)
                        tableSw.Write(AES.Encode(strStream, data.crypto));
                    else
                        tableSw.Write(strStream);

                    tableSw.Close();
                    exportSw.Close();

                    if (data.isEnableExportLocaleString && "String" == table.TableName)
                    {
                        exportLocaleString(table, data.localeStringPath, sc, ec, init_r);
                    }
                }

                AssetDatabase.Refresh();

                Debug.Log(filename + " 익스포트가 완료 되었습니다.");

                if (data.showDoneDialog)
                    EditorUtility.DisplayDialog("엑셀 익스포트", filename + " 익스포트가 완료 되었습니다.", "확인");

                success = true;
            }
            catch (Exception e)
            {
                EditorUtility.DisplayDialog("엑셀 익스포트 에러 " + filename, e.Message, "확인");
            }

            return success;
        }

        private static void exportLocaleString(ExcelTable table, string localeStringPath, int sc, int ec, int init_r)
        {
            for (int c = sc+2; c <= ec; ++c)
            {
                StringBuilder sbRow = new StringBuilder();

                var language = table.GetCell(init_r, c).Value;

                for (int r = init_r + 1; r <= table.NumberOfRows; ++r)
                {
                    ExcelTableCell cell = table.GetCell(r, c);

                    if (string.IsNullOrEmpty(cell.Value))
                        break;

                    sbRow.AppendLine(cell.Value);
                }

                // The character used for Underline is not available in font asset 경고 때문에 밑줄을 포함시킨다.
                sbRow.AppendLine("_");

                var stringSw = new StreamWriter(localeStringPath + "/" + "String_" + language + ".txt");
                stringSw.Write(sbRow.ToString());
                stringSw.Close();
            }
        }

        private static void checkDirectoryAndCreate(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            if (di.Exists)
                return;

            di.Create();
        }

        private static void findColumnRange(ExcelTable table, int r, out int sc, out int ec)
        {
            for (sc = 1; sc <= table.NumberOfColumns; ++sc)
            {
                ExcelTableCell cell = table.GetCell(r, sc);
                if (!string.IsNullOrEmpty(cell.Value))
                    break;
            }

            // 끝 column을 찾는다.
            ec = table.NumberOfColumns;
            for (; ec >= 0; --ec)
            {
                ExcelTableCell cell = table.GetCell(r, ec);
                if (!string.IsNullOrEmpty(cell.Value))
                    break;
            }
        }

        private static void findReservedKeywords(ExcelTable table, out int r, out Dictionary<string, int> symbols)
        {
            symbols = new Dictionary<string, int>();

            for (r = 2; r <= table.NumberOfRows; ++r)
            {
                // 첫 컬럼값이 비어 있으면 이하는 무시하자
                string value = table.GetCell(r, 1).Value;
                if (string.IsNullOrEmpty(value))
                    break;

                if (string.Equals("#symbol", value))
                {
                    for (int c = 2; c <= table.NumberOfColumns; c+=2)
                    {
                        if (c + 1 > table.NumberOfColumns)
                        {
                            //Debug.LogErrorFormat("Invalid symbol column count, r = {0}, c = {1}, column count = {2}", r, c, table.NumberOfColumns);
                            break;
                        }

                        ExcelTableCell keyCell = table.GetCell(r, c);
                        ExcelTableCell valueCell = table.GetCell(r, c + 1);

                        if (string.IsNullOrEmpty(keyCell.Value))
                        {
                            //Debug.LogErrorFormat("Invalid key cell value, r = {0}, c = {1}, vlaue = {2}", r, c, keyCell.Value);
                            break;
                        }
                        if (string.IsNullOrEmpty(valueCell.Value))
                        {
                            //Debug.LogErrorFormat("Invalid value cell value, r = {0}, c = {1}, vlaue = {2}", r, c, valueCell.Value);
                            break;
                        }

                        if (!int.TryParse(valueCell.Value, out int symbolValue))
                        {
                            Debug.LogErrorFormat("Invalid symbol value r = {0}, c = {1}, vlaue = {2}", r, c, valueCell.Value);
                            break;
                        }

                        if (symbols.ContainsKey(keyCell.Value))
                        {
                            Debug.LogErrorFormat("Duplicated symbol key r = {0}, c = {1}, key = {2}", r, c, keyCell.Value);
                            continue;
                        }

                        symbols.Add(keyCell.Value, symbolValue);
                    }
                }
                else if (string.Equals("#comment", value))
                {
                    continue;
                }
                else
                {
                    break;
                }
            }
        }

        private static string parseSymbol(Dictionary<string, int> symbols, string value)
        {
            if (symbols.ContainsKey(value))
            {
                return symbols[value].ToString();
            }
            else
            {
                return value;
            }
        }
    }
}