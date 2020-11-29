using System.Linq;
using System.Threading.Tasks;
using Assets.Scripts.Common.Util;
using Assets.Scripts.OmaWatch;
using Assets.Scripts.OmaWatch.GamePlay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class WinController : MonoBehaviour
    {
        public Transform LeaderboardRoot;
        public LeaderboardEntry LeaderboardEntryPrefab;
        public TMP_Text YourTimeText;
        public TMP_InputField DisplayNameInputField;
        public Button DisplayNameSubmitButton;

        protected void OnEnable()
        {
            StartUp().FireAndForget();
        }

        public void SubmitNewDisplayName()
        {
            var displayName = DisplayNameInputField.text;
            if (string.IsNullOrEmpty(displayName))
                return;
            SubmitNewDisplayNameAsync(displayName).FireAndForget();
        }

        public void BackToMenu()
        {
            UIManager.Instance.Fire(UITrigger.ToMainMenu);
        }

        private async Task StartUp()
        {
            DisplayNameSubmitButton.interactable = false;
            foreach (Transform child in LeaderboardRoot)
            {
                Destroy(child.gameObject);
            }

            YourTimeText.text = $"Your Time: {LevelCoordinator.Instance.Score.ToScoreDisplay()}";
            if (!PlayFabManager.Instance.IsLoggedIn)
                await PlayFabManager.Instance.Login();

            DisplayNameInputField.text = await PlayFabManager.Instance.GetLocalAccountDisplayName();
            var leaderboard = await PlayFabManager.Instance.GetLeaderboard("High Score");

            foreach (var leaderboardEntry in leaderboard.Leaderboard.OrderBy(e => e.Position))
            {
                var instance = Instantiate(LeaderboardEntryPrefab, LeaderboardRoot, false);
                instance.Entry = leaderboardEntry;
            }

            DisplayNameSubmitButton.interactable = true;
        }

        private async Task SubmitNewDisplayNameAsync(string displayName)
        {
            DisplayNameSubmitButton.interactable = false;
            if (!PlayFabManager.Instance.IsLoggedIn)
                await PlayFabManager.Instance.Login();
            await PlayFabManager.Instance.SetDisplayName(displayName);
            await StartUp();
        }
    }
}