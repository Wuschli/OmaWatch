using Assets.Scripts.OmaWatch.Ai;
using Assets.Scripts.OmaWatch.GamePlay;
using Assets.Scripts.OmaWatch.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.OmaWatch
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class AbstractPlayerController : MonoBehaviour, DefaultInputActions.IPlayerActions
    {
        public ScrapTrail ScrapTrail;
        public Transform LookTarget;
        public AnimationCurve SpeedCurve;
        public Animator Animator;
        public Vector2 StartRotation = Vector2.down;
        public AudioClip[] FootStepClips;

        private DefaultInputActions _defaultInput;
        private Vector2 _moveInput;
        private Vector2 _lookInput;
        private Vector2 _lastDirection;

        protected Vector2 MoveInput => _moveInput;
        protected Vector2 LookInput => _lookInput;
        protected float Speed => SpeedCurve.Evaluate(ScrapTrail.ScrapCount);

        public void OnMove(InputAction.CallbackContext context)
        {
            _moveInput = context.action.ReadValue<Vector2>();
            if (_moveInput.sqrMagnitude > 1)
                _moveInput.Normalize();
        }

        public void OnDropAll(InputAction.CallbackContext context)
        {
            if (context.performed)
                ScrapTrail.DropAll();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
                Interact();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            _lookInput = context.action.ReadValue<Vector2>();
            if (_lookInput.sqrMagnitude > 1)
                _lookInput.Normalize();
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;
            LevelCoordinator.Instance.TogglePause();
        }

        public void OnFootstep()
        {
            var index = Random.Range(0, FootStepClips.Length);
            GetComponent<AudioSource>().PlayOneShot(FootStepClips[index]);
        }

        protected virtual void Awake()
        {
            _defaultInput = new DefaultInputActions();
            _defaultInput.Player.SetCallbacks(this);
        }

        protected virtual void OnEnable()
        {
            _defaultInput.Player.Enable();
            if (Animator == null)
                Animator = GetComponent<Animator>();
            _lastDirection = StartRotation;
            AICoordinator.Instance.Player = this;
        }

        protected virtual void OnDisable()
        {
            _defaultInput.Player.Disable();
        }

        protected virtual void Update()
        {
            if (Time.timeScale == 0)
            {
                LookTarget.localPosition = Vector3.zero;
                return;
            }

            LookTarget.localPosition = new Vector3(LookInput.x * 2.9f, LookInput.y * 1.9f);

            if (MoveInput.sqrMagnitude > .01f)
                _lastDirection = MoveInput.normalized;

            Animator.SetFloat("AbsoluteSpeed", MoveInput.magnitude);
            Animator.SetFloat("Horizontal", _lastDirection.x);
            Animator.SetFloat("Vertical", _lastDirection.y);
        }

        protected virtual void FixedUpdate()
        {
        }

        protected virtual void Interact()
        {
            throw new System.NotImplementedException();
        }
    }
}