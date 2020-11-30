using System;
using GameEventBus;
using GameEventBus.Events;
using GameEventBus.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Common.Util
{
    public class MessageBus : Singleton<MessageBus>
    {
        private readonly IEventBus _bus = new EventBus();

        public void Subscribe<TEventBase>(Action<TEventBase> action) where TEventBase : EventBase
        {
            try
            {
                _bus.Subscribe(action);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void Unsubscribe<TEventBase>(Action<TEventBase> token) where TEventBase : EventBase
        {
            try
            {
                _bus.Unsubscribe(token);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void Publish<TEventBase>(TEventBase eventItem) where TEventBase : EventBase
        {
            try
            {
                _bus.Publish(eventItem);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}