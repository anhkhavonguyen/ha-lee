using Harvey.PIM.Application.Infrastructure.Enums;
using System;
using System.Collections.Generic;

namespace Harvey.PIM.Application.Channels.Services
{
    public interface IAssignmentService
    {
        List<Guid> GetAssignmentBy(AssortmentAssignmentType assortmentAssignmentType, Guid channelId);
        bool IsAssignment(AssortmentAssignmentType assortmentAssignmentType, Guid channelId, Guid referenceId);
    }
}
