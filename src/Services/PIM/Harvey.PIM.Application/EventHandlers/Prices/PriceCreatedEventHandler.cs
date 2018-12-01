using Harvey.EventBus;
using Harvey.EventBus.Abstractions;
using Harvey.EventBus.Events.Prices;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure;
using Harvey.PIM.Application.Infrastructure.Domain;
using Microsoft.Extensions.Logging;

using System.Threading.Tasks;

namespace Harvey.PIM.Application.Events.Products
{
    public sealed class PriceCreatedEventHandler : EventHandlerBase<PriceCreatedEvent>
    {
        private readonly IEfRepository<PimDbContext, Price> _efRepository;
        private readonly IEfRepository<PimDbContext, Variant> _variantEfRepository;
        private readonly IEventStore _eventStore;
        public PriceCreatedEventHandler(
            IEventStore eventStore,
            ILogger<EventHandlerBase<PriceCreatedEvent>> logger,
            IEfRepository<PimDbContext, Price> efRepository,
            IEfRepository<PimDbContext, Variant> variantEfRepository) : base(eventStore, logger)
        {
            _eventStore = eventStore;
            _efRepository = efRepository;
            _variantEfRepository = variantEfRepository;
        }
        protected override async Task ExecuteAsync(PriceCreatedEvent @event)
        {
            var price = new Price()
            {
                Id = @event.PriceId,
                StaffPrice = @event.StaffPrice,
                MemberPrice = @event.MemberPrice,
                ListPrice = @event.ListPrice
            };
            await _efRepository.AddAsync(price);
            var variant = await _variantEfRepository.GetByIdAsync(@event.VariantId);
            variant.PriceId = price.Id;
            await _efRepository.SaveChangesAsync();
        }
    }
}
