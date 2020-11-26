using System.Diagnostics;
using System.Threading.Tasks;
using Assets.Scripts.Common.Util;
using Assets.Scripts.Messages;
using Assets.Scripts.OmaWatch.GamePlay.Interactions;
using Assets.Scripts.OmaWatch.Util;
using UnityEngine;

namespace Assets.Scripts.OmaWatch.GamePlay
{
    public class LevelCoordinator : Singleton<LevelCoordinator>
    {
        private readonly Stopwatch _currentLevelStopwatch = new Stopwatch();
        private bool _paused;

        public RocketConstructionSite ConstructionSite { get; set; }
        public int Score { get; private set; }
        public int CurrentScore => (int) -_currentLevelStopwatch.ElapsedMilliseconds;

        public bool CheckWinningCondition()
        {
            if (ConstructionSite == null)
                return false;
            if (!ConstructionSite.AllSlotsFilled)
                return false;
            StartWinningSequence().FireAndForget();
            return true;
        }

        public void TogglePause()
        {
            if (_paused)
                Resume();
            else
                Pause();
        }

        private void Pause()
        {
            _paused = true;
            Time.timeScale = 0;
            _currentLevelStopwatch.Stop();
            MessageBus.Instance.Publish(new GamePauseMessage(_paused));
        }

        private void Resume()
        {
            _paused = false;
            Time.timeScale = 1;
            _currentLevelStopwatch.Start();
            MessageBus.Instance.Publish(new GamePauseMessage(_paused));
        }

        private async Task StartWinningSequence()
        {
            Score = (int) _currentLevelStopwatch.ElapsedMilliseconds * -1;
            await PlayFabManager.Instance.UpdatePlayerStatistic("High Score", Score);
            _currentLevelStopwatch.Stop();
            await Task.Delay(5000);
            MessageBus.Instance.Publish(new GameWinMessage());
        }

        public void StartLevel(string levelName)
        {
            _currentLevelStopwatch.Restart();
        }
    }
}