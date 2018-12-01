using System;
using System.Threading;
using Hangfire;

namespace Harvey.Job.Hangfire
{
    public class HangfireJobManager : IJobManager
    {
        public void RegisterRecurringJob<T>(Guid correlationId, string name, Action execution, TimeSpan dueTime, TimeSpan interval)
            where T : IWorker
        {
            RecurringJob.RemoveIfExists(name);
            Thread.Sleep(dueTime.Seconds);
            RecurringJob.AddOrUpdate<T>(name, (worker) => worker.Execute(correlationId, name), Cron.MinuteInterval(interval.Minutes));
            RecurringJob.Trigger(name);
        }
    }
}
