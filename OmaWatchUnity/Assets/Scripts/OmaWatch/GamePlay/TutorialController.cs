namespace Assets.Scripts.OmaWatch.GamePlay
{
    public class TutorialController : AbstractTimelineController
    {
        protected void OnEnable()
        {
            LevelCoordinator.Instance.TutorialController = this;
        }

        protected void OnDisable()
        {
            if (LevelCoordinator.Instance.TutorialController == this)
                LevelCoordinator.Instance.TutorialController = null;
        }
    }
}