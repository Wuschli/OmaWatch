using Assets.Scripts.Common.Util;
using Assets.Scripts.OmaWatch.Player;

namespace Assets.Scripts.OmaWatch.Ai
{
    public class AICoordinator : Singleton<AICoordinator>
    {
        public AbstractPlayerController Player { get; set; }
        public SuspiciousBehaviour SuspiciousBehaviour => Player.GetComponent<SuspiciousBehaviour>();
    }
}