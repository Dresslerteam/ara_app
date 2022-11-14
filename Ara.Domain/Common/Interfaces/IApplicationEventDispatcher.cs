using System;
using System.Collections.Generic;
using System.Text;

namespace Ara.Domain.Common.Interfaces
{
    public interface IApplicationEventDispatcher
    {
        void AddListener<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent;
        void RemoveListener<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent;
        void Dispatch<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}
