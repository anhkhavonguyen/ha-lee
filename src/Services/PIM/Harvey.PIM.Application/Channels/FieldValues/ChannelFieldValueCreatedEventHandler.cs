using Harvey.EventBus;
using Harvey.EventBus.Abstractions;
using Harvey.EventBus.Events.FileldValues;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Channels.Services;
using Harvey.PIM.Application.Infrastructure;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Domain.Catalog;
using Harvey.PIM.Application.Infrastructure.Enums;
using Harvey.PIM.MarketingAutomation.Connectors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Channels.FieldValues
{
    public class ChannelFieldValueCreatedEventHandler : EventHandlerBase<MarketingAutomationEvent<FieldValueCreatedEvent>>
    {
        private readonly IAssignmentService _assignmentService;
        private readonly ConnectorInfoCollection _connectorInfos;
        private readonly IEfRepository<TransientPimDbContext, Channel> _efRepository;
        public ChannelFieldValueCreatedEventHandler(IEventStore eventStore,
                                                  IAssignmentService assignmentService,
                                                 ILogger<EventHandlerBase<MarketingAutomationEvent<FieldValueCreatedEvent>>> logger,
                                                 ConnectorInfoCollection connectorInfos,
                                                 IEfRepository<TransientPimDbContext, Channel> efRepository) : base(eventStore, logger)
        {
            _connectorInfos = connectorInfos;
            _efRepository = efRepository;
            _assignmentService = assignmentService;
        }

        protected override async Task ExecuteAsync(MarketingAutomationEvent<FieldValueCreatedEvent> @event)
        {
            var idToFind = @event.InnerEvent.IsVariantField ? @event.InnerEvent.EntityId : Guid.Parse(@event.InnerEvent.AggregateId);
            var isAssignemt = _assignmentService.IsAssignment(AssortmentAssignmentType.Product, @event.CorrelationId.Value, idToFind);
            if (isAssignemt)
            {
                var channel = await _efRepository.GetByIdAsync(@event.CorrelationId.Value);
                var optionsBuilder = new DbContextOptionsBuilder<CatalogDbContext>();
                optionsBuilder.UseNpgsql(channel.ServerInformation);
                using (var dbContext = new CatalogDbContext(optionsBuilder.Options))
                {
                    var entity = dbContext.FieldValues.FirstOrDefault(x => x.Id == @event.InnerEvent.FieldValueId);
                    if (entity != null)
                    {
                        return;
                    }
                    await dbContext.FieldValues.AddAsync(new CatalogFieldValue()
                    {
                        FieldValueId = @event.InnerEvent.FieldValueId,
                        EntityId = @event.InnerEvent.EntityId,
                        FieldId = @event.InnerEvent.FieldId,
                        FieldValue = @event.InnerEvent.FieldValue,
                        FieldType = @event.InnerEvent.FieldType,
                        FieldName = @event.InnerEvent.FieldName,
                        Section = @event.InnerEvent.Section,
                        OrderSection = @event.InnerEvent.OrderSection,
                        IsVariantField = @event.InnerEvent.IsVariantField,
                    });

                    await dbContext.SaveChangesAsync();
                };
            }

        }
    }
}
