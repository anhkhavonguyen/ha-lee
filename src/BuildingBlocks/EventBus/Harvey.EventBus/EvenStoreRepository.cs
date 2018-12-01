using System;
using System.Reflection;
using System.Threading.Tasks;
using Harvey.Domain;
using Harvey.EventBus.Abstractions;

namespace Harvey.EventBus
{
    public class EvenStoreRepository<TAggregateRoot> : IEventStoreRepository<TAggregateRoot>
        where TAggregateRoot : AggregateRootBase, IAggregateRoot
    {
        private readonly IEventStore _eventStore;
        private readonly IEventBus _eventBus;
        public EvenStoreRepository(IEventStore eventStore, IEventBus eventBus)
        {
            _eventStore = eventStore;
            _eventBus = eventBus;
        }

        public async Task<TAggregateRoot> GetByIdAsync(Guid id)
        {
            var aggregate = CreateEmptyAggregate();
            var events = await _eventStore.ReadEventsAsync(id.ToString());
            foreach (var item in events)
            {
                aggregate.ApplyEvent(item, item.Version);
            }
            return aggregate;
        }

        public async Task SaveAsync(TAggregateRoot aggregate)
        {
            foreach (var @event in aggregate.GetUncommittedEvents())
            {
                await _eventStore.AppendEventAsync((dynamic)@event);
                await _eventBus.PublishAsync((dynamic)@event);
            }
            ((IEventStoreAggregate)aggregate).ClearUncommittedEvents();
        }

        private TAggregateRoot CreateEmptyAggregate()
        {
            return (TAggregateRoot)typeof(TAggregateRoot)
                    .GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                        null, new Type[0], new ParameterModifier[0])
                    .Invoke(new object[0]);
        }
    }
}
