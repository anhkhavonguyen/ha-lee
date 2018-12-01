using System;

namespace Harvey.Polly
{
    public interface IIdempotentPolicy<T>
    {
        bool ShouldHandle(T entity);
        void Handle(T entity);
    }
}
