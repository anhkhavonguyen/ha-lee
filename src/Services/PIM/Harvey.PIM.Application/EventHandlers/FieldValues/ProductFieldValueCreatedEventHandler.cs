using Harvey.EventBus;
using Harvey.EventBus.Abstractions;
using Harvey.EventBus.Events.FileldValues;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.FieldFramework;
using Harvey.PIM.Application.FieldFramework.Entities;
using Harvey.PIM.Application.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.EventHandlers.FieldValues
{
    public class ProductFieldValueCreatedEventHandler : EventHandlerBase<FieldValueCreatedEvent>
    {
        private readonly IEfRepository<TransientPimDbContext, FieldValue> _efRepository;
        private readonly IEventStore _eventStore;
        public ProductFieldValueCreatedEventHandler(
            IEventStore eventStore,
            ILogger<EventHandlerBase<FieldValueCreatedEvent>> logger,
            IEfRepository<TransientPimDbContext, FieldValue> efRepository) : base(eventStore, logger)
        {
            _eventStore = eventStore;
            _efRepository = efRepository;
        }
        protected override async Task ExecuteAsync(FieldValueCreatedEvent @event)
        {
            var fieldValue = FieldValueFactory.CreateFromFieldType(@event.FieldType, @event.FieldValue);
            fieldValue.Id = @event.FieldValueId;
            fieldValue.FieldId = @event.FieldId;
            fieldValue.EntityId = @event.EntityId;
            await _efRepository.AddAsync(fieldValue);
            await _efRepository.SaveChangesAsync();
        }
    }
}
