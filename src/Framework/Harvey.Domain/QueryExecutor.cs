using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Harvey.Domain
{
    public class QueryExecutor : IQueryExecutor
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<QueryExecutor> _logger;
        public QueryExecutor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = (ILogger<QueryExecutor>)_serviceProvider.GetService(typeof(ILogger<QueryExecutor>));
        }
        public Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query)
        {
            var htype = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            dynamic handler = _serviceProvider.GetService(htype);
            _logger.LogTrace($"[Query Executor] [{handler.GetType().Name}] {JsonConvert.SerializeObject(query)}");
            return handler.Handle((dynamic)query);
        }
    }
}
