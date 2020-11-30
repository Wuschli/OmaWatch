using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assets.Scripts.Common.Util;
using Assets.Scripts.OmaWatch.Ai;
using Assets.Scripts.OmaWatch.GamePlay;
using Assets.Scripts.OmaWatch.GamePlay.Interactions;
using Assets.Scripts.OmaWatch.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.OmaWatch
{
    public abstract class AbstractPlayerController : MonoBehaviour, DefaultInputActions.IPlayerActions
    {
        public ScrapTrail ScrapTrail;
        public Transform LookTarget;
        public AnimationCurve SpeedCurve;
        public Animator Animator;
        public Vector2 StartRotation = Vector2.down;

        private DefaultInputActions _defaultInput;
        private Vector2 _moveInput;
        private Vector2 _lookInput;
        private Vector2 _lastDirection;
        private List<AbstractButtonInteraction> _possibleInteractions = new List<AbstractButtonInteraction>();

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
                Interact().FireAndForget();
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

        public virtual void AddPossibleInteraction(AbstractButtonInteraction possibleInteraction)
        {
            _possibleInteractions.Add(possibleInteraction);
            UpdatePossibleInteractions();
        }

        public virtual void RemovePossibleInteraction(AbstractButtonInteraction possibleInteraction)
        {
            _possibleInteractions.Remove(possibleInteraction);
            possibleInteraction.InputIconView.ShowAction(null);
            UpdatePossibleInteractions();
        }

        public virtual void SetGrabbed(bool grabbed)
        {
            if (grabbed)
            {
                _defaultInput.Player.Disable();
                _moveInput = Vector2.zero;
                _lookInput = Vector2.zero;
            }
            else
            {
                _defaultInput.Player.Enable();
            }
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

            UpdatePossibleInteractions();
        }

        protected virtual void FixedUpdate()
        {
        }

        protected virtual async Task Interact()
        {
            var nextInteraction = _possibleInteractions.FirstOrDefault();
            if (nextInteraction == null)
                return;
            var couldInteract = await nextInteraction.InteractAsync(this);
            if (couldInteract)
                RemovePossibleInteraction(nextInteraction);
        }

        protected virtual void UpdatePossibleInteractions()
        {
            _possibleInteractions = _possibleInteractions
                .Distinct()
                .OrderBy(i => (transform.position - i.transform.position).magnitude)
                .ToList();
            if (!_possibleInteractions.Any())
                return;
            for (var i = 0; i < _possibleInteractions.Count; i++)
            {
                if (i == 0)
                    _possibleInteractions[i].InputIconView.ShowAction(_defaultInput.Player.Interact);
                else
                    _possibleInteractions[i].InputIconView.ShowAction(null);
            }
        }
    }
}