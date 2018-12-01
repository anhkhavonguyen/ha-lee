using System.Collections.Generic;
using System.Linq;

namespace Harvey.Domain
{
    public abstract class AggregateRootBase : EntityBase, IAggregateRoot, IEventStoreAggregate
    {
        private long _version = -1;
        private readonly ICollection<IDomainEvent> _uncommittedEvents = new LinkedList<IDomainEvent>();
        public long Version => _version;

        public void ApplyEvent(IDomainEvent @event, long version)
        {
            if (!_uncommittedEvents.Any(x => Equals(x.Id, @event.Id)))
            {
                ((dynamic)this).Apply((dynamic)@event);
                _version = version;
            }
        }

        public IEnumerable<IDomainEvent> GetUncommittedEvents()
        {
            return _uncommittedEvents.AsEnumerable();
        }

        protected void RaiseEvent<TEvent>(TEvent @event)
            where TEvent : IDomainEvent
        {
            @event.Version = _version;
            ApplyEvent(@event, _version + 1);
            _uncommittedEvents.Add(@event);
        }

        void IEventStoreAggregate.ClearUncommittedEvents()
        {
            _uncommittedEvents.Clear();
        }
    }
}
