using Assets.Scripts.OmaWatch.GamePlay;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(Slider))]
    public class VolumeSlider : MonoBehaviour
    {
        public AudioMixer Mixer;
        public string ParameterName;

        private Slider _slider;

        protected void OnEnable()
        {
            _slider = GetComponent<Slider>();
            if (Mixer.GetFloat(ParameterName, out var value))
                _slider.value = value;
            _slider.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(float value)
        {
            Mixer.SetFloat(ParameterName, value);
            switch (ParameterName)
            {
                case "MusicVolume":
                    LevelCoordinator.Instance.MusicVolume = value;
                    break;
                case "SFXVolume":
                    LevelCoordinator.Instance.SFXVolume = value;
                    break;
                default:
                    Debug.LogWarning($"{ParameterName} is not saved to profile");
                    break;
            }
        }
    }
}