using Assets.Scripts.OmaWatch;
using Assets.Scripts.OmaWatch.Util;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class MainMenuController : MonoBehaviour
    {
        protected void OnEnable()
        {
            PlayFabManager.Instance.Login().FireAndForget();
        }

        public void StartGame()
        {
            UIManager.Instance.Fire(UITrigger.StartGame).FireAndForget();
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }
    }
}