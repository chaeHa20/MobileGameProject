using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;

namespace UnityHelper
{
    public class BaseBuildSettingsEditor : Editor
    {
        private enum eBuild { Debug, Release };

        private eBuild m_selectedBuild = eBuild.Debug;
        private bool m_isDebugx = false;
        private string m_buildPath = null;
        private int m_buildCount = 0;
        private GUIStyle m_horizontalLine = null;
        private string[] m_buildStrings = null;

        protected virtual void Awake()
        {
            m_isDebugx = DebugSettings.instance.isDebug;
            m_buildStrings = SystemHelper.getEnumStrings<eBuild>();
            loadBuildInfo();
            initHorizontalLine();
        }

        private void initHorizontalLine()
        {
            m_horizontalLine = new GUIStyle();
            m_horizontalLine.normal.background = EditorGUIUtility.whiteTexture;
            m_horizontalLine.margin = new RectOffset(0, 0, 4, 4);
            m_horizontalLine.fixedHeight = 1;
        }

        public override void OnInspectorGUI()
        {
            base.DrawDefaultInspector();

            setIntentLevel(0);

            onVersionField();
            onBuildPath();
            onKeyPassward();
            onBuildAppBundle();
            onBuildSelect();
            onBuildSettings();
            onBuildButton();
            onApkName();
            onOpenPlayerSettings();
            onClearBuildCount();
            onOpenApkExplore();
        }

        private void setIntentLevel(int level)
        {
            EditorGUI.indentLevel = level;
        }

        private void onVersionField()
        {
            EditorGUILayout.Space();

#if UNITY_ANDROID
            onAndroidVersion();
#elif UNITY_IOS
        onIOSVersion();
#endif

            EditorGUILayout.LabelField("Build Count", m_buildCount.ToString());
        }

        private void onBuildPath()
        {
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("BuildPath", m_buildPath);

            if (GUILayout.Button("...", GUILayout.MaxWidth(30.0f)))
            {
                m_buildPath = EditorUtility.OpenFolderPanel("Select Path", "", "");
                saveBuildInfo();
            }

            EditorGUILayout.EndHorizontal();
        }

#if UNITY_ANDROID
        private void onAndroidVersion()
        {
            PlayerSettings.bundleVersion = EditorGUILayout.TextField("Version", PlayerSettings.bundleVersion);
            PlayerSettings.Android.bundleVersionCode = EditorGUILayout.IntField("BundleVersionCode", PlayerSettings.Android.bundleVersionCode);
        }
#elif UNITY_IOS
    private void onIOSVersion()
    {
        PlayerSettings.bundleVersion = EditorGUILayout.TextField("Version", PlayerSettings.bundleVersion);
        PlayerSettings.iOS.buildNumber = EditorGUILayout.TextField("BuildNumber", PlayerSettings.iOS.buildNumber);
    }
#endif

        private void onBuildSelect()
        {
            EditorGUILayout.Space();

            if (eBuild.Debug == m_selectedBuild)
                GUI.backgroundColor = Color.green;
            if (eBuild.Release == m_selectedBuild)
                GUI.backgroundColor = Color.yellow;

            m_selectedBuild = (eBuild)GUILayout.SelectionGrid((int)m_selectedBuild, m_buildStrings, 1, EditorStyles.radioButton);
        }

        private void onBuildSettings()
        {
            EditorGUILayout.Space();
            setIntentLevel(1);

            if (eBuild.Release == m_selectedBuild)
            {
                onReleaseSettings();
            }
            else if (eBuild.Debug == m_selectedBuild)
            {
                onDebugSettings();
            }
        }

        private void onReleaseSettings()
        {

        }

        private void onBuildAppBundle()
        {
            EditorUserBuildSettings.buildAppBundle = EditorGUILayout.Toggle("BuildAppBundle(Android)", EditorUserBuildSettings.buildAppBundle);
        }

        private void onDebugSettings()
        {
            m_isDebugx = EditorGUILayout.Toggle("active Debug", m_isDebugx);
        }

        private void onBuildButton()
        {
            EditorGUILayout.Space();

            string buttonLabel = string.Format("Build({0})", m_selectedBuild.ToString());
            if (GUILayout.Button(buttonLabel))
            {
                if (eBuild.Release == m_selectedBuild)
                {
                    buildRelease();
                }
                else if (eBuild.Debug == m_selectedBuild)
                {
                    buildDebug();
                }

                // "BeginLayoutGroup must be called first" 오류 때문에
                GUIUtility.ExitGUI();
            }
        }

        private void onApkName()
        {
            EditorGUILayout.LabelField(getApkName(m_buildCount + 1));
        }

        private void buildRelease()
        {
            bool oldActiveDebugx = DebugSettings.instance.isDebug;
            var oldLogxLevel = Logx.level;

            DebugSettings.instance.isDebug = false;
            Logx.level = Logx.eLevel.Off;

            build();

            DebugSettings.instance.isDebug = oldActiveDebugx;
            Logx.level = oldLogxLevel;
        }

        private void buildDebug()
        {
            bool oldActiveDebugx = DebugSettings.instance.isDebug;
            DebugSettings.instance.isDebug = m_isDebugx;

            build();

            DebugSettings.instance.isDebug = oldActiveDebugx;
        }

        private void build()
        {
            if (string.IsNullOrEmpty(m_buildPath))
            {
                EditorUtility.DisplayDialog("error", "Build path를 설정해주세요.", "확인");
                return;
            }

            PlayerSettings.SplashScreen.show = false;
            PlayerSettings.SplashScreen.showUnityLogo = false;

#if UNITY_ANDROID
            buildAndroid();
#elif UNITY_IOS
            buildIOS();
#endif
        }

#if UNITY_ANDROID
        private void buildAndroid()
        {
            if (!isValidKey())
            {
                EditorUtility.DisplayDialog("경고", "키를 설정해 주세요.", "확인");
                return;
            }

            incBuildCount();
            string apkName = getApkName(m_buildCount);
            BuildPipeline.BuildPlayer(EditorHelper.getScenes(), m_buildPath + "/" + apkName, BuildTarget.Android, BuildOptions.StrictMode);

            EditorUtility.DisplayDialog("Build completed", apkName, "Ok");

            openApkExplore();
        }

        private bool isValidKey()
        {
            if (string.IsNullOrEmpty(PlayerSettings.Android.keystorePass) ||
                string.IsNullOrEmpty(PlayerSettings.Android.keyaliasPass))
                return false;

            return true;
        }

#elif UNITY_IOS
    private void buildIOS()
    {
        
    }
#endif

        private void onKeyPassward()
        {
#if UNITY_ANDROID
            PlayerSettings.Android.keystorePass = EditorGUILayout.PasswordField("Key Store Password", PlayerSettings.Android.keystorePass);
            PlayerSettings.Android.keyaliasPass = EditorGUILayout.PasswordField("Key Alias Password", PlayerSettings.Android.keyaliasPass);
#endif
        }

        private string getApkName(int buildCount)
        {
            string productName = Application.productName;
            productName = productName.Replace(" ", "").Replace(":", "_");

            return string.Format("{0}_{1}_{2}_{3}.{4}", productName, PlayerSettings.bundleVersion, buildCount, m_selectedBuild.ToString(), getExeName());
        }

        private string getExeName()
        {
            if (EditorUserBuildSettings.buildAppBundle)
                return "aab";
            else
                return "apk";
        }

        private void incBuildCount()
        {
            ++m_buildCount;
            saveBuildInfo();
        }

        private void onOpenPlayerSettings()
        {
            GUI.backgroundColor = Color.white;

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            horizontalLine(Color.gray);

            GUI.backgroundColor = Color.black;

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.normal.textColor = Color.white;
            if (GUILayout.Button("Player Settings", buttonStyle))
            {
                EditorApplication.ExecuteMenuItem("Edit/Project Settings/Player");
            }
        }

        private void onClearBuildCount()
        {
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.normal.textColor = Color.white;
            if (GUILayout.Button("Clear BuildCount", buttonStyle))
            {
                m_buildCount = 0;
                saveBuildInfo();
            }
        }

        private void onOpenApkExplore()
        {
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.normal.textColor = Color.white;
            if (GUILayout.Button("Open Apk explore", buttonStyle))
            {
                openApkExplore();
            }
        }

        private void openApkExplore()
        {
            string path = m_buildPath.Replace("/", "\\");
            System.Diagnostics.Process.Start("explorer.exe", path);
        }

        private void horizontalLine(Color color)
        {
            var c = GUI.color;
            GUI.color = color;
            GUILayout.Box(GUIContent.none, m_horizontalLine);
            GUI.color = c;
        }

        private string getBuildInfoPath()
        {
            return Application.dataPath + "/../build.txt";
        }

        private void loadBuildInfo()
        {
            string path = getBuildInfoPath();

            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(path))
                {
                m_buildCount = System.Convert.ToInt32(reader.ReadLine());
                m_buildPath = reader.ReadLine();
                }
            }
            else
            {
                m_buildCount = 0;
                m_buildPath = "";
            }
        }

        private void saveBuildInfo()
        {
            string path = getBuildInfoPath();
            StreamWriter writer = new StreamWriter(path);

            writer.WriteLine(m_buildCount);
            writer.WriteLine(m_buildPath);

            writer.Close();
        }
    }
}
#endif