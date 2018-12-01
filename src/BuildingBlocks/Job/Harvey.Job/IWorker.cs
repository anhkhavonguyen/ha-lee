using System;

namespace Harvey.Job
{
    public interface IWorker
    {
        void Execute(Guid correlationId, string jobName);
    }
}
