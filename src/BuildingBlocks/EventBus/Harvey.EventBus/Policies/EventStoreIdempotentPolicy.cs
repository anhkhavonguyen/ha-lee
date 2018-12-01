using Harvey.Domain;
using Harvey.EventBus.Abstractions;
using Harvey.Polly;

namespace Harvey.EventBus.Policies
{
    public class EventStoreIdempotentPolicy<TEvent> : IIdempotentPolicy<TEvent>
        where TEvent : EventBase
    {
        private readonly IEventStore _eventStore;
        public EventStoreIdempotentPolicy(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public void Handle(TEvent entity)
        {
        }

        public bool ShouldHandle(TEvent entity)
        {
            return _eventStore.Existed<TEvent>(entity.Id);
        }
    }
}
