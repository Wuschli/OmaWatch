using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.OmaWatch.GamePlay.Interactions
{
    public abstract class AbstractInputIconView : MonoBehaviour
    {
        public abstract void ShowAction(InputAction action);
    }
}