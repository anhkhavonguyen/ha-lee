using System;

namespace Harvey.Domain
{
    public interface IDomainEvent
    {
        Guid Id { get; set; }
        string AggregateId { get; set; }
        DateTime LastTimeStamp { get; set; }
        Guid CreatedBy { get; set; }
        DateTime CreatedDate { get; set; }
        long Version { get; set; }
    }
}
