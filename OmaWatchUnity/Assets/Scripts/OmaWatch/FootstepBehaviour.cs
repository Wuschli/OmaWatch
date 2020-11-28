using UnityEngine;

namespace Assets.Scripts.OmaWatch
{
    [RequireComponent(typeof(AudioSource))]
    public class FootstepBehaviour : MonoBehaviour
    {
        public AudioClip[] FootStepClips;

        public void OnFootstep()
        {
            var index = Random.Range(0, FootStepClips.Length);
            GetComponent<AudioSource>().PlayOneShot(FootStepClips[index]);
        }
    }
}