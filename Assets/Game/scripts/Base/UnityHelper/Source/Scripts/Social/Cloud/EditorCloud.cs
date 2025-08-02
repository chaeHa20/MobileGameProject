using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace UnityHelper
{
#if UNITY_EDITOR
    public class EditorCloud : SocialCloud
    {
        public override bool isLoggedIn()
        {
            return true;
        }

        /// <param name="callback">success, error</param>
        public override void login(bool isDownloadProfile, Action<bool, string> callback)
        {
            callback?.Invoke(true, null);
        }

        public override void save(Action<eResult> callback)
        {
            CloudDataParser dataParser = new CloudDataParser();

            string cloudData = dataParser.getData(m_crypto);

            string path = Application.dataPath + "/../" + m_filename;
            FileHelper.writeStream(path, cloudData);

            callback?.Invoke(eResult.SUCCESS);
        }

        public override void load(Action<eResult> callback)
        {
            string path = Application.dataPath + "/../" + m_filename;

            if (File.Exists(path))
            {
                string cloudData = FileHelper.readStream(path);
                CloudDataParser dataParser = new CloudDataParser();
                dataParser.setData(cloudData, m_crypto);

                callback?.Invoke(eResult.SUCCESS);
            }
            else
            {
                callback?.Invoke(eResult.FAIL);
            }
        }

        public override void getSavedDataTime(Action<string> callback)
        {
            callback?.Invoke(getNowSavedTimeString());
        }
    }
#endif
}