using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Diagnostics;

namespace UnityHelper
{
    public class ExcelExportWindow : EditorWindow
    {
        private string m_version = "1.6.2";
        private string m_excelPath = "";
        private string m_exportPath = "";
        private string m_exportLocaleStringPath = "";
        private bool m_isEnableExportLocaleString = true;
        private string m_svnExePath = "";
        private string[] m_excelFiles = null;
        private int m_selectedExcelIndex = 0;
        private GUIStyle m_horizontalLine = null;
        private List<string> m_sheetNames = new List<string>();
        private Vector2 m_sheetScrollPosition = Vector3.zero;
        private bool m_isShowSheet = false;
        private bool m_isInitialized = false;

        [MenuItem("Table/Export")]
        static void Init()
        {
            ExcelExportWindow window = (ExcelExportWindow)EditorWindow.GetWindow(typeof(ExcelExportWindow));
            window.Show();
            window.initialize();
        }

        public void initialize()
        {
            m_isInitialized = true;

            loadPath();
            initHorizontalLine();
        }

        private void initHorizontalLine()
        {
            m_horizontalLine = new GUIStyle();
            m_horizontalLine.normal.background = EditorGUIUtility.whiteTexture;
            m_horizontalLine.margin = new RectOffset(0, 0, 4, 4);
            m_horizontalLine.fixedHeight = 1;
        }

        private void OnGUI()
        {
            if (!m_isInitialized)
                initialize();

            onVersion();
            onExcelPath();
            onExportPath();
            onExportLocaleStringPath();
            onExcelFiles();
            onSvn();
            onExport();
            onSheet();
        }

        private void onVersion()
        {
            GUILayout.Label("Ver " + m_version, EditorStyles.boldLabel);
        }

        private void onExcelPath()
        {
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            EditorGUIUtility.labelWidth = 70.0f;
            EditorGUILayout.LabelField("Excel Path", m_excelPath);

            if (GUILayout.Button("...", GUILayout.MaxWidth(30.0f)))
            {
                m_excelPath = EditorUtility.OpenFolderPanel("Select Path", "", "");
                savePath();

                if (!string.IsNullOrEmpty(m_excelPath))
                    loadExcelFiles(m_excelPath);
            }

            EditorGUILayout.EndHorizontal();
        }

        private void onExportPath()
        {
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            EditorGUIUtility.labelWidth = 70.0f;
            EditorGUILayout.LabelField("ExportPath", m_exportPath);

            if (GUILayout.Button("...", GUILayout.MaxWidth(30.0f)))
            {
                m_exportPath = EditorUtility.OpenFolderPanel("Select Path", "", "");
                savePath();
            }

            EditorGUILayout.EndHorizontal();
        }

        private void onExportLocaleStringPath()
        {
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            EditorGUIUtility.labelWidth = 70.0f;
            EditorGUILayout.LabelField("ExportLocaleStringPath", m_exportLocaleStringPath);

            if (GUILayout.Button("...", GUILayout.MaxWidth(30.0f)))
            {
                m_exportLocaleStringPath = EditorUtility.OpenFolderPanel("Select Path", "", "");
                savePath();
            }

            EditorGUILayout.EndHorizontal();
        }

        private void loadPath()
        {
            string tableInfoPath = getTableInfoPath();
            if (File.Exists(tableInfoPath))
            {
                using (StreamReader reader = new StreamReader(tableInfoPath))
                {
                    m_excelPath = reader.ReadLine();
                m_svnExePath = reader.ReadLine();
                m_exportPath = reader.ReadLine();
                    m_exportLocaleStringPath = reader.ReadLine();
                }
            }

            if (!string.IsNullOrEmpty(m_excelPath))
                loadExcelFiles(m_excelPath);
        }

        private void savePath()
        {
            string tableInfoPath = getTableInfoPath();
            StreamWriter writer = new StreamWriter(tableInfoPath);

            writer.WriteLine(m_excelPath);
            writer.WriteLine(m_svnExePath);
            writer.WriteLine(m_exportPath);
            writer.WriteLine(m_exportLocaleStringPath);

            writer.Close();
        }

        private string getTableInfoPath()
        {
            return Application.dataPath + "/../TableExcelPath.txt";
        }

        private void onExcelFiles()
        {
            EditorGUILayout.Space();

            if (null == m_excelFiles)
                return;

            EditorGUILayout.BeginHorizontal();

            /**
             * excel list
             * */
            int oldSelectExcelIndex = m_selectedExcelIndex;
            m_selectedExcelIndex = EditorGUILayout.Popup("Excels", m_selectedExcelIndex, m_excelFiles);
            if (oldSelectExcelIndex != m_selectedExcelIndex)
            {
                m_sheetNames.Clear();
                setSheets(m_selectedExcelIndex);
            }

            /*
             * open
             * */
            if (GUILayout.Button("Open", GUILayout.MaxWidth(50.0f)))
            {
                string excelFilePath = Path.Combine(m_excelPath, m_excelFiles[m_selectedExcelIndex]);
                Process.Start(excelFilePath);
            }

            EditorGUILayout.EndHorizontal();
        }

        private void loadExcelFiles(string excelPath)
        {
            List<string> excels = new List<string>();
            DirectoryInfo di = new DirectoryInfo(excelPath);
            if (!di.Exists)
            {
                EditorUtility.DisplayDialog("경고", "경로가 잘못 됐거나 디렉토리가 비어 있습니다.", "확인");
                return;
            }

            foreach (FileInfo fi in di.GetFiles("*.xlsx"))
            {
                // 엑셀 임시 저장 파일 체크
                if (fi.Name[0] == '~')
                    continue;

                excels.Add(fi.Name);
            }

            m_excelFiles = excels.ToArray();
        }

        private string getExcelPath(int excelFileIndex)
        {
            string excelFileName = m_excelFiles[excelFileIndex];
            return Path.Combine(m_excelPath, excelFileName);
        }

        private void onExport()
        {
            if (null == m_excelFiles)
                return;

            EditorGUILayout.Space();
            horizontalLine(Color.gray);

            onEnableExportLocaleString();
            onExportOne();
            onExportAll();
        }

        private void onEnableExportLocaleString()
        {
            EditorGUILayout.Space();

            m_isEnableExportLocaleString = GUILayout.Toggle(m_isEnableExportLocaleString, "Enable Export Locale String");
        }

        private void onExportOne()
        {
            EditorGUILayout.Space();

            if (m_selectedExcelIndex >= m_excelFiles.Length)
                return;

            Color oldColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Export"))
            {
                Crypto crypto = FileHelper.readJson<Crypto>("TableCrypto.txt");

                var exportData = new ExcelExporter.Data
                {
                    excelPath = getExcelPath(m_selectedExcelIndex),
                    tablePath = m_exportPath,
                    localeStringPath = m_exportLocaleStringPath,
                    showDoneDialog = false,
                    crypto = crypto,
                    isEnableExportLocaleString = m_isEnableExportLocaleString,
                };
                ExcelExporter.export(exportData);

                EditorUtility.DisplayDialog("엑셀 익스포트", " 익스포트가 완료 되었습니다.", "확인");
            }
            GUI.backgroundColor = oldColor;
        }

        private void onExportAll()
        {
            if (GUILayout.Button("Export All"))
            {
                Crypto crypto = FileHelper.readJson<Crypto>("TableCrypto.txt");
                for (int i = 0; i < m_excelFiles.Length; ++i)
                {
                    var exportData = new ExcelExporter.Data
                    {
                        excelPath = getExcelPath(i),
                        tablePath = m_exportPath,
                        localeStringPath = m_exportLocaleStringPath,
                        showDoneDialog = false,
                        crypto = crypto,
                        isEnableExportLocaleString = m_isEnableExportLocaleString,
                    };
                    ExcelExporter.export(exportData);
                }

                EditorUtility.DisplayDialog("엑셀 익스포트", " 익스포트가 완료 되었습니다.", "확인");
            }
        }

        private void setSheets(int excelFileIndex)
        {
            if (!m_isShowSheet)
                return;

            string excelPath = getExcelPath(excelFileIndex);
            Excel xlsx = ExcelHelper.LoadExcel(excelPath);

            m_sheetNames.Clear();
            foreach (ExcelTable table in xlsx.tables)
            {
                m_sheetNames.Add(table.TableName);
            }
        }

        private void onSvn()
        {
            horizontalLine(Color.gray);

            GUILayout.Label("Svn", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("svn 실행파일은 TortoiseProc.exe 입니다.", MessageType.Info);
            onSvnExePath();
            onSvnUpdate();
        }

        private void onSvnExePath()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUIUtility.labelWidth = 100.0f;
            EditorGUILayout.LabelField("SvnExePath", m_svnExePath);

            if (GUILayout.Button("...", GUILayout.MaxWidth(30.0f)))
            {
                string[] filters = new string[2];
                filters[0] = "exe files";
                filters[1] = "exe";
                m_svnExePath = EditorUtility.OpenFilePanelWithFilters("Select File", "C:\\Program Files\\TortoiseSVN\\bin", filters);
                savePath();
            }

            EditorGUILayout.EndHorizontal();
        }

        private void onSvnUpdate()
        {
            if (GUILayout.Button("Svn Update"))
            {
                try
                {
                    var info = new ProcessStartInfo(m_svnExePath, "/command:update /path:" + m_excelPath);
                    Process.Start(info);
                }
                catch (System.Exception e)
                {
                    EditorUtility.DisplayDialog("에러", e.ToString(), "확인");
                }
            }
        }

        private void onSheet()
        {
            if (null == m_excelFiles)
                return;

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            onSheetShowToggle();
            onSheetView();
        }

        private void onSheetShowToggle()
        {

            m_isShowSheet = GUILayout.Toggle(m_isShowSheet, "Show Sheet", "Button", GUILayout.Width(100));

            if (0 == m_sheetNames.Count)
                setSheets(m_selectedExcelIndex);
        }

        private void onSheetView()
        {
            if (!m_isShowSheet)
                return;

            GUILayout.BeginVertical("BOX");

            m_sheetScrollPosition = GUILayout.BeginScrollView(m_sheetScrollPosition, GUIStyle.none);

            for (int i = 0; i < m_sheetNames.Count; ++i)
            {
                GUILayout.Label(m_sheetNames[i]);
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        private void horizontalLine(Color color)
        {
            var c = GUI.color;
            GUI.color = color;
            GUILayout.Box(GUIContent.none, m_horizontalLine);
            GUI.color = c;
        }

    }
}