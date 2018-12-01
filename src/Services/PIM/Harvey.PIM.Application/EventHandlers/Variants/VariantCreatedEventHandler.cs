using Harvey.EventBus;
using Harvey.EventBus.Abstractions;
using Harvey.EventBus.Events.Variants;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure;
using Harvey.PIM.Application.Infrastructure.Domain;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.EventHandlers.Variants
{
    public sealed class VariantCreatedEventHandler : EventHandlerBase<VariantCreatedEvent>
    {
        private readonly IEfRepository<TransientPimDbContext, Variant> _efRepository;
        private readonly IEventStore _eventStore;
        public VariantCreatedEventHandler(
            IEventStore eventStore,
            ILogger<EventHandlerBase<VariantCreatedEvent>> logger,
            IEfRepository<TransientPimDbContext, Variant> efRepository) : base(eventStore, logger)
        {
            _eventStore = eventStore;
            _efRepository = efRepository;
        }

        protected override async Task ExecuteAsync(VariantCreatedEvent @event)
        {
            await _efRepository.AddAsync(new Variant()
            {
                Id = @event.VariantId,
                ProductId = @event.ProductId
            });
            await _efRepository.SaveChangesAsync();

        }
    }
}
