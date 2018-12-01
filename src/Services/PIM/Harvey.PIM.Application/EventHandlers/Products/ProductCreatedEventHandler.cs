using Harvey.EventBus;
using Harvey.EventBus.Abstractions;
using Harvey.EventBus.Events.Products;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Indexing;
using Harvey.Search.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.EventHandlers.Products
{
    public sealed class ProductCreatedEventHandler : EventHandlerBase<ProductCreatedEvent>
    {
        private readonly IEfRepository<TransientPimDbContext, Product> _efRepository;
        private readonly IEventStore _eventStore;
        private readonly ISearchService _searchService;
        public ProductCreatedEventHandler(
            IEventStore eventStore,
            ILogger<EventHandlerBase<ProductCreatedEvent>> logger,
            IEfRepository<TransientPimDbContext, Product> efRepository,
            ISearchService searchService) : base(eventStore, logger)
        {
            _eventStore = eventStore;
            _efRepository = efRepository;
            _searchService = searchService;
        }

        protected override async Task ExecuteAsync(ProductCreatedEvent @event)
        {
            await _efRepository.AddAsync(new Product()
            {
                Id = Guid.Parse(@event.AggregateId),
                Name = @event.Name,
                CategoryId = @event.CategoryId,
                Description = @event.Description,
                FieldTemplateId = @event.FieldTemplateId
            });
            await _efRepository.SaveChangesAsync();

            var document = new ProductSearchIndexedItem(new ProductSearchItem(Guid.NewGuid())
            {
                Name = @event.Name,
                Description = @event.Description,
                IndexingValue = @event.IndexingValue
            });
            await _searchService.AddAsync(document);
        }
    }
}
