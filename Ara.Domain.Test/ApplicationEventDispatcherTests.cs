using Ara.Domain.Common.Interfaces;
using Ara.Domain.Common.Services;
using Ara.Domain.Handlers.EventHandlers;
using Ara.Domain.Handlers.Events;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ara.Domain.Test
{
    public class ApplicationEventDispatcherTests
    {
        [Fact]
        public void Dispatch_ShouldCallHandle_WhenHandlerAdded()
        {
            ApplicationEventDispatcher applicationEventDispatcher = new ApplicationEventDispatcher();
            var testEvent = new PhotoSavedEvent();
            var testEventHandler = Substitute.For<IEventHandler<PhotoSavedEvent>>();

            applicationEventDispatcher.AddListener(testEventHandler);

            applicationEventDispatcher.Dispatch(testEvent);

            testEventHandler.Received(1).Handle(testEvent);
            Assert.True(true);
        }
    }
}
