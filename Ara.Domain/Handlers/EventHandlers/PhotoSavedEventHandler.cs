using Ara.Domain.Common.Interfaces;
using Ara.Domain.Handlers.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ara.Domain.Handlers.EventHandlers
{
    public class PhotoSavedEventHandler : IEventHandler<PhotoSavedEvent>
    {
        public void Handle(PhotoSavedEvent eventData)
        {
            Console.WriteLine("PhotoSavedEventHandler called");
        }
    }
}
