using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Common.Util
{
    public static class SceneHelper
    {
        public static async Task LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            //TODO show loading screen?
            await Awaiters.NextFrame; // switch to main thread
            Debug.Log($"Loading {sceneName} {mode}");
            var operation = SceneManager.LoadSceneAsync(sceneName, mode);
            operation.allowSceneActivation = true;
            await operation;
            Debug.Log($"Done loading {sceneName} {mode}");
        }

        public static async Task UnloadScene(string sceneName)
        {
            await Awaiters.NextFrame; // switch to main thread
            Debug.Log($"Unloading {sceneName}");
            var operation = SceneManager.UnloadSceneAsync(sceneName);
            await operation;
            Debug.Log($"Done unloading {sceneName}");
        }

        public static bool IsSceneLoaded(string sceneName)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.name == sceneName)
                    return true;
            }

            return false;
        }
    }
}