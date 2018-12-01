using System;

namespace Harvey.Domain
{
    public interface IAggregateRoot
    {
        Guid Id { get; set; }
    }
}
