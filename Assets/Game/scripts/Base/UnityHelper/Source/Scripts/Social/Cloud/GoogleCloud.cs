using System;
#if UNITY_ANDROID && _SOCIAL_
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
#endif

namespace UnityHelper
{
    public class GoogleCloud : SocialCloud
    {
#if UNITY_ANDROID && _SOCIAL_
        private Action<eResult> m_callback = null;

        public override bool isLoggedIn()
        {
            return SocialHelper.instance.isLoggedIn(eSocialMedia.Google);
        }

        /// <param name="callback">success, error</param>
        public override void login(bool isDownloadProfile, Action<bool, string> callback)
        {
            SocialHelper.instance.login(eSocialMedia.Google, isDownloadProfile, callback);
        }

        public override void save(Action<eResult> callback)
        {
            eResult result = isValid();
            if (eResult.SUCCESS != result)
            {
                callback?.Invoke(result);
                return;
            }

            openWithAutomaticConflictResolution(callback, saveWithAutomaticConflictResolutionCallback);
        }

        public override void load(Action<eResult> callback)
        {
            eResult result = isValid();
            if (eResult.SUCCESS != result)
            {
                callback(result);
                return;
            }

            openWithAutomaticConflictResolution(callback, loadWithAutomaticConflictResolutionCallback);
        }

        private void openWithAutomaticConflictResolution(Action<eResult> callback, Action<SavedGameRequestStatus, ISavedGameMetadata> cloudCallback)
        {
            m_callback = callback;

            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.OpenWithAutomaticConflictResolution(m_filename,
                                                                DataSource.ReadNetworkOnly,
                                                                ConflictResolutionStrategy.UseLongestPlaytime,
                                                                cloudCallback);
        }

        private void saveWithAutomaticConflictResolutionCallback(SavedGameRequestStatus status, ISavedGameMetadata game)
        {
            if (Logx.isActive)
                Logx.trace("saveWithAutomaticConflictResolutionCallback status {0}", status);

            if (status == SavedGameRequestStatus.Success)
            {
                CloudDataParser dataParser = new CloudDataParser();
                saveGame(game, dataParser.getCloudByteData(m_crypto), getPlayedTime());
            }
            else
            {
                if (null != m_callback)
                {
                    m_callback(eResult.FAIL);
                }
            }
        }

        private void saveGame(ISavedGameMetadata game, byte[] savedData, TimeSpan totalPlaytime)
        {
            if (Logx.isActive)
                Logx.trace("saveGame totalPlayTime {0}, new {1}", totalPlaytime.Ticks, UnbiasedTime.Instance.Now().ToString());

            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

            SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();
            builder = builder
                .WithUpdatedPlayedTime(totalPlaytime)
                .WithUpdatedDescription(getNowSavedTimeString());

            SavedGameMetadataUpdate updatedMetadata = builder.Build();
            savedGameClient.CommitUpdate(game, updatedMetadata, savedData, saveGameCallback);
        }

        private void saveGameCallback(SavedGameRequestStatus status, ISavedGameMetadata game)
        {
            if (Logx.isActive)
                Logx.trace("saveGameCallback {0}", status);

            if (status == SavedGameRequestStatus.Success)
            {
                if (null != m_callback)
                    m_callback(eResult.SUCCESS);
            }
            else
            {
                if (null != m_callback)
                    m_callback(eResult.FAIL);
            }
        }

        private void loadWithAutomaticConflictResolutionCallback(SavedGameRequestStatus status, ISavedGameMetadata game)
        {
            if (Logx.isActive)
                Logx.trace("loadWithAutomaticConflictResolutionCallback status {0}", status);

            if (status == SavedGameRequestStatus.Success)
            {
                loadGame(game);
            }
            else
            {
                if (null != m_callback)
                    m_callback(eResult.FAIL);
            }
        }

        //데이터 읽기를 시도합니다.
        private void loadGame(ISavedGameMetadata game)
        {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.ReadBinaryData(game, loadGameCallback);
        }

        private void loadGameCallback(SavedGameRequestStatus status, byte[] data)
        {
            if (Logx.isActive)
                Logx.trace("loadGameCallback status {0}", status);

            if (status == SavedGameRequestStatus.Success)
            {
                CloudDataParser dataParser = new CloudDataParser();
                dataParser.setCloudByteData(data, m_crypto);

                if (null != m_callback)
                    m_callback(eResult.SUCCESS);
            }
            else
            {
                if (null != m_callback)
                    m_callback(eResult.FAIL);
            }
        }

        public override void getSavedDataTime(Action<string> callback)
        {
            eResult result = isValid();
            if (eResult.SUCCESS != result)
            {
                callback(null);
                return;
            }

            Action<string> _callback = callback;

            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.OpenWithAutomaticConflictResolution(m_filename,
                                                                DataSource.ReadNetworkOnly,
                                                                ConflictResolutionStrategy.UseLongestPlaytime,
                                                                (status, game) =>
                                                                {
                                                                    if (null == game)
                                                                    {
                                                                        if (Logx.isActive)
                                                                            Logx.trace("getSavedDateTime Description game is null");

                                                                        _callback(null);
                                                                    }
                                                                    else
                                                                    {
                                                                        if (Logx.isActive)
                                                                            Logx.trace("getSavedDateTime Description {0}", game.Description);

                                                                        if (status == SavedGameRequestStatus.Success)
                                                                            _callback(game.Description);
                                                                        else
                                                                            _callback(null);
                                                                    }
                                                                });
        }
#endif
    }
}