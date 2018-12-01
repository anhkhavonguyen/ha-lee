using Microsoft.Extensions.Logging;
using System;

namespace Harvey.Polly
{
    public static class IdempotentPolicyExtensions
    {
        public static void ExecuteStategyAsync<T>(this IIdempotentPolicy<T> idempotentPolicy, T entity, ILogger logger, Action execution)
        {
            if (idempotentPolicy.ShouldHandle(entity))
            {
                idempotentPolicy.Handle(entity);
            }
            else
            {
                execution();
            }
        }
    }
}
