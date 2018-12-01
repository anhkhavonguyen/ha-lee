using System.Collections.Generic;

namespace Harvey.Domain
{
    public interface IEventStoreAggregate
    {
        long Version { get; }
        IEnumerable<IDomainEvent> GetUncommittedEvents();
        void ClearUncommittedEvents();
        void ApplyEvent(IDomainEvent @event, long version);
    }
}
