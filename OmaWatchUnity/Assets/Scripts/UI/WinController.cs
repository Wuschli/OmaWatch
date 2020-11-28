using System.Linq;
using System.Threading.Tasks;
using Assets.Scripts.Common.Util;
using Assets.Scripts.OmaWatch;
using Assets.Scripts.OmaWatch.GamePlay;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class WinController : MonoBehaviour
    {
        public Transform LeaderboardRoot;
        public LeaderboardEntry LeaderboardEntryPrefab;
        public TMP_Text YourTimeText;

        protected void OnEnable()
        {
            StartUp().FireAndForget();
        }

        public void BackToMenu()
        {
            UIManager.Instance.Fire(UITrigger.ToMainMenu);
        }

        private async Task StartUp()
        {
            foreach (Transform child in LeaderboardRoot)
            {
                Destroy(child.gameObject);
            }

            YourTimeText.text = $"Your Time: {LevelCoordinator.Instance.Score.ToScoreDisplay()}";
            if (!PlayFabManager.Instance.IsLoggedIn)
                await PlayFabManager.Instance.Login();
            var leaderboard = await PlayFabManager.Instance.GetLeaderboard("High Score");

            foreach (var leaderboardEntry in leaderboard.Leaderboard.OrderBy(e => e.Position))
            {
                var instance = Instantiate(LeaderboardEntryPrefab, LeaderboardRoot, false);
                instance.Entry = leaderboardEntry;
            }
        }
    }
}