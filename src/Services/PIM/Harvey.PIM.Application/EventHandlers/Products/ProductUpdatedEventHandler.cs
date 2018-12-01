using System;
using System.Threading.Tasks;
using Harvey.EventBus;
using Harvey.EventBus.Abstractions;
using Harvey.EventBus.Events.Products;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Indexing;
using Harvey.Search.Abstractions;
using Microsoft.Extensions.Logging;

namespace Harvey.PIM.Application.EventHandlers.Products
{
    public sealed class ProductUpdatedEventHandler : EventHandlerBase<ProductUpdatedEvent>
    {
        private readonly IEfRepository<TransientPimDbContext, Product> _efRepository;
        private readonly IEventStore _eventStore;
        private readonly ISearchService _searchService;
        public ProductUpdatedEventHandler(
            IEventStore eventStore,
            ILogger<EventHandlerBase<ProductUpdatedEvent>> logger,
            IEfRepository<TransientPimDbContext, Product> efRepository,
            ISearchService searchService) : base(eventStore, logger)
        {
            _eventStore = eventStore;
            _efRepository = efRepository;
            _searchService = searchService;
        }
        protected override async Task ExecuteAsync(ProductUpdatedEvent @event)
        {
            var product = await _efRepository.GetByIdAsync(Guid.Parse(@event.AggregateId));
            product.Name = @event.Name;
            product.Description = @event.Description;
            product.CategoryId = @event.CategoryId;
            await _efRepository.UpdateAsync(product);
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
