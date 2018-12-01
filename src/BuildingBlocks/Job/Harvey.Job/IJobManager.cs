using System;

namespace Harvey.Job
{
    public interface IJobManager
    {
        void RegisterRecurringJob<T>(Guid correlationId, string name, Action execution, TimeSpan dueTime, TimeSpan interval)
            where T : IWorker;
    }
}
