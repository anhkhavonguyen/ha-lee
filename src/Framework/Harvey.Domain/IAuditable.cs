using System;

namespace Harvey.Domain
{
    public interface IAuditable
    {
        Guid CreatedBy { get; set; }
        Guid UpdatedBy { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime UpdatedDate { get; set; }
    }
}
