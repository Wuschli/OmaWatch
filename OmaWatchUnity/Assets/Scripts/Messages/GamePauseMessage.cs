using GameEventBus.Events;

namespace Assets.Scripts.Messages
{
    public class GamePauseMessage : EventBase
    {
        public bool IsPaused { get; }

        public GamePauseMessage(bool isPaused)
        {
            IsPaused = isPaused;
        }
    }
}