using Assets.Scripts.OmaWatch.Input;
using Assets.Scripts.OmaWatch.Util;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace Assets.Scripts.OmaWatch
{
    public class PlayerController : MonoBehaviour, DefaultInputActions.IPlayerActions
    {
        public AnimationCurve Speed;
        public Vector2 StartRotation = Vector2.down;
        public ScrapTrail ScrapTrail;
        public Transform lookTarget;

        public float CurrentSpeed;

        private DefaultInputActions _defaultInput;
        private Animator _animator;
        private Vector2 _lastDirection;

        private void Awake()
        {
            _defaultInput = new DefaultInputActions();
            _defaultInput.Player.SetCallbacks(this);

            var nav = GetComponent<NavMeshAgent>();
            if (nav != null)
            {
                nav.updateRotation = false;
                nav.updateUpAxis = false;
            }
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
            if (Time.timeScale == 0)
                return;

            var inputDirection = _defaultInput.Player.Move.ReadValue<Vector2>();
            if (inputDirection.sqrMagnitude > 1)
                inputDirection.Normalize();
            var speed = Speed.Evaluate(ScrapTrail.ScrapCount);
            CurrentSpeed = speed;
            var translation = inputDirection * speed * Time.deltaTime;
            translation *= new Vector2(1f, 0.5f);
            transform.Translate(translation.ToVector3());
            if (inputDirection.sqrMagnitude > .01f)
                _lastDirection = inputDirection.normalized;
            _animator.SetFloat("AbsoluteSpeed", inputDirection.magnitude);
            _animator.SetFloat("Horizontal", _lastDirection.x);
            _animator.SetFloat("Vertical", _lastDirection.y);

            var look = _defaultInput.Player.Look.ReadValue<Vector2>();
            if (look.sqrMagnitude > 1)
                look.Normalize();

            lookTarget.localPosition = new Vector3(look.x * 2.9f, look.y * 1.9f);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
        }

        public void OnDropAll(InputAction.CallbackContext context)
        {
            if (context.performed)
                ScrapTrail.DropAll();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
        }
    }
}