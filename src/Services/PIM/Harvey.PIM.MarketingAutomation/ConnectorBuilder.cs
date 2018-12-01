using Harvey.Domain;
using Harvey.Job;
using Harvey.PIM.MarketingAutomation.Connectors;
using Harvey.PIM.MarketingAutomation.Enums;
using System;

namespace Harvey.PIM.MarketingAutomation
{
    public class ConnectorBuilder
    {
        private readonly IJobManager _jobManager;
        private readonly ConnectorInfo _connector;
        private readonly IServiceProvider _serviceProvider;
        public ConnectorBuilder(IServiceProvider serviceProvider, IJobManager jobManager, ConnectorInfo connector)
        {
            _serviceProvider = serviceProvider;
            _connector = connector;
            _jobManager = jobManager;
        }

        public ConnectorBuilder AddProductSyncService(Action<SyncServiceBuilder> registration)
        {
            registration(new SyncServiceBuilder(SyncType.Product, _serviceProvider, _connector));
            return this;
        }

        public ConnectorBuilder AddProductFeedService<TSource, TTarget>(Action<FeedServiceBuilder<TSource, TTarget>> registration)
            where TTarget : FeedItemBase
        {
            var buider = new FeedServiceBuilder<TSource, TTarget>(FeedType.Product, _serviceProvider, _connector);
            registration(buider);
            _jobManager.RegisterRecurringJob<FeedWorker>(buider.Feed.CorrelationId, buider.Feed.Name, buider.Feed.Execute, buider.Feed.DueTime, buider.Feed.Interval);
            return this;
        }

        public ConnectorBuilder AddVariantSyncService(Action<SyncServiceBuilder> registration)
        {
            registration(new SyncServiceBuilder(SyncType.Variant, _serviceProvider, _connector));
            return this;
        }

        public ConnectorBuilder AddVariantFeedService<TSource, TTarget>(Action<FeedServiceBuilder<TSource, TTarget>> registration)
          where TTarget : FeedItemBase
        {
            var buider = new FeedServiceBuilder<TSource, TTarget>(FeedType.Variant, _serviceProvider, _connector);
            registration(buider);
            _jobManager.RegisterRecurringJob<FeedWorker>(buider.Feed.CorrelationId, buider.Feed.Name, buider.Feed.Execute, buider.Feed.DueTime, buider.Feed.Interval);
            return this;
        }

        public ConnectorBuilder AddCategorySyncService(Action<SyncServiceBuilder> registration)
        {
            registration(new SyncServiceBuilder(SyncType.Category, _serviceProvider, _connector));
            return this;
        }

        public ConnectorBuilder AddCategoryFeedService<TSource, TTarget>(Action<FeedServiceBuilder<TSource, TTarget>> registration)
          where TTarget : FeedItemBase
        {
            var buider = new FeedServiceBuilder<TSource, TTarget>(FeedType.Category, _serviceProvider, _connector);
            registration(buider);
            _jobManager.RegisterRecurringJob<FeedWorker>(buider.Feed.CorrelationId, buider.Feed.Name, buider.Feed.Execute, buider.Feed.DueTime, buider.Feed.Interval);
            return this;
        }

        public ConnectorBuilder AddPriceFeedService<TSource, TTarget>(Action<FeedServiceBuilder<TSource, TTarget>> registration)
          where TTarget : FeedItemBase
        {
            var buider = new FeedServiceBuilder<TSource, TTarget>(FeedType.Price, _serviceProvider, _connector);
            registration(buider);
            _jobManager.RegisterRecurringJob<FeedWorker>(buider.Feed.CorrelationId, buider.Feed.Name, buider.Feed.Execute, buider.Feed.DueTime, buider.Feed.Interval);
            return this;
        }

        public ConnectorBuilder AddFieldValueSyncService(Action<SyncServiceBuilder> registration)
        {
            registration(new SyncServiceBuilder(SyncType.FieldValue, _serviceProvider, _connector));
            return this;
        }

        public ConnectorBuilder AddPriceSyncService(Action<SyncServiceBuilder> registration)
        {
            registration(new SyncServiceBuilder(SyncType.Price, _serviceProvider, _connector));
            return this;
        }

        public ConnectorInfo GetConnector()
        {
            return _connector;
        }
    }
}
