using System.Diagnostics;
using System.Threading.Tasks;
using Assets.Scripts.Common.Util;
using Assets.Scripts.Messages;
using Assets.Scripts.OmaWatch.GamePlay.Interactions;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.OmaWatch.GamePlay
{
    public class LevelCoordinator : Singleton<LevelCoordinator>
    {
        private const string TutorialSceneName = "Tutorial";
        private readonly Stopwatch _currentLevelStopwatch = new Stopwatch();
        private bool _paused;

        public RocketConstructionSite ConstructionSite { get; set; }

        public bool SkipTutorial
        {
            get => PlayFabManager.Instance.Profile.SkipTutorial;
            set
            {
                PlayFabManager.Instance.Profile.SkipTutorial = value;
                PlayFabManager.Instance.SavePlayerProfile();
            }
        }

        public int Score { get; private set; }
        public int CurrentScore => (int) -_currentLevelStopwatch.ElapsedMilliseconds;
        public TutorialController TutorialController { get; set; }

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
            Time.timeScale = 0;
            _currentLevelStopwatch.Stop();
            Score = (int) _currentLevelStopwatch.ElapsedMilliseconds * -1;
            await PlayFabManager.Instance.UpdatePlayerStatistic("High Score", Score);
            Debug.Log("Wait for 2 seconds");
            await (new WaitForSecondsRealtime(2));
            Debug.Log("Publish GameWinMessage");
            MessageBus.Instance.Publish(new GameWinMessage());
        }

        public async Task StartLevel(string levelName)
        {
            if (!SkipTutorial)
            {
                await RunTutorial();
            }

            _currentLevelStopwatch.Restart();
        }

        private async Task RunTutorial()
        {
            Time.timeScale = 0;
            await Awaiters.NextFrame;
            await TutorialController.Run();
            Time.timeScale = 1;
        }
    }
}