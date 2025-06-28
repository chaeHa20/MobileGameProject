using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public class LocalDataHelper : MonoSingleton<LocalDataHelper>
    {
        /// <summary>
        /// data name, local data
        /// </summary>
        private Dictionary<string, LocalData> m_datas = new Dictionary<string, LocalData>();
        private Dictionary<int, LocalDataProtocol> m_protocols = new Dictionary<int, LocalDataProtocol>();
        private Crypto m_crypto = new Crypto();

        public virtual void initialize(Crypto crypto)
        {
            m_crypto = crypto;

            initProtocols();
            initDatas();
        }

        protected virtual void initDatas()
        {
            m_datas.Clear();
        }

        protected virtual void initProtocols()
        {
            m_protocols.Clear();
        }

        public DATA addData<DATA>(string dataName, int id) where DATA : LocalData, new()
        {
            DATA data;
            if (PlayerPrefs.HasKey(dataName))
            {
                data = getPlayerPrefsData<DATA>(dataName);
                data.deserialize();
                data.checkValid();
            }
            else
            {
                data = new DATA();
                data.initialize(dataName, id);
                setPlayerPrefsData<DATA>(data);
            }

            data.isUsed = true;

            m_datas.Add(dataName, data);

            data.addSubDatas(this);

            return data;
        }

        protected void addProtocol<P>(int pid, bool isSaveData) where P : LocalDataProtocol, new()
        {
            if (m_protocols.ContainsKey(pid))
            {
                if (Logx.isActive)
                    Logx.error("Duplicated protocol id {0}", pid);
                return;
            }

            m_protocols.Add(pid, new P() { isSaveData = isSaveData });
        }

        public virtual void request<U>(Req_LocalData req, Action<U> callback) where U : Res_LocalData
        {
            if (Logx.isActive)
            {
                Logx.assert(null != req, "req is null");
                Logx.trace("<color=red>{0} : {1}</color>", req.GetType().Name, JsonHelper.toJson(req));
            }

            if (m_protocols.TryGetValue(req.pid, out LocalDataProtocol protocol))
            {
                protocol.process(req, (_res) =>
                {
                    U res = _res as U;

                    if (Logx.isActive)
                        Logx.trace("<color=lightblue>{0} : {1}</color>", res.GetType().Name, JsonHelper.toJson(res));

                    if (res.isSuccess)
                    {
                        if (protocol.isSaveData)
                        {
                            saveUsedDatas(req.dataType);
                            saveIosRestoreDatas(req.pid);
                        }
                        // 디버깅 때문에 OnApplicationQuit()가 호출 안되는 경우가 있다.
                        else if (Debugx.isActive)
                        {
                            saveUsedDatas(req.dataType);
                        }
                    }
                    else
                    {
                        openErrorMsgBox(res.error, res.errorArgs, errorToText(res.error));
                    }

                    if (null != callback)
                        callback(res);
                });
            }
            else
            {
                if (Logx.isActive)
                    Logx.error("Failed find protocol {0}", req.GetType().Name);
            }
        }

        public void request(MultiPacketRequester multiRequester, Action<bool, List<Res_LocalData>> callback)
        {
            var isSuccess = true;
            int resCount = 0;
            List<Res_LocalData> results = new List<Res_LocalData>();
            var bufferCount = multiRequester.bufferCount;
            for (int i = 0; i < bufferCount; ++i)
            {
                var buffer = multiRequester.getBuffer(i);
                request<Res_LocalData>(buffer.req, (res) =>
                {
                    if (!res.isSuccess)
                        isSuccess = false;

                    results.Add(res);
                    buffer.callback?.Invoke(res);

                    ++resCount;
                    if (resCount == bufferCount)
                    {
                        callback?.Invoke(isSuccess, results);
                    }
                });
            }
        }

        protected virtual string errorToText(int error)
        {
            return error.ToString();
        }

        protected virtual void openErrorMsgBox(int error, object[] errorArgs, string errorText)
        {

        }

        public bool isExistData(string dataName)
        {
            return m_datas.ContainsKey(dataName);
        }

        public DATA getData<DATA>(string dataName) where DATA : LocalData
        {
            if (Logx.isActive)
                Logx.assert(!string.IsNullOrEmpty(dataName), "dataName is null or empty");

            if (m_datas.TryGetValue(dataName, out LocalData data))
            {
                data.isUsed = true;
                return data as DATA;
            }
            else
            {
                if (PlayerPrefs.HasKey(dataName))
                {
                    data = getPlayerPrefsData<DATA>(dataName);
                    data.deserialize();
                    data.checkValid();
                    data.isUsed = true;
                    m_datas.Add(dataName, data);

                    return data as DATA;
                }
            }

            //if (Logx.isActive)
            //    Logx.error("Failed getData(), dataName is {0}", dataName);

            return null;
        }

        private DATA getPlayerPrefsData<DATA>(string dataName) where DATA : LocalData
        {
            string value = PlayerPrefs.GetString(dataName);
            return JsonHelper.fromJson<DATA>(value, m_crypto);
        }

        private void setPlayerPrefsData<DATA>(DATA data) where DATA : LocalData
        {
            string value = JsonHelper.toJson(data, m_crypto);
            PlayerPrefs.SetString(data.name, value);
            PlayerPrefs.Save();
        }

        public void setCloudData(string data, Crypto crypto)
        {
            if (string.IsNullOrEmpty(data))
                return;

            PlayerPrefsNodes nodes = JsonHelper.fromJson<PlayerPrefsNodes>(data, crypto);
            foreach (var node in nodes.nodes)
            {
                PlayerPrefs.SetString(node.key, AES.Encode(node.value, m_crypto));
            }
            PlayerPrefs.Save();

            initDatas();
        }

        public string getCloudData(Crypto crypto)
        {
            PlayerPrefsNodes nodes = new PlayerPrefsNodes();
            foreach (var pair in m_datas)
            {
                var data = pair.Value;
                data.serialize();

                if (Logx.isActive)
                {
                    Logx.trace("{0} : {1}", data.name, data.toString());
                }

                nodes.add(data.name, pair.Value);
            }

            return JsonHelper.toJson(nodes, crypto);
        }

        public void saveUsedDatas(eLocalData dataType)
        {
            if (m_datas.TryGetValue(dataType.ToString(), out LocalData data))
            {
                if (data.isUsed)
                {
                    data.isUsed = false;
                    data.serialize();
                    setPlayerPrefsData(data);
                }
            }
        }// 모든 데이터를 매번 저장시 부하 있을 수 있어 필요 데이터만 따로 저장

        public void saveUsedDatas()
        {
            foreach (var pair in m_datas)
            {
                LocalData data = pair.Value;
                if (data.isUsed)
                {
                    data.isUsed = false;
                    data.serialize();
                    setPlayerPrefsData(data);
                }
            }
        }// 모든 데이터 저장

        public void setDailyReset()
        {
            foreach (var pair in m_datas)
            {
                pair.Value.setDailyReset();
            }
        }

        protected void testProtocol()
        {
#if UNITY_EDITOR
            foreach (var pair in m_protocols)
            {
                pair.Value.test();
            }
#endif
        }

        protected void traceDatas()
        {
#if UNITY_EDITOR
            foreach (var pair in m_datas)
            {
                Logx.trace(JsonHelper.toJson(pair.Value, true));
            }
#endif
        }

        protected virtual void saveIosRestoreDatas(int pid)
        {

        }

        void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                saveUsedDatas();
            }
        }

        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();

            saveUsedDatas();
        }
    }
}