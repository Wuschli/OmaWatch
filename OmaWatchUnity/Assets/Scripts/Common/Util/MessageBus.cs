using System;
using GameEventBus;
using GameEventBus.Events;
using GameEventBus.Interfaces;

namespace Assets.Scripts.Common.Util
{
    public class MessageBus : Singleton<MessageBus>, IEventBus
    {
        private readonly IEventBus _bus = new EventBus();

        public void Subscribe<TEventBase>(Action<TEventBase> action) where TEventBase : EventBase
        {
            _bus.Subscribe(action);
        }

        public void Unsubscribe<TEventBase>(Action<TEventBase> token) where TEventBase : EventBase
        {
            _bus.Unsubscribe(token);
        }

        public void Publish<TEventBase>(TEventBase eventItem) where TEventBase : EventBase
        {
            _bus.Publish(eventItem);
        }

        public void PublishAsync<TEventBase>(TEventBase eventItem) where TEventBase : EventBase
        {
            _bus.PublishAsync(eventItem);
        }

        public void PublishAsync<TEventBase>(TEventBase eventItem, AsyncCallback callback) where TEventBase : EventBase
        {
            _bus.PublishAsync(eventItem, callback);
        }
    }
}