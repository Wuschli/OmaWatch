using Assets.Scripts.OmaWatch.Player;
using Assets.Scripts.OmaWatch.Util;

namespace Assets.Scripts.OmaWatch.Ai
{
    public class AICoordinator : Singleton<AICoordinator>
    {
        public AbstractPlayerController Player { get; set; }
        public SuspiciousBehaviour SuspiciousBehaviour => Player.GetComponent<SuspiciousBehaviour>();
    }
}