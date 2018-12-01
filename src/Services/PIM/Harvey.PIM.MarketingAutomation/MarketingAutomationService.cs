using Harvey.EventBus.Abstractions;
using Harvey.EventBus.Events.Categories;
using Harvey.EventBus.Events.FileldValues;
using Harvey.EventBus.Events.Prices;
using Harvey.EventBus.Events.Products;
using Harvey.EventBus.Events.Variants;
using Harvey.EventBus.Publishers;
using Harvey.PIM.MarketingAutomation.EventHandlers;

namespace Harvey.PIM.MarketingAutomation
{
    public class MarketingAutomationService
    {
        private readonly IEventBus _eventBus;
        public MarketingAutomationService(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void RegisterProductCreated()
        {
            _eventBus.AddSubcription<ProductPublisher, ProductCreatedEvent, MarketingProductCreatedEventHandler>();
        }

        public void RegisterProductUpdated()
        {
            _eventBus.AddSubcription<ProductPublisher, ProductUpdatedEvent, MarketingProductUpdatedEventHandler>();
        }

        public void RegisterCategoryCreated()
        {
            _eventBus.AddSubcription<ProductPublisher, CategoryCreatedEvent, MarketingCategoryCreatedEventHandler>();
        }

        public void RegisterCategoryUpdated()
        {
            _eventBus.AddSubcription<ProductPublisher, CategoryUpdatedEvent, MarketingCategoryUpdatedEventHandler>();
        }

        public void RegisterVariantCreated()
        {
            _eventBus.AddSubcription<ProductPublisher, VariantCreatedEvent, MarketingVariantCreatedEventHandler>();
        }

        public void RegisterFieldValueCreated()
        {
            _eventBus.AddSubcription<ProductPublisher, FieldValueCreatedEvent, MarketingFieldValueCreatedEventHandler>();
        }

        public void RegisterFieldValueUpdated()
        {
            _eventBus.AddSubcription<ProductPublisher, FieldValueUpdatedEvent, MarketingFieldValueUpdatedEventHandler>();
        }

        public void RegisterPriceCreated()
        {
            _eventBus.AddSubcription<ProductPublisher, PriceCreatedEvent, MarketingPriceCreatedEventHandler>();
        }

    }
}
