using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Util;
using MessagePack;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Assets.Scripts.OmaWatch
{
    public class PlayFabManager : Singleton<PlayFabManager>
    {
        private string PlayerProfilePath => Path.Combine(Application.persistentDataPath, "profile.dat");
        private LoginResult _loginResult;
        private PlayerProfile _playerProfile;

        public bool IsLoggedIn => _loginResult != null;
        public string LocalPlayFabId => _loginResult?.PlayFabId;

        public PlayerProfile Profile
        {
            get
            {
                if (_playerProfile == null)
                    _playerProfile = LoadOrCreatePlayerProfile();
                return _playerProfile;
            }
        }

        public async Task Login()
        {
            if (_loginResult != null)
                throw new InvalidOperationException("PlayFab login was already done!");

            _playerProfile = LoadOrCreatePlayerProfile();
            var request = new LoginWithCustomIDRequest
            {
                CustomId = _playerProfile.Id.ToString(),
                CreateAccount = true
            };
            var completionSource = new TaskCompletionSource<LoginResult>();
            PlayFabError error = null;
            PlayFabClientAPI.LoginWithCustomID(request, completionSource.SetResult, e =>
            {
                error = e;
                completionSource.SetResult(null);
            });

            var loginResult = await completionSource.Task;

            if (error != null)
            {
                Debug.LogError(error.GenerateErrorReport());
                return;
            }

            Debug.Log($"Login successful: {loginResult.PlayFabId}");
            _loginResult = loginResult;
        }

        public async Task<string> GetLocalAccountDisplayName()
        {
            if (!IsLoggedIn)
                await Login();
            var accountInfoResult = await GetAccountInfo(_loginResult.PlayFabId);
            return accountInfoResult?.AccountInfo.TitleInfo.DisplayName;
        }

        public async Task UpdatePlayerStatistic(string statisticName, int value)
        {
            if (_loginResult == null)
                throw new InvalidOperationException("PlayFab is not logged in!");

            var request = new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate>
                {
                    new StatisticUpdate {StatisticName = statisticName, Value = value}
                }
            };
            var completionSource = new TaskCompletionSource<UpdatePlayerStatisticsResult>();
            PlayFabError error = null;

            PlayFabClientAPI.UpdatePlayerStatistics(request, completionSource.SetResult, e =>
            {
                error = e;
                completionSource.SetResult(null);
            });
            var updateResult = await completionSource.Task;
            if (error != null)
            {
                Debug.LogError(error.GenerateErrorReport());
                return;
            }

            Debug.Log($"Statistic {statisticName} successfully set to {value}");
        }

        public async Task<GetLeaderboardResult> GetLeaderboard(string statisticName, int startPosition = 0, int maxResultsCount = 10)
        {
            if (_loginResult == null)
                throw new InvalidOperationException("PlayFab is not logged in!");

            var request = new GetLeaderboardRequest
            {
                StatisticName = statisticName,
                StartPosition = startPosition,
                MaxResultsCount = maxResultsCount
            };
            var completionSource = new TaskCompletionSource<GetLeaderboardResult>();
            PlayFabError error = null;

            PlayFabClientAPI.GetLeaderboard(request, completionSource.SetResult, e =>
            {
                error = e;
                completionSource.SetResult(null);
            });

            var result = await completionSource.Task;

            if (error != null)
            {
                Debug.LogError(error.GenerateErrorReport());
                return null;
            }

            Debug.Log($"Leaderboard for {statisticName} retrieved successfully");
            return result;
        }

        public async Task<GetAccountInfoResult> GetAccountInfo(string playFabId)
        {
            if (_loginResult == null)
                throw new InvalidOperationException("PlayFab is not logged in!");

            var request = new GetAccountInfoRequest {PlayFabId = playFabId};
            var completionSource = new TaskCompletionSource<GetAccountInfoResult>();
            PlayFabError error = null;

            PlayFabClientAPI.GetAccountInfo(request, completionSource.SetResult, e =>
            {
                error = e;
                completionSource.SetResult(null);
            });

            var result = await completionSource.Task;

            if (error != null)
            {
                Debug.LogError(error.GenerateErrorReport());
                return null;
            }

            Debug.Log($"AccountInfo for {playFabId} retrieved successfully");
            return result;
        }

        public void SavePlayerProfile()
        {
            if (_playerProfile == null)
            {
                _playerProfile = LoadOrCreatePlayerProfile();
            }

            SavePlayerProfile(_playerProfile);
        }

        public async Task SetDisplayName(string displayName)
        {
            var request = new UpdateUserTitleDisplayNameRequest {DisplayName = displayName};
            var completionSource = new TaskCompletionSource<UpdateUserTitleDisplayNameResult>();
            PlayFabError error = null;
            PlayFabClientAPI.UpdateUserTitleDisplayName(request, completionSource.SetResult, e =>
            {
                error = e;
                completionSource.SetResult(null);
            });

            var result = await completionSource.Task;

            if (error != null)
            {
                Debug.LogError(error.GenerateErrorReport());
                return;
            }

            Debug.Log($"Display name set to {displayName} successfully");
        }

        private PlayerProfile LoadOrCreatePlayerProfile()
        {
            PlayerProfile result;
            if (File.Exists(PlayerProfilePath))
            {
                result = MessagePackSerializer.Deserialize<PlayerProfile>(File.ReadAllBytes(PlayerProfilePath));
                if (result != null)
                    return result;
            }

            result = new PlayerProfile
            {
                Id = Guid.NewGuid()
            };

            SavePlayerProfile(result);
            return result;
        }

        private void SavePlayerProfile(PlayerProfile playerProfile)
        {
            var serialized = MessagePackSerializer.Serialize(playerProfile);
            File.WriteAllBytes(PlayerProfilePath, serialized);
#if UNITY_EDITOR
            var json = MessagePackSerializer.SerializeToJson(playerProfile);
            var jsonPath = Path.ChangeExtension(PlayerProfilePath, "json");
            File.WriteAllText(jsonPath, json, Encoding.UTF8);
#endif
        }
    }
}