using System;
using Harvey.EventBus;
using Harvey.EventBus.Abstractions;
using Harvey.EventBus.Events.Products;
using Harvey.Persitance.EF;
using Harvey.PIM.Application.Channels.Products;
using Harvey.PIM.Application.Channels.Variants;
using Harvey.PIM.Application.Infrastructure;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.MarketingAutomation;
using Harvey.EventBus.Events.Categories;
using Harvey.PIM.Application.Channels.Categories;
using Harvey.EventBus.Events.FileldValues;
using Harvey.PIM.Application.Channels.FieldValues;
using Harvey.EventBus.Events.Variants;
using Harvey.PIM.Application.Infrastructure.Domain.Catalog;
using Harvey.PIM.Application.Channels.Prices;
using Harvey.EventBus.Events.Prices;

namespace Harvey.PIM.Application.Channels
{
    public class ChannelConnectorInstaller
    {
        private readonly IEfRepository<PimDbContext, Channel> _efRepository;
        private readonly IEventBus _eventBus;
        public ChannelConnectorInstaller(
            IEfRepository<PimDbContext, Channel> efRepository,
            IEventBus eventBus)
        {
            _eventBus = eventBus;
            _efRepository = efRepository;
        }
        public void Install(ApplicationBuilder appBuilder)
        {
            var channels = _efRepository.GetAsync().Result;
            foreach (var item in channels)
            {
                if (item.IsProvision)
                {
                    appBuilder.AddConnector(item.Id, item.Name, _eventBus, (connectorRegistration) =>
                    {
                        connectorRegistration
                        .AddProductSyncService(productSyncServiceRegistration =>
                        {
                            productSyncServiceRegistration
                            .UseSyncHandler<MarketingAutomationEvent<ProductCreatedEvent>, ChannelProductCreatedEventHandler>()
                            .UseSyncHandler<MarketingAutomationEvent<ProductUpdatedEvent>, ChannelProductUpdatedEventHandler>();
                        })
                        .AddProductFeedService<ProductFeed, CatalogProductFeed>(productFeedServiceRegistration =>
                        {
                            productFeedServiceRegistration
                            .UseFetcher<ChannelProductFetcher>()
                            .UseFilter<ChannelProductFilter>()
                            .UseConverter<ChannelProductConveter>()
                            .UseSerializer<ChannelProductSerializer>()
                            .SetScheduler(new TimeSpan(0, 0, 5), new TimeSpan(0, 1, 0));
                        })
                        .AddVariantSyncService(variantSyncServiceRegistration =>
                        {
                            variantSyncServiceRegistration
                            .UseSyncHandler<MarketingAutomationEvent<VariantCreatedEvent>, ChannelVariantCreatedEventHandler>();
                        })
                        .AddVariantFeedService<Variant, CatalogVariant>(productFeedServiceRegistration =>
                        {
                            productFeedServiceRegistration
                            .UseFetcher<ChannelVariantFetcher>()
                            .UseFilter<ChannelVariantFilter>()
                            .UseConverter<ChannelVariantConveter>()
                            .UseSerializer<ChannelVariantSerializer>()
                            .SetScheduler(new TimeSpan(0, 0, 5), new TimeSpan(0, 1, 0));
                        })
                        .AddCategorySyncService(categorySyncServiceRegistration =>
                        {
                            categorySyncServiceRegistration
                            .UseSyncHandler<MarketingAutomationEvent<CategoryCreatedEvent>, ChannelCategoryCreatedEventHandler>()
                            .UseSyncHandler<MarketingAutomationEvent<CategoryUpdatedEvent>, ChannelCategoryUpdatedEventHandler>();
                        })
                        .AddCategoryFeedService<Category, CatalogCategory>(productFeedServiceRegistration =>
                        {
                            productFeedServiceRegistration
                            .UseFetcher<ChannelCategoryFetcher>()
                            .UseFilter<ChannelCategoryFilter>()
                            .UseConverter<ChannelCategoryConveter>()
                            .UseSerializer<ChannelCategorySerializer>()
                            .SetScheduler(new TimeSpan(0, 0, 5), new TimeSpan(0, 1, 0));
                        })
                        .AddFieldValueSyncService(fieldValueSyncServiceRegistration =>
                        {
                            fieldValueSyncServiceRegistration
                            .UseSyncHandler<MarketingAutomationEvent<FieldValueCreatedEvent>, ChannelFieldValueCreatedEventHandler>()
                            .UseSyncHandler<MarketingAutomationEvent<FieldValueUpdatedEvent>, ChannelFieldValueUpdatedEventHandler>();
                        })
                         .AddPriceSyncService(priceSyncServiceRegistration =>
                         {
                             priceSyncServiceRegistration
                             .UseSyncHandler<MarketingAutomationEvent<PriceCreatedEvent>, ChannelPriceCreatedEventHandler>();
                         })
                        .AddPriceFeedService<Price, CatalogPrice>(priceFeedServiceRegistration =>
                         {
                             priceFeedServiceRegistration
                             .UseFetcher<ChannelPriceFetcher>()
                             .UseFilter<ChannelPriceFilter>()
                             .UseConverter<ChannelPriceConveter>()
                             .UseSerializer<ChannelPriceSerializer>()
                             .SetScheduler(new TimeSpan(0, 0, 5), new TimeSpan(0, 1, 0));
                         });
                    });
                }

            }
        }
    }
}
