using System.Threading.Tasks;
using Assets.Scripts.Common.Util;
using Assets.Scripts.Messages;
using Assets.Scripts.OmaWatch.GamePlay.Interactions;
using Assets.Scripts.OmaWatch.Util;

namespace Assets.Scripts.OmaWatch.Ai
{
    public class LevelCoordinator : Singleton<LevelCoordinator>
    {
        public RocketConstructionSite ConstructionSite { get; set; }

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
            await Task.Delay(5000);
            MessageBus.Instance.Publish(new GameWinMessage());
        }
    }
}