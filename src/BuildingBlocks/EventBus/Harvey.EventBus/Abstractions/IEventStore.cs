using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Harvey.EventBus.Abstractions
{
    public interface IEventStore
    {
        bool Existed<TEvent>(Guid eventId)
            where TEvent : EventBase;
        Task<IEnumerable<EventBase>> ReadEventsAsync(string aggregateId);
        Task<bool> AppendEventAsync(EventBase @event);
    }
}
