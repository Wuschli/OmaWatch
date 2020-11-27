using Assets.Scripts.OmaWatch.Util;
using UnityEngine;

namespace Assets.Scripts.OmaWatch.GamePlay
{
    public class LevelStarter : MonoBehaviour
    {
        public string LevelName;

        protected void OnEnable()
        {
            if (!PlayFabManager.Instance.IsLoggedIn)
                PlayFabManager.Instance.Login().FireAndForget();
            if (!string.IsNullOrEmpty(LevelName))
                LevelCoordinator.Instance.StartLevel(LevelName);
        }
    }
}