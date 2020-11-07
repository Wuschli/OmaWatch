using UnityEngine;

namespace Assets.Scripts.UI
{
    public class MainMenuController : MonoBehaviour
    {
        public void StartGame()
        {
            _ = UIManager.Instance.Fire(UITrigger.StartGame);
        }
    }
}