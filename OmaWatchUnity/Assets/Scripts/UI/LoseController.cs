using UnityEngine;

namespace Assets.Scripts.UI
{
    public class LoseController : MonoBehaviour
    {
        public void BackToMenu()
        {
            UIManager.Instance.Fire(UITrigger.ToMainMenu);
        }
    }
}