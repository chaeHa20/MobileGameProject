using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using System.IO;
using System.Collections.Generic;

#if UNITY_IPHONE
using UnityEditor.iOS.Xcode;

public class UnityHelperPostProcess
{
#if !UNITY_2019_3_OR_NEWER
        private const string UnityMainTargetName = "Unity-iPhone";
#endif

    [PostProcessBuildAttribute(int.MaxValue)]
    public static void ChangeXcodePlist(BuildTarget buildTarget, string buildPath)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            var projectPath = PBXProject.GetPBXProjectPath(buildPath);
            var project = new PBXProject();
            project.ReadFromFile(projectPath);
            
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(buildPath + "/Info.plist"));
            if (plist != null)
            {
            /*
                // Get root
                PlistElementDict rootDict = plist.root;

                File.WriteAllText(buildPath + "/Info.plist", plist.WriteToString());
            */
            }            

#if UNITY_2019_3_OR_NEWER
            var unityMainTargetGuid = project.GetUnityMainTargetGuid();
            var unityFrameworkTargetGuid = project.GetUnityFrameworkTargetGuid();
#else
            var unityMainTargetGuid = project.TargetGuidByName(UnityMainTargetName);
            var unityFrameworkTargetGuid = project.TargetGuidByName(UnityMainTargetName);
#endif

            project.SetBuildProperty(unityMainTargetGuid, "ENABLE_BITCODE", "NO");
            project.SetBuildProperty(unityMainTargetGuid, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");

            /* 에러나는 경우에만 하자
            project.SetBuildProperty(unityFrameworkTargetGuid, "ENABLE_BITCODE", "NO");
            project.SetBuildProperty(unityFrameworkTargetGuid, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");
            project.SetBuildProperty(unityFrameworkTargetGuid, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "NO");            
            */

            project.WriteToFile(projectPath);
        }
    }
}
#endif