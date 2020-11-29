using Assets.Scripts.Common.Util;
using Assets.Scripts.OmaWatch.GamePlay.Interactions;
using TMPro;
using UnityEngine.InputSystem;

namespace Assets.Scripts.UI
{
    public class InputIconView : AbstractInputIconView
    {
        public TMP_Text Text;

        public override void ShowAction(InputAction action)
        {
            if (action == null)
                Text.text = string.Empty;
            else
                Text.text = action.GetRichTextIconString();
        }

        protected void OnEnable()
        {
            Text.text = string.Empty;
        }
    }
}