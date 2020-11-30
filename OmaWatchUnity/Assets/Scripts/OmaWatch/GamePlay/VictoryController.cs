namespace Assets.Scripts.OmaWatch.GamePlay
{
    public class VictoryController : AbstractTimelineController
    {
        protected void OnEnable()
        {
            LevelCoordinator.Instance.VictoryController = this;
        }

        protected void OnDisable()
        {
            if (LevelCoordinator.Instance.VictoryController == this)
                LevelCoordinator.Instance.VictoryController = null;
        }
    }
}