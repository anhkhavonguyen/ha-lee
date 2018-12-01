using Harvey.EventBus.Abstractions;
using Harvey.EventBus.Policies;
using Harvey.Exception.Extensions;
using Harvey.Polly;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.EventBus
{
    public abstract class EventHandlerBase<TEvent> where TEvent : EventBase
    {
        private readonly EventStoreIdempotentPolicy<TEvent> _eventStoreIdempotentPolicy;
        protected readonly ILogger<EventHandlerBase<TEvent>> Logger;
        protected virtual List<IIdempotentPolicy<TEvent>> AdditionalIdempotentPolicies => new List<IIdempotentPolicy<TEvent>>();
        //TODO don't expose eventStore
        public EventHandlerBase(IEventStore eventStore, ILogger<EventHandlerBase<TEvent>> logger)
        {
            _eventStoreIdempotentPolicy = new EventStoreIdempotentPolicy<TEvent>(eventStore);
            Logger = logger;
        }
        protected abstract Task ExecuteAsync(TEvent @event);

        public virtual async Task Handle(TEvent @event)
        {
            var eventIdempotentDetected = true;
            var additonalIdempotentDetected = true;

            ((IIdempotentPolicy<TEvent>)_eventStoreIdempotentPolicy).ExecuteStategyAsync(@event, Logger, () =>
            {
                eventIdempotentDetected = false;
            });

            if (AdditionalIdempotentPolicies.Any())
            {
                AdditionalIdempotentPolicies.ForEach(x => x.ExecuteStategyAsync(@event, Logger, () =>
                {
                    additonalIdempotentDetected = false;
                }));
            }
            else
            {
                additonalIdempotentDetected = false;
            }

            if (!eventIdempotentDetected && !additonalIdempotentDetected)
            {
                var htype = typeof(EventHandlerBase<>).MakeGenericType(typeof(TEvent));
                Logger.LogTrace($"[EventBus] [Handler {htype.Name}] {JsonConvert.SerializeObject(@event)}");
                try
                {
                    await ExecuteAsync(@event);
                    return;
                }
                catch (System.Exception ex)
                {
                    Logger.LogInformation(ex.GetTraceLog());
                    throw;
                }
            }
            return;
        }
    }
}
