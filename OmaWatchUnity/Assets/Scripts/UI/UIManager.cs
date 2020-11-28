using System.Threading.Tasks;
using Assets.Scripts.Common.Util;
using Assets.Scripts.Messages;
using Stateless;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI
{
    public enum UIState
    {
        Splash,
        MainMenu,
        InGame,
        Pause,
        Lose,
        Win
    }

    public enum UITrigger
    {
        SplashDone,
        StartGame,
        Pause,
        Resume,
        Lose,
        Win,
        ToMainMenu
    }

    public class UIManager : Singleton<UIManager>
    {
        public UIState DefaultState = UIState.Splash;
        private StateMachine<UIState, UITrigger> _stateMachine;

        public async Task Fire(UITrigger trigger)
        {
            await _stateMachine.FireAsync(trigger);
        }

        protected override void Awake()
        {
            base.Awake();
            _stateMachine = new StateMachine<UIState, UITrigger>(DefaultState);
            ConfigureStateMachine();
        }

        protected virtual void OnEnable()
        {
            if (Instance != this)
                return;
            MessageBus.Instance.Subscribe<GameWinMessage>(OnGameWin);
            MessageBus.Instance.Subscribe<GamePauseMessage>(OnGamePaused);
        }

        private void ConfigureStateMachine()
        {
            _stateMachine.Configure(UIState.Splash)
                .OnEntryAsync(OnEntrySplashAsync)
                .OnExitAsync(OnExitSplashAsync)
                .Permit(UITrigger.SplashDone, UIState.MainMenu);

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

        private void OnGameWin(GameWinMessage msg)
        {
            Fire(UITrigger.Win).FireAndForget();
        }

        private void OnGamePaused(GamePauseMessage msg)
        {
            if (msg.IsPaused)
                Fire(UITrigger.Pause).FireAndForget();
            else
                Fire(UITrigger.Resume).FireAndForget();
        }

        private async Task OnEntrySplashAsync(StateMachine<UIState, UITrigger>.Transition transition)
        {
            await SceneHelper.LoadScene("Splash");
        }

        private Task OnExitSplashAsync(StateMachine<UIState, UITrigger>.Transition transition)
        {
            return Task.CompletedTask;
        }

        private async Task OnEntryMainMenuAsync(StateMachine<UIState, UITrigger>.Transition transition)
        {
            await SceneHelper.LoadScene("MainMenu");
        }

        private Task OnExitMainMenuAsync(StateMachine<UIState, UITrigger>.Transition transition)
        {
            return Task.CompletedTask;
        }

        private async Task OnEntryInGameAsync(StateMachine<UIState, UITrigger>.Transition transition)
        {
            await SceneHelper.LoadScene("level1");
            await SceneHelper.LoadScene("HUD", LoadSceneMode.Additive);
        }

        private Task OnExitInGameAsync(StateMachine<UIState, UITrigger>.Transition transition)
        {
            return Task.CompletedTask;
        }

        private async Task OnEntryPauseAsync(StateMachine<UIState, UITrigger>.Transition transition)
        {
            await SceneHelper.LoadScene("Pause", LoadSceneMode.Additive);
        }

        private async Task OnExitPauseAsync(StateMachine<UIState, UITrigger>.Transition transition)
        {
            await SceneHelper.UnloadScene("Pause").ConfigureAwait(true);
        }

        private async Task OnEntryLoseAsync(StateMachine<UIState, UITrigger>.Transition transition)
        {
            await SceneHelper.LoadScene("Lose");
        }

        private Task OnExitLoseAsync(StateMachine<UIState, UITrigger>.Transition transition)
        {
            return Task.CompletedTask;
        }

        private async Task OnEntryWinAsync(StateMachine<UIState, UITrigger>.Transition transition)
        {
            await SceneHelper.LoadScene("Win");
        }

        private Task OnExitWinAsync(StateMachine<UIState, UITrigger>.Transition transition)
        {
            return Task.CompletedTask;
        }
    }
}