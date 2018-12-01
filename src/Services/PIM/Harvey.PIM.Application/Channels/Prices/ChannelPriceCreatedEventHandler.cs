using Harvey.EventBus;
using Harvey.EventBus.Abstractions;
using Harvey.EventBus.Events.Prices;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Channels.Services;
using Harvey.PIM.Application.Infrastructure;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Domain.Catalog;
using Harvey.PIM.MarketingAutomation.Connectors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Channels.Prices
{
    public class ChannelPriceCreatedEventHandler : EventHandlerBase<MarketingAutomationEvent<PriceCreatedEvent>>
    {
        private readonly ConnectorInfoCollection _connectorInfos;
        private readonly IEfRepository<TransientPimDbContext, Channel> _efRepository;
        private readonly IAssignmentService _assignmentService;

        public ChannelPriceCreatedEventHandler(
                    IAssignmentService assignmentService,
                    IEventStore eventStore,
                    ILogger<EventHandlerBase<MarketingAutomationEvent<PriceCreatedEvent>>> logger,
                    ConnectorInfoCollection connectorInfos,
                    IEfRepository<TransientPimDbContext, Channel> efRepository) : base(eventStore, logger)
        {
            _connectorInfos = connectorInfos;
            _efRepository = efRepository;
            _assignmentService = assignmentService;
        }

        protected override async Task ExecuteAsync(MarketingAutomationEvent<PriceCreatedEvent> @event)
        {
            var channel = await _efRepository.GetByIdAsync(@event.CorrelationId.Value);
            var optionsBuilder = new DbContextOptionsBuilder<CatalogDbContext>();
            optionsBuilder.UseNpgsql(channel.ServerInformation);
            using (var dbContext = new CatalogDbContext(optionsBuilder.Options))
            {
                var entity = dbContext.Variants.FirstOrDefault(x => x.Id == @event.InnerEvent.VariantId);
                if (entity != null)
                {
                    return;
                }
                await dbContext.Prices.AddAsync(new CatalogPrice()
                {
                    Id = @event.InnerEvent.VariantId,
                    ListPrice = @event.InnerEvent.ListPrice,
                    MemberPrice = @event.InnerEvent.MemberPrice,
                    StaffPrice = @event.InnerEvent.StaffPrice,

                });
                await dbContext.SaveChangesAsync();
            };
        }
    }
}
