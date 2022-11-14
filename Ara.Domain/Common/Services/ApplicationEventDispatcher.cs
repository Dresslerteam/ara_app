using Ara.Domain.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ara.Domain.Common.Services
{
    public class ApplicationEventDispatcher : IApplicationEventDispatcher
    {
        private Dictionary<Type, IEventHandler> _applicationEventHandlers;

        public ApplicationEventDispatcher()
        {
            _applicationEventHandlers = new Dictionary<Type, IEventHandler>();
        }

        public void AddListener<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent
        {
            _applicationEventHandlers.Add(typeof(TEvent), handler);
        }

        public void Dispatch<TEvent>(TEvent @event) where TEvent : IEvent
        {
            IEventHandler eventHandler = null;
            if (_applicationEventHandlers.TryGetValue(@event.GetType(), out eventHandler))
            {
                ((IEventHandler<TEvent>)eventHandler).Handle(@event);
            }
            else
            {
                throw new Exception($"EventHandler cant be found for the Event {@event.GetType().ToString()}");
            }
        }

        public void RemoveListener<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent
        {
            _applicationEventHandlers.Remove(typeof(TEvent));
        }
    }
}
