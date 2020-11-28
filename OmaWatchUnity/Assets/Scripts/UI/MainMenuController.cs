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
        public Toggle SkipTutorialToggle;

        protected void OnEnable()
        {
            StartUp().FireAndForget();
            SkipTutorialToggle.isOn = LevelCoordinator.Instance.SkipTutorial;
        }

        public void StartGame()
        {
            UIManager.Instance.Fire(UITrigger.StartGame).FireAndForget();
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
            if (!PlayFabManager.Instance.IsLoggedIn)
                await PlayFabManager.Instance.Login();
        }
    }
}