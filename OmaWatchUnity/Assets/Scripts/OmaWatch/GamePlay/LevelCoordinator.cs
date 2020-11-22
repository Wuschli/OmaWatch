using System.Diagnostics;
using System.Threading.Tasks;
using Assets.Scripts.Common.Util;
using Assets.Scripts.Messages;
using Assets.Scripts.OmaWatch.GamePlay.Interactions;
using Assets.Scripts.OmaWatch.Util;

namespace Assets.Scripts.OmaWatch.GamePlay
{
    public class LevelCoordinator : Singleton<LevelCoordinator>
    {
        public RocketConstructionSite ConstructionSite { get; set; }

        private readonly Stopwatch _currentLevelStopwatch = new Stopwatch();

        public bool CheckWinningCondition()
        {
            if (ConstructionSite == null)
                return false;
            if (!ConstructionSite.AllSlotsFilled)
                return false;
            StartWinningSequence().FireAndForget();
            return true;
        }

        private async Task StartWinningSequence()
        {
            await PlayFabManager.Instance.UpdatePlayerStatistic("High Score", (int) _currentLevelStopwatch.ElapsedMilliseconds);
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