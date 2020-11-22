using Assets.Scripts.Common.Util;
using Assets.Scripts.Messages;
using Assets.Scripts.OmaWatch.GamePlay.Interactions;

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
            MessageBus.Instance.Publish(new GameWinMessage());
            return true;
        }
    }
}