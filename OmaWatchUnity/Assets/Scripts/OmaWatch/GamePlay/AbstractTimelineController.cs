using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

namespace Assets.Scripts.OmaWatch.GamePlay
{
    [RequireComponent(typeof(PlayableDirector))]
    public abstract class AbstractTimelineController : MonoBehaviour
    {
        public virtual async Task Run()
        {
            var director = GetComponent<PlayableDirector>();
            var directorCompletionSource = new TaskCompletionSource<bool>();

            void OnDirectorDone(PlayableDirector playableDirector)
            {
                directorCompletionSource.SetResult(true);
            }

            director.stopped += OnDirectorDone;
            director.Play();
            await directorCompletionSource.Task;
            director.stopped -= OnDirectorDone;
        }
    }
}