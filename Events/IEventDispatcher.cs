﻿using System.Threading.Tasks;

namespace Events
{
    public interface IEventDispatcher
    {
        Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}