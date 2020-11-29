using System.Linq;
using Assets.Scripts.Common.Util;
using Assets.Scripts.OmaWatch.Input;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class InputIconReplacer : MonoBehaviour
    {
        protected void OnEnable()
        {
            var tmpText = GetComponent<TMP_Text>();
            if (!tmpText.text.Contains("["))
                return;
            var inputActions = new DefaultInputActions();
            var inputActionMap = inputActions.Player.Get();
            foreach (var action in inputActionMap.actions)
            {
                tmpText.text = tmpText.text.Replace($"[{action.name}]", action.GetRichTextIconString());
            }
        }
    }
}