using System.Threading.Tasks;
using Harvey.EventBus;
using Harvey.EventBus.Abstractions;
using Harvey.EventBus.Events.FileldValues;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.FieldFramework;
using Harvey.PIM.Application.FieldFramework.Entities;
using Harvey.PIM.Application.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Harvey.PIM.Application.EventHandlers.FieldValues
{
    public sealed class ProductFieldValueUpdatedEventHandler : EventHandlerBase<FieldValueUpdatedEvent>
    {
        private readonly IEfRepository<TransientPimDbContext, FieldValue> _efRepository;
        private readonly IEventStore _eventStore;
        public ProductFieldValueUpdatedEventHandler(
            IEventStore eventStore,
            ILogger<EventHandlerBase<FieldValueUpdatedEvent>> logger,
            IEfRepository<TransientPimDbContext, FieldValue> efRepository) : base(eventStore, logger)
        {
            _eventStore = eventStore;
            _efRepository = efRepository;
        }
        protected override async Task ExecuteAsync(FieldValueUpdatedEvent @event)
        {
            var field = await _efRepository.GetByIdAsync(@event.FieldValueId);
            var fieldValue = FieldValueFactory.CreateFromFieldType(@event.FieldType, @event.FieldValue, field);
            await _efRepository.UpdateAsync(fieldValue);
            await _efRepository.SaveChangesAsync();
        }
    }
}
