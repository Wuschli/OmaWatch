using System;
using Assets.Scripts.OmaWatch.Input;
using Assets.Scripts.OmaWatch.Util;
using UnityEngine;

namespace Assets.Scripts.OmaWatch
{
    public class PlayerController : MonoBehaviour
    {
        public float Speed = 3f;

        private DefaultInputActions _defaultInput;

        private void Awake()
        {
            _defaultInput = new DefaultInputActions();
        }

        private void OnEnable()
        {
            _defaultInput.Player.Enable();
        }

        private void OnDisable()
        {
            _defaultInput.Player.Disable();
        }

        private void Update()
        {
            var translation = _defaultInput.Player.Move.ReadValue<Vector2>().normalized * Speed * Time.deltaTime;
            transform.Translate(translation.ToVector3());
        }
    }
}