using System;
using Harvey.PIM.MarketingAutomation.Connectors;
using Harvey.PIM.MarketingAutomation.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Harvey.PIM.MarketingAutomation
{
    public class FeedServiceBuilder<TSource, TTarget>
        where TTarget : FeedItemBase
    {
        private readonly IServiceProvider _serviceProvider;
        public Feed<TSource, TTarget> Feed;
        private readonly ConnectorInfo _connectorInfo;
        public FeedServiceBuilder(FeedType feedType, IServiceProvider serviceProvider, ConnectorInfo connectorInfo)
        {
            _serviceProvider = serviceProvider;
            _connectorInfo = connectorInfo;
            Feed = new Feed<TSource, TTarget>(_connectorInfo.CorrelationId, $"{feedType.ToString()}_{_connectorInfo.Name}_{_connectorInfo.CorrelationId.ToString()}");
            Feed.SetLogger(_serviceProvider.GetService<ILogger<Feed<TSource, TTarget>>>());
            _connectorInfo.Feeds.Add(Feed);
        }

        public FeedServiceBuilder<TSource, TTarget> UseConverter<TConverter>() where TConverter : IFeedConverter<TSource, TTarget>
        {
            var converter = _serviceProvider.GetService<TConverter>();
            if (converter == null)
            {
                throw new System.Exception($"Cannot resolve converter {nameof(TConverter)} from container.");
            }
            Feed.SetConverter(converter);
            return this;
        }

        public FeedServiceBuilder<TSource, TTarget> UseFetcher<TFetcher>() where TFetcher : IFeedFetcher<TSource>
        {
            var fetcher = _serviceProvider.GetService<TFetcher>();
            if (fetcher == null)
            {
                throw new System.Exception($"Cannot resolve fetcher {nameof(TFetcher)} from container.");
            }
            Feed.SetFetcher(fetcher);
            return this;
        }

        public FeedServiceBuilder<TSource, TTarget> UseFilter<TFilter>() where TFilter : IFeedFilter<TSource>
        {
            var filter = _serviceProvider.GetService<TFilter>();
            if (filter == null)
            {
                throw new System.Exception($"Cannot resolve fetcher {nameof(TFilter)} from container.");
            }
            Feed.SetFilter(filter);
            return this;
        }

        public FeedServiceBuilder<TSource, TTarget> UseSerializer<TSerializer>() where TSerializer : IFeedSerializer<TTarget>
        {
            var serializer = _serviceProvider.GetService<TSerializer>();
            if (serializer == null)
            {
                throw new System.Exception($"Cannot resolve serializer {nameof(TSerializer)} from container.");
            }
            Feed.SetSerializer(serializer);
            return this;
        }

        public FeedServiceBuilder<TSource, TTarget> SetScheduler(TimeSpan dueTime, TimeSpan interval)
        {
            Feed.SetScheduler(dueTime, interval);
            return this;
        }
    }
}
