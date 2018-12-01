using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Harvey.Exception.Extensions;
using Microsoft.Extensions.Logging;

namespace Harvey.PIM.MarketingAutomation
{
    public class Feed<TSource, TTarget> : FeedBase
        where TTarget : FeedItemBase
    {
        private ILogger<Feed<TSource, TTarget>> _logger;
        protected IFeedFetcher<TSource> _fetcher;
        protected IFeedFilter<TSource> _filter;
        protected IFeedConverter<TSource, TTarget> _converter;
        protected IFeedSerializer<TTarget> _serializer;
        private bool _isRunning = false;
        private static readonly object _syncLock = new object();
        public Feed(Guid correlationId, string name) : base(correlationId, name)
        {
        }

        public void SetFetcher(IFeedFetcher<TSource> fetcher)
        {
            _fetcher = fetcher;
        }

        public void SetFilter(IFeedFilter<TSource> filter)
        {
            _filter = filter;
        }

        public void SetConverter(IFeedConverter<TSource, TTarget> converter)
        {
            _converter = converter;
        }

        public void SetSerializer(IFeedSerializer<TTarget> serializer)
        {
            _serializer = serializer;
        }

        public void SetLogger(ILogger<Feed<TSource, TTarget>> logger)
        {
            _logger = logger;
        }

        public void SetScheduler(TimeSpan dueTime, TimeSpan interval)
        {
            DueTime = dueTime;
            Interval = interval;
        }

        public override void Execute()
        {
            if (_isRunning)
            {
                return;
            }
            Task.Factory.StartNew(async () =>
            {
                await Task.Yield();
                _isRunning = true;
                IEnumerable<TSource> source = null;
                try
                {
                    var result = await _fetcher.FetchAsync();
                    if (!result.Any())
                    {
                        return;
                    }
                    source = _filter.Filter(CorrelationId, result);
                    if (_converter.CanConvert(source.GetType()))
                    {
                        var data = _converter.Convert(source).ToList();
                        foreach (var item in data)
                        {
                            item.CorrelationId = CorrelationId;
                        }
                        await _serializer.SerializeAsync(data);
                    }
                }
                catch (System.Exception ex)
                {
                    _logger.LogError(ex.GetTraceLog());
                }
                finally
                {
                    _isRunning = false;
                }
            }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }
    }
}
