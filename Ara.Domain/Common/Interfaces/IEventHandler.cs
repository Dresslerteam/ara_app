using System;
using System.Collections.Generic;
using System.Text;

namespace Ara.Domain.Common.Interfaces
{
    public interface IEvent { }
    public interface IEventHandler { }
    public interface IEventHandler<in T> : IEventHandler where T : IEvent
    {
        void Handle(T eventData);
    }
}
