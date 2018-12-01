using Harvey.EventBus;
using Harvey.EventBus.Abstractions;
using Harvey.EventBus.Events.Variants;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Channels.Services;
using Harvey.PIM.Application.Infrastructure;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Domain.Catalog;
using Harvey.PIM.Application.Infrastructure.Enums;
using Harvey.PIM.MarketingAutomation.Connectors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Channels.Variants
{
    public class ChannelVariantCreatedEventHandler : EventHandlerBase<MarketingAutomationEvent<VariantCreatedEvent>>
    {
        private readonly ConnectorInfoCollection _connectorInfos;
        private readonly IEfRepository<TransientPimDbContext, Channel> _efRepository;
        private readonly IAssignmentService _assignmentService;

        public ChannelVariantCreatedEventHandler(
                    IAssignmentService assignmentService,
                    IEventStore eventStore,
                    ILogger<EventHandlerBase<MarketingAutomationEvent<VariantCreatedEvent>>> logger,
                    ConnectorInfoCollection connectorInfos,
                    IEfRepository<TransientPimDbContext, Channel> efRepository) : base(eventStore, logger)
        {
            _connectorInfos = connectorInfos;
            _efRepository = efRepository;
            _assignmentService = assignmentService;
        }

        protected override async Task ExecuteAsync(MarketingAutomationEvent<VariantCreatedEvent> @event)
        {
            var isAssignemt = _assignmentService.IsAssignment(AssortmentAssignmentType.Product, @event.CorrelationId.Value, @event.InnerEvent.ProductId);
            if (isAssignemt)
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
                    await dbContext.Variants.AddAsync(new CatalogVariant()
                    {
                        Id = @event.InnerEvent.VariantId,
                        ProductId = @event.InnerEvent.ProductId
                    });
                    await dbContext.SaveChangesAsync();
                };
            }
        }
    }
}
