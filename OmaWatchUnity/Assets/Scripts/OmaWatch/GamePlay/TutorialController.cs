using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

namespace Assets.Scripts.OmaWatch.GamePlay
{
    [RequireComponent(typeof(PlayableDirector))]
    public class TutorialController : MonoBehaviour
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

        public async Task Run()
        {
            Time.timeScale = 0;
            var director = GetComponent<PlayableDirector>();
            var directorCompletionSource = new TaskCompletionSource<bool>();

            void OnDirectorOnStopped(PlayableDirector playableDirector)
            {
                directorCompletionSource.SetResult(true);
            }

            director.stopped += OnDirectorOnStopped;
            director.Play();
            await directorCompletionSource.Task;
            Time.timeScale = 1;
            director.stopped -= OnDirectorOnStopped;
        }
    }
}