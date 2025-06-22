using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace UnityHelper
{
    public partial class FileHelper
    {
        public static string makeDataPath(string filename)
        {
            if (Logx.isActive)
                Logx.assert(!string.IsNullOrEmpty(filename), "filename is null or empty");

            return combine(Application.dataPath, filename);
        }

        public static string makeProjectPath(string filename)
        {
            if (Logx.isActive)
                Logx.assert(!string.IsNullOrEmpty(filename), "filename is null or empty");

            return combine(Application.dataPath + "/../", filename);
        }

        /// <summary>
        /// IOS에서는 makePersistentDataPath()를 쓰는 경로를 UnityEngine.iOS.Device.SetNoBackupFlag(path)로
        /// 설정해 줘야 리젝을 안당한다고 한다.
        /// </summary>
        public static string makePersistentDataPath(string filename)
        {
            if (Logx.isActive)
                Logx.assert(!string.IsNullOrEmpty(filename), "filename is null or empty");

            return combine(Application.persistentDataPath, filename);
        }

        public static string combine(string path1, string path2)
        {
            if (Logx.isActive)
            {
                Logx.assert(!string.IsNullOrEmpty(path1), "path1 is null or empty");
                Logx.assert(!string.IsNullOrEmpty(path2), "path2 is null or empty");
            }

            return Path.Combine(path1, path2);
        }

        /// <summary>
        /// path 경로에 있는 파일 목록을 구한다.
        /// </summary>
        /// <param name="path"></param>
        /// /// <param name="withoutExtension"></param>
        /// <param name="exceptExtentions">제외 시킬려는 확장자, "."을 포함해야 된다.</param>
        /// <returns></returns>
        public static List<string> getFiles(string path, bool withoutExtension, HashSet<string> exceptExtentions)
        {
            if (Logx.isActive)
                Logx.assert(!string.IsNullOrEmpty(path), "path is null or empty");

            DirectoryInfo info = new DirectoryInfo(path);
            FileInfo[] fileInfos = info.GetFiles();

            List<string> filenames = new List<string>();

            for (int i = 0; i < fileInfos.Length; ++i)
            {
                string name = fileInfos[i].Name;
                string filename = (withoutExtension) ? Path.GetFileNameWithoutExtension(name) : Path.GetFileName(name);

                if (null == exceptExtentions)
                {
                    filenames.Add(filename);
                }
                else
                {
                    string ext = Path.GetExtension(name);
                    if (exceptExtentions.Contains(ext))
                        continue;

                    filenames.Add(filename);
                }
            }

            return filenames;
        }

        public static string readStream(string path)
        {
            if (Logx.isActive)
                Logx.assert(!string.IsNullOrEmpty(path), "path is null or empty");

            try
            {
                string text = null;
                using (StreamReader reader = new StreamReader(path))
                {
                    text = reader.ReadToEnd();
                }
                return text;
            }
            catch(Exception e)
            {
                if (Logx.isActive)
                    Logx.exception(e);
            }

            return null;            
        }

        public static void writeStream(string path, string data)
        {
            if (Logx.isActive)
            {
                Logx.assert(!string.IsNullOrEmpty(path), "path is null or empty");
                Logx.assert(!string.IsNullOrEmpty(data), "data is null or empty");
            }

            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    writer.Write(data);
                }
            }
            catch(Exception e)
            {
                if (Logx.isActive)
                    Logx.exception(e);
            }
        }

        public static T readJson<T>(string path)
        {
            if (Logx.isActive)
                Logx.assert(!string.IsNullOrEmpty(path), "path is null or empty");

            string json = readStream(path);
            return JsonHelper.fromJson<T>(json);
        }

        /// <param name="isPrettyPrint">true일 경우에는 파일 크기가 늘어납니다.</param>
        public static void writeJson(string path, object obj, bool isPrettyPrint = false)
        {
            if (Logx.isActive)
            {
                Logx.assert(!string.IsNullOrEmpty(path), "path is null or empty");
                Logx.assert(null != obj, "obj is null");
            }

            string json = JsonHelper.toJson(obj, isPrettyPrint);
            writeStream(path, json);
        }
    }
}