using System;
using Assets.Scripts.Common.Util;
using Assets.Scripts.Messages;
using Stateless;
using UnityEngine;
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

        public void Fire(UITrigger trigger)
        {
            Debug.Log($"Fire {trigger}");
            try
            {
                _stateMachine.Fire(trigger);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            Debug.Log($"Fire {trigger} done");
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
                .OnEntry(OnEntrySplash)
                .OnExit(OnExitSplash)
                .Permit(UITrigger.ToMainMenu, UIState.MainMenu);

            _stateMachine.Configure(UIState.MainMenu)
                .OnEntry(OnEntryMainMenu)
                .OnExit(OnExitMainMenu)
                .Permit(UITrigger.StartGame, UIState.InGame);

            _stateMachine.Configure(UIState.InGame)
                .OnEntry(OnEntryInGame)
                .OnExit(OnExitInGame)
                .Permit(UITrigger.Pause, UIState.Pause)
                .Permit(UITrigger.Lose, UIState.Lose)
                .Permit(UITrigger.Win, UIState.Win);

            _stateMachine.Configure(UIState.Pause)
                .SubstateOf(UIState.InGame)
                .OnEntry(OnEntryPause)
                .OnExit(OnExitPause)
                .Permit(UITrigger.Resume, UIState.InGame)
                .Permit(UITrigger.ToMainMenu, UIState.MainMenu);

            _stateMachine.Configure(UIState.Lose)
                .OnEntry(OnEntryLose)
                .OnExit(OnExitLose)
                .Permit(UITrigger.ToMainMenu, UIState.MainMenu);

            _stateMachine.Configure(UIState.Win)
                .OnEntry(OnEntryWin)
                .OnExit(OnExitWin)
                .Permit(UITrigger.ToMainMenu, UIState.MainMenu);
        }

        private void OnGameWin(GameWinMessage msg)
        {
            Fire(UITrigger.Win);
        }

        private void OnGamePaused(GamePauseMessage msg)
        {
            if (msg.IsPaused)
                Fire(UITrigger.Pause);
            else
                Fire(UITrigger.Resume);
        }

        private void OnEntrySplash(StateMachine<UIState, UITrigger>.Transition transition)
        {
            SceneHelper.LoadScene("Splash").FireAndForget();
        }

        private void OnExitSplash(StateMachine<UIState, UITrigger>.Transition transition)
        {
        }

        private void OnEntryMainMenu(StateMachine<UIState, UITrigger>.Transition transition)
        {
            SceneHelper.LoadScene("MainMenu").FireAndForget();
        }

        private void OnExitMainMenu(StateMachine<UIState, UITrigger>.Transition transition)
        {
        }

        private void OnEntryInGame(StateMachine<UIState, UITrigger>.Transition transition)
        {
            SceneHelper.LoadScene("level1").FireAndForget();
            SceneHelper.LoadScene("HUD", LoadSceneMode.Additive).FireAndForget();
        }

        private void OnExitInGame(StateMachine<UIState, UITrigger>.Transition transition)
        {
        }

        private void OnEntryPause(StateMachine<UIState, UITrigger>.Transition transition)
        {
            SceneHelper.LoadScene("Pause", LoadSceneMode.Additive).FireAndForget();
        }

        private void OnExitPause(StateMachine<UIState, UITrigger>.Transition transition)
        {
            SceneHelper.UnloadScene("Pause").FireAndForget();
        }

        private void OnEntryLose(StateMachine<UIState, UITrigger>.Transition transition)
        {
            SceneHelper.LoadScene("Lose").FireAndForget();
        }

        private void OnExitLose(StateMachine<UIState, UITrigger>.Transition transition)
        {
        }

        private void OnEntryWin(StateMachine<UIState, UITrigger>.Transition transition)
        {
            SceneHelper.LoadScene("Win").FireAndForget();
        }

        private void OnExitWin(StateMachine<UIState, UITrigger>.Transition transition)
        {
        }
    }
}