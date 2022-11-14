// See https://aka.ms/new-console-template for more information
using Ara.Domain.Common.Interfaces;
using Ara.Domain.Common.Services;
using Ara.Domain.Handlers.EventHandlers;
using Ara.Domain.Handlers.Events;

IApplicationEventDispatcher dispatcher = new ApplicationEventDispatcher();
dispatcher.AddListener<PhotoSavedEvent>(new PhotoSavedEventHandler());

dispatcher.Dispatch<PhotoSavedEvent>(new PhotoSavedEvent());

Console.WriteLine("Hello, World!");
