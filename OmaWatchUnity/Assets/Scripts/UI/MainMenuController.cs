using System.Threading.Tasks;
using Assets.Scripts.Common.Util;
using Assets.Scripts.OmaWatch;
using Assets.Scripts.OmaWatch.GamePlay;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class MainMenuController : MonoBehaviour
    {
        public Button QuitGameButton;
        public Toggle SkipTutorialToggle;
        public Slider MusicVolumeSlider;
        public Slider SFXVolumeSlider;

        protected void OnEnable()
        {
            StartUp().FireAndForget();
#if !(UNITY_EDITOR || UNITY_STANDALONE)
            QuitGameButton.gameObject.SetActive(false);
#endif
        }

        public void StartGame()
        {
            UIManager.Instance.Fire(UITrigger.StartGame);
        }

        public void ToggleSkipTutorial()
        {
            LevelCoordinator.Instance.SkipTutorial = SkipTutorialToggle.isOn;
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }

        private async Task StartUp()
        {
            SkipTutorialToggle.interactable = false;
            MusicVolumeSlider.interactable = false;
            SFXVolumeSlider.interactable = false;

            if (!PlayFabManager.Instance.IsLoggedIn)
                await PlayFabManager.Instance.Login();
            SkipTutorialToggle.isOn = LevelCoordinator.Instance.SkipTutorial;
            MusicVolumeSlider.value = LevelCoordinator.Instance.MusicVolume;
            SFXVolumeSlider.value = LevelCoordinator.Instance.SFXVolume;

            SkipTutorialToggle.interactable = true;
            MusicVolumeSlider.interactable = true;
            SFXVolumeSlider.interactable = true;
        }
    }
}