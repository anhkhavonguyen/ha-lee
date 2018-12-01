using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marten;
using Marten.Events;

namespace Harvey.EventBus.EventStore.Marten
{
    public class MartenEventStore : Abstractions.IEventStore
    {
        private readonly DocumentStore _store;
        public MartenEventStore(string connectionString)
        {
            _store = DocumentStore.For(cfg =>
            {
                cfg.Connection(connectionString);
                cfg.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;
                cfg.Events.StreamIdentity = StreamIdentity.AsString;
            });
        }
        public async Task<bool> AppendEventAsync(EventBase @event)
        {
            var result = false;
            using (var session = _store.LightweightSession())
            {
                session.Events.Append(@event.AggregateId, @event);
                await session.SaveChangesAsync();
                result = true;
            }
            return result;
        }

        public bool Existed<TEvent>(Guid eventId)
            where TEvent : EventBase
        {
            using (var session = _store.OpenSession())
            {
                return session.Query<TEvent>().ToList().Any(x => x.Id == eventId);
            }
        }

        public async Task<IEnumerable<EventBase>> ReadEventsAsync(string aggregateId)
        {
            await Task.Yield();
            var result = new List<EventBase>();
            using (var session = _store.OpenSession())
            {
                var events = await session.Events.FetchStreamAsync(aggregateId);
                foreach (var item in events)
                {
                    var @event = (EventBase)item.Data;
                    @event.LastTimeStamp = item.Timestamp.DateTime;
                    result.Add(@event);
                }
                return result.OrderBy(x => x.LastTimeStamp);
            }
        }
    }
}
