using System;
using System.Linq;
using System.Threading.Tasks;
using Harvey.EventBus;
using Harvey.EventBus.Abstractions;
using Harvey.EventBus.Events.Products;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Channels.Services;
using Harvey.PIM.Application.Infrastructure;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Domain.Catalog;
using Harvey.PIM.Application.Infrastructure.Enums;
using Harvey.PIM.MarketingAutomation.Connectors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Harvey.PIM.Application.Channels.Products
{
    public class ChannelProductCreatedEventHandler : EventHandlerBase<MarketingAutomationEvent<ProductCreatedEvent>>
    {
        private readonly ConnectorInfoCollection _connectorInfos;
        private readonly IEfRepository<TransientPimDbContext, Channel> _efRepository;
        private readonly IAssignmentService _assignmentService;

        public ChannelProductCreatedEventHandler(
            IEventStore eventStore,
            ILogger<EventHandlerBase<MarketingAutomationEvent<ProductCreatedEvent>>> logger,
            ConnectorInfoCollection connectorInfos,
            IEfRepository<TransientPimDbContext, Channel> efRepository,
            IAssignmentService assignmentService
            ) : base(eventStore, logger)
        {
            _connectorInfos = connectorInfos;
            _efRepository = efRepository;
            _assignmentService = assignmentService;
        }

        protected override async Task ExecuteAsync(MarketingAutomationEvent<ProductCreatedEvent> @event)
        {
            var isAssignemt = _assignmentService.IsAssignment(AssortmentAssignmentType.Product, @event.CorrelationId.Value, Guid.Parse(@event.InnerEvent.AggregateId));
            if (isAssignemt)
            {
                var channel = await _efRepository.GetByIdAsync(@event.CorrelationId.Value);
                var optionsBuilder = new DbContextOptionsBuilder<CatalogDbContext>();
                optionsBuilder.UseNpgsql(channel.ServerInformation);
                using (var dbContext = new CatalogDbContext(optionsBuilder.Options))
                {
                    var product = dbContext.Products.FirstOrDefault(x => x.Id == Guid.Parse(@event.InnerEvent.AggregateId));
                    if (product != null)
                    {
                        return;
                    }
                    await dbContext.Products.AddAsync(new CatalogProduct()
                    {
                        Id = Guid.Parse(@event.InnerEvent.AggregateId),
                        Name = @event.InnerEvent.Name,
                        Description = @event.InnerEvent.Description
                    });

                    await dbContext.SaveChangesAsync();
                };
            }
        }
    }
}
