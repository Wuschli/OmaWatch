using Assets.Scripts.OmaWatch.Ai;
using UnityEngine;

namespace Assets.Scripts.OmaWatch
{
    [RequireComponent(typeof(AudioSource))]
    public class FootstepBehaviour : MonoBehaviour
    {
        public AnimationCurve DistanceToVolumeCurve;
        public AudioClip[] FootStepClips;

        public void OnFootstep()
        {
            var index = Random.Range(0, FootStepClips.Length);
            var distanceToPlayer = (AICoordinator.Instance.Player.transform.position - transform.position).magnitude;
            var volume = DistanceToVolumeCurve.Evaluate(distanceToPlayer);
            GetComponent<AudioSource>().PlayOneShot(FootStepClips[index], volume);
        }
    }
}