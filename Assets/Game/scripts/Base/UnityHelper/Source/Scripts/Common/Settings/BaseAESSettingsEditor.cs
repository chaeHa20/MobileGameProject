using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace UnityHelper
{
    public class BaseAESSettingsEditor : Editor
    {
        private string m_encodedText = null;
        private string m_decodeText = null;
        private string m_key = null;
        private string m_iv = null;

        public override void OnInspectorGUI()
        {
            base.DrawDefaultInspector();

            onSave();
            onLoad();
            onDecode();
        }

        private void onSave()
        {
            if (GUILayout.Button("save"))
            {
                BaseAESSettings aesSettings = target as BaseAESSettings;
                FileHelper.writeJson(FileHelper.makeProjectPath("TableCrypto.txt"), aesSettings.table);
                FileHelper.writeJson(FileHelper.makeProjectPath("LocalDataCrypto.txt"), aesSettings.localData);
            }
        }

        private void onLoad()
        {
            if (GUILayout.Button("load"))
            {
                BaseAESSettings aesSettings = target as BaseAESSettings;
                aesSettings.table = FileHelper.readJson<Crypto>(FileHelper.makeProjectPath("TableCrypto.txt"));
                aesSettings.localData = FileHelper.readJson<Crypto>(FileHelper.makeProjectPath("LocalDataCrypto.txt"));
            }
        }

        private void onDecode()
        {
            EditorGUILayout.Space();

            m_encodedText = EditorGUILayout.TextField("encodedText", m_encodedText);
            m_key = EditorGUILayout.TextField("key", m_key);
            m_iv = EditorGUILayout.TextField("iv", m_iv);

            if (GUILayout.Button("decode"))
            {
                if (string.IsNullOrEmpty(m_encodedText))
                    return;
                if (string.IsNullOrEmpty(m_key))
                    return;
                if (string.IsNullOrEmpty(m_iv))
                    return;

                m_decodeText = AES.Decode(m_encodedText, new Crypto(m_iv, m_key));

                if (Logx.isActive)
                    Logx.trace(m_decodeText);
            }

            EditorGUILayout.TextArea(m_decodeText);
        }
    }
}

#endif