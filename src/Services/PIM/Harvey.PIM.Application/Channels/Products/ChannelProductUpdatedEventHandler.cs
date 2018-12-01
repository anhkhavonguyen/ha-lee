using Harvey.EventBus;
using Harvey.EventBus.Abstractions;
using Harvey.EventBus.Events.Products;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Channels.Services;
using Harvey.PIM.Application.Infrastructure;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Enums;
using Harvey.PIM.MarketingAutomation.Connectors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Channels.Products
{
    public class ChannelProductUpdatedEventHandler : EventHandlerBase<MarketingAutomationEvent<ProductUpdatedEvent>>
    {
        private readonly ConnectorInfoCollection _connectorInfos;
        private readonly IEfRepository<TransientPimDbContext, Channel> _efRepository;
        private readonly IAssignmentService _assignmentService;
        public ChannelProductUpdatedEventHandler(IEventStore eventStore,
                                                ILogger<EventHandlerBase<MarketingAutomationEvent<ProductUpdatedEvent>>> logger,
                                                ConnectorInfoCollection connectorInfos,
                                                IEfRepository<TransientPimDbContext, Channel> efRepository,
                                                IAssignmentService assignmentService) : base(eventStore, logger)
        {
            _connectorInfos = connectorInfos;
            _efRepository = efRepository;
            _assignmentService = assignmentService;
        }
        protected override async Task ExecuteAsync(MarketingAutomationEvent<ProductUpdatedEvent> @event)
        {
            var isAssignemt = _assignmentService.IsAssignment(AssortmentAssignmentType.Product, @event.CorrelationId.Value, Guid.Parse(@event.InnerEvent.AggregateId));
            if (isAssignemt)
            {
                var channel = await _efRepository.GetByIdAsync(@event.CorrelationId.Value);
                var optionsBuilder = new DbContextOptionsBuilder<CatalogDbContext>();
                optionsBuilder.UseNpgsql(channel.ServerInformation);
                using (var dbContext = new CatalogDbContext(optionsBuilder.Options))
                {
                    var entity = dbContext.Products.FirstOrDefault(x => x.Id == Guid.Parse(@event.InnerEvent.AggregateId));
                    if (entity == null)
                    {
                        throw new InvalidOperationException($"product {@event.InnerEvent.Id} is not presented.");
                    }
                    entity.Name = @event.InnerEvent.Name;
                    entity.Description = @event.InnerEvent.Description;
                    dbContext.Products.Update(entity);
                    await dbContext.SaveChangesAsync();
                };
            }
        }
    }
}
