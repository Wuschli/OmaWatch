using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.OmaWatch
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicPlayer : MonoBehaviour
    {
        public AudioClip[] Tracks;

        private int _currentTrack = -1;
        private AudioSource _audioSource;

        protected void OnEnable()
        {
            _audioSource = GetComponent<AudioSource>();
            PlayNextTrack(0);
        }

        protected void Update()
        {
            if (_audioSource.isPlaying)
                return;
            PlayNextTrack(Random.Range(5f, 20f));
        }

        private void PlayNextTrack(float delay)
        {
            _currentTrack++;
            _currentTrack %= Tracks.Length;
            _audioSource.clip = Tracks[_currentTrack];
            _audioSource.loop = false;
            _audioSource.PlayDelayed(delay);
        }
    }
}