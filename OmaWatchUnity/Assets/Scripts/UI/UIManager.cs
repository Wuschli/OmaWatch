using System.Threading.Tasks;
using Assets.Scripts.OmaWatch.Input;
using Assets.Scripts.Common.Util;
using Assets.Scripts.Messages;
using Assets.Scripts.OmaWatch.Util;
using Stateless;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI
{
    public enum UIState
    {
        MainMenu,
        InGame,
        Pause,
        Lose,
        Win
    }

    public enum UITrigger
    {
        StartGame,
        Pause,
        Resume,
        Lose,
        Win,
        ToMainMenu
    }

    public class UIManager : Singleton<UIManager>, DefaultInputActions.IUIActions
    {
        public UIState DefaultState = UIState.MainMenu;
        private StateMachine<UIState, UITrigger> _stateMachine;
        private DefaultInputActions _defaultInput;

        public async Task Fire(UITrigger trigger)
        {
            await _stateMachine.FireAsync(trigger);
        }

        protected override void Awake()
        {
            base.Awake();
            _stateMachine = new StateMachine<UIState, UITrigger>(DefaultState);
            ConfigureStateMachine();
            _defaultInput = new DefaultInputActions();
            _defaultInput.UI.SetCallbacks(this);
        }

        protected virtual void OnEnable()
        {
            _defaultInput.UI.Enable();
            MessageBus.Instance.Subscribe<GameWinMessage>(OnGameWin);
        }

        private void OnGameWin(GameWinMessage msg)
        {
            Fire(UITrigger.Win).FireAndForget();
        }

        protected virtual void OnDisable()
        {
            _defaultInput?.UI.Disable();
        }

        private void ConfigureStateMachine()
        {
            _stateMachine.Configure(UIState.MainMenu)
                .OnEntryAsync(OnEntryMainMenuAsync)
                .OnExitAsync(OnExitMainMenuAsync)
                .Permit(UITrigger.StartGame, UIState.InGame);

            _stateMachine.Configure(UIState.InGame)
                .OnEntryAsync(OnEntryInGameAsync)
                .OnExitAsync(OnExitInGameAsync)
                .Permit(UITrigger.Pause, UIState.Pause)
                .Permit(UITrigger.Lose, UIState.Lose)
                .Permit(UITrigger.Win, UIState.Win);

            _stateMachine.Configure(UIState.Pause)
                .SubstateOf(UIState.InGame)
                .OnEntryAsync(OnEntryPauseAsync)
                .OnExitAsync(OnExitPauseAsync)
                .Permit(UITrigger.Resume, UIState.InGame)
                .Permit(UITrigger.ToMainMenu, UIState.MainMenu);

            _stateMachine.Configure(UIState.Lose)
                .OnEntryAsync(OnEntryLoseAsync)
                .OnExitAsync(OnExitLoseAsync)
                .Permit(UITrigger.ToMainMenu, UIState.MainMenu);

            _stateMachine.Configure(UIState.Win)
                .OnEntryAsync(OnEntryWinAsync)
                .OnExitAsync(OnExitWinAsync)
                .Permit(UITrigger.ToMainMenu, UIState.MainMenu);
        }

        private async Task LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            //TODO show loading screen
            await Awaiters.NextFrame; // switch to main thread
            Debug.Log($"Loading {sceneName} {mode}");
            var operation = SceneManager.LoadSceneAsync(sceneName, mode);
            await operation;
            Debug.Log($"Done loading {sceneName} {mode}");
        }

        private async Task UnloadScene(string sceneName)
        {
            await Awaiters.NextFrame; // switch to main thread
            Debug.Log($"Unloading {sceneName}");
            var operation = SceneManager.UnloadSceneAsync(sceneName);
            await operation;
            Debug.Log($"Done unloading {sceneName}");
        }

        private async Task OnEntryMainMenuAsync(StateMachine<UIState, UITrigger>.Transition transition)
        {
            await LoadScene("MainMenu");
        }

        private Task OnExitMainMenuAsync(StateMachine<UIState, UITrigger>.Transition transition)
        {
            return Task.CompletedTask;
        }

        private async Task OnEntryInGameAsync(StateMachine<UIState, UITrigger>.Transition transition)
        {
            await LoadScene("level1");
        }

        private Task OnExitInGameAsync(StateMachine<UIState, UITrigger>.Transition transition)
        {
            return Task.CompletedTask;
        }

        private async Task OnEntryPauseAsync(StateMachine<UIState, UITrigger>.Transition transition)
        {
            await Awaiters.NextFrame; // switch to main thread
            Time.timeScale = 0;
            await LoadScene("Pause", LoadSceneMode.Additive);
        }

        private async Task OnExitPauseAsync(StateMachine<UIState, UITrigger>.Transition transition)
        {
            await UnloadScene("Pause").ConfigureAwait(true);
            Time.timeScale = 1;
        }

        private async Task OnEntryLoseAsync(StateMachine<UIState, UITrigger>.Transition transition)
        {
            await LoadScene("Lose");
        }

        private Task OnExitLoseAsync(StateMachine<UIState, UITrigger>.Transition transition)
        {
            return Task.CompletedTask;
        }

        private async Task OnEntryWinAsync(StateMachine<UIState, UITrigger>.Transition transition)
        {
            await LoadScene("Win");
        }

        private Task OnExitWinAsync(StateMachine<UIState, UITrigger>.Transition transition)
        {
            return Task.CompletedTask;
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
        }

        public void OnLeftClick(InputAction.CallbackContext context)
        {
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
        }

        public void OnMove(InputAction.CallbackContext context)
        {
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
        }

        public void OnTrackedPosition(InputAction.CallbackContext context)
        {
        }

        public void OnTrackedOrientation(InputAction.CallbackContext context)
        {
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;
            if (_stateMachine.CanFire(UITrigger.Resume))
                Fire(UITrigger.Resume).FireAndForget();
            else if (_stateMachine.CanFire(UITrigger.Pause))
                Fire(UITrigger.Pause).FireAndForget();
        }
    }
}