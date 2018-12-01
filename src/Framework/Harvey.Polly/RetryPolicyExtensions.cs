using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System;
using System.Threading.Tasks;

namespace Harvey.Polly
{
    public static class RetryPolicyExtensions
    {
        public static Task ExecuteStrategyAsync(this IRetrivalPolicy retrivalPolicy, ILogger logger, Action execution)
        {
            RetryPolicy retryPolicy = null;
            var policy = Policy.Handle<FakeException>();
            foreach (var type in retrivalPolicy.HandledExceptions)
            {
                policy.Or<Exception>(t => t.GetType() == type.GetType());
            }
            switch (retrivalPolicy.RetrivalStategy)
            {
                case RetrivalStategy.None:
                    retryPolicy = policy.WaitAndRetryAsync(0, retryAttempt => TimeSpan.FromSeconds(0), (ex, time) => { });
                    break;
                case RetrivalStategy.Immediate:
                    retryPolicy = policy.WaitAndRetryAsync(retrivalPolicy.NumbersOfRetrival, retryAttempt => TimeSpan.FromSeconds(0), (ex, time) =>
                    {
                        logger.LogWarning(ex.ToString());
                    });
                    break;
                case RetrivalStategy.Intervals:
                    retryPolicy = policy.WaitAndRetryAsync(retrivalPolicy.NumbersOfRetrival, retryAttempt => TimeSpan.FromSeconds(retryAttempt), (ex, time) =>
                    {
                        logger.LogWarning(ex.ToString());
                    });
                    break;
                case RetrivalStategy.Exponential:
                    retryPolicy = policy.WaitAndRetryAsync(retrivalPolicy.NumbersOfRetrival, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                    {
                        logger.LogWarning(ex.ToString());
                    });
                    break;
            }
            return retryPolicy.ExecuteAsync(() => Task.Run(() => { execution(); }));
        }
    }
}
