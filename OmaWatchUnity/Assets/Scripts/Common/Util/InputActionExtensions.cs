using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Common.Util
{
    public static class InputActionExtensions
    {
        public static IEnumerable<string> GetIconNames(this InputAction action)
        {
            if (action.activeControl != null)
                return new[] {action.activeControl.path};
            return action.controls.Select(c => c?.path);
        }

        public static string GetRichTextIconString(this InputAction action)
        {
            return string.Join("", action.GetIconNames().Select(s => $" <sprite name=\"{s}\">"));
        }
    }
}