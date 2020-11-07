using System.Threading.Tasks;
using Assets.Scripts.OmaWatch.Input;
using Stateless;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

#pragma warning disable 1998

namespace Assets.Scripts.UI
{
    public enum UIState
    {
        MainMenu,
        InGame,
        Pause,
        GameOver
    }

    public enum UITrigger
    {
        StartGame,
        Pause,
        Resume,
        GameOver,
        ToMainMenu
    }

    public class UIManager : Singleton<UIManager>, DefaultInputActions.IUIActions
    {
        private readonly StateMachine<UIState, UITrigger> _stateMachine = new StateMachine<UIState, UITrigger>(UIState.MainMenu);
        private DefaultInputActions _defaultInput;

        public async Task Fire(UITrigger trigger)
        {
            await _stateMachine.FireAsync(trigger);
        }

        protected override void Awake()
        {
            base.Awake();
            ConfigureStateMachine();
            _defaultInput = new DefaultInputActions();
            _defaultInput.UI.SetCallbacks(this);
        }

        protected virtual void OnEnable()
        {
            _defaultInput.UI.Enable();
        }

        protected virtual void OnDisable()
        {
            _defaultInput.UI.Disable();
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
                .Permit(UITrigger.GameOver, UIState.GameOver)
                .Permit(UITrigger.ToMainMenu, UIState.MainMenu);

            _stateMachine.Configure(UIState.Pause)
                //.SubstateOf(UIState.InGame)
                .OnEntryAsync(OnEntryPauseAsync)
                .OnExitAsync(OnExitPauseAsync)
                .Permit(UITrigger.Resume, UIState.InGame);

            _stateMachine.Configure(UIState.GameOver)
                .OnEntryAsync(OnEntryGameOverAsync)
                .OnExitAsync(OnExitGameOverAsync)
                .Permit(UITrigger.ToMainMenu, UIState.MainMenu);
        }

        private async Task LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            var operation = SceneManager.LoadSceneAsync(sceneName, mode);
            await operation;
        }

        private async Task UnloadScene(string sceneName)
        {
            var operation = SceneManager.UnloadSceneAsync(sceneName);
            await operation;
        }

        private async Task OnEntryMainMenuAsync(StateMachine<UIState, UITrigger>.Transition transition)
        {
            await LoadScene("MainMenu");
        }

        private async Task OnExitMainMenuAsync(StateMachine<UIState, UITrigger>.Transition transition)
        {
            //await UnloadScene("MainMenu");
        }

        private async Task OnEntryInGameAsync(StateMachine<UIState, UITrigger>.Transition transition)
        {
            await LoadScene("Test");
        }

        private async Task OnExitInGameAsync(StateMachine<UIState, UITrigger>.Transition transition)
        {
            //await UnloadScene("Test");
        }

        private async Task OnEntryPauseAsync(StateMachine<UIState, UITrigger>.Transition transition)
        {
            Time.timeScale = 0;
            await LoadScene("Pause", LoadSceneMode.Additive);
        }

        private async Task OnExitPauseAsync(StateMachine<UIState, UITrigger>.Transition transition)
        {
            await UnloadScene("Pause");
            Time.timeScale = 1;
        }

        private async Task OnEntryGameOverAsync(StateMachine<UIState, UITrigger>.Transition transition)
        {
            await LoadScene("GameOver");
        }

        private async Task OnExitGameOverAsync(StateMachine<UIState, UITrigger>.Transition transition)
        {
            //await UnloadScene("GameOver");
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
            if (_stateMachine.CanFire(UITrigger.Pause))
                _ = Fire(UITrigger.Pause);
            else if (_stateMachine.CanFire(UITrigger.Resume))
                _ = Fire(UITrigger.Resume);
        }
    }
}