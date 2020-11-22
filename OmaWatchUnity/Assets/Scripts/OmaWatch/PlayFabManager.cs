using System;
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

        public async Task Login()
        {
            if (_loginResult != null)
                throw new InvalidOperationException("PlayFab login was already done!");

            var playerProfile = LoadOrCreatePlayerProfile();
            var request = new LoginWithCustomIDRequest
            {
                CustomId = playerProfile.Id.ToString(),
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
                Debug.LogError(error.GenerateErrorReport());
            else
            {
                Debug.Log($"Login successful: {loginResult.PlayFabId}");
                _loginResult = loginResult;
            }
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