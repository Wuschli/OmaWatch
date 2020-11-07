using Assets.Scripts.OmaWatch.Input;
using Assets.Scripts.OmaWatch.Util;
using UnityEngine;

namespace Assets.Scripts.OmaWatch
{
    public class PlayerController : MonoBehaviour
    {
        public float Speed = 3f;
        public Vector2 StartRotation = Vector2.down;
        public ScrapTrail ScrapTrail;

        private DefaultInputActions _defaultInput;
        private Animator _animator;
        private Vector2 _lastDirection;

        private void Awake()
        {
            _defaultInput = new DefaultInputActions();
        }

        private void OnEnable()
        {
            _defaultInput.Player.Enable();
            _animator = GetComponent<Animator>();
            _lastDirection = StartRotation;
        }

        private void OnDisable()
        {
            _defaultInput.Player.Disable();
        }

        private void Update()
        {
            var inputDirection = _defaultInput.Player.Move.ReadValue<Vector2>();
            if (inputDirection.sqrMagnitude > 1)
                inputDirection.Normalize();
            var translation = inputDirection * Speed * Time.deltaTime;
            transform.Translate(translation.ToVector3());
            if (inputDirection.sqrMagnitude > .01f)
                _lastDirection = inputDirection.normalized;
            _animator.SetFloat("AbsoluteSpeed", inputDirection.magnitude);
            _animator.SetFloat("Horizontal", _lastDirection.x);
            _animator.SetFloat("Vertical", _lastDirection.y);
        }
    }
}