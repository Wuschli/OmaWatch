using Assets.Scripts.OmaWatch.GamePlay;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class HUDController : MonoBehaviour
    {
        public TMP_Text YourTimeText;

        protected void LateUpdate()
        {
            YourTimeText.text = $"<mspace=14.0>{LevelCoordinator.Instance.CurrentScore.ToScoreDisplay()}</mspace>";
        }
    }
}