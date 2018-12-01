using Harvey.Domain;
using Harvey.PIM.Application.Infrastructure.Enums;
using System;

namespace Harvey.PIM.Application.Infrastructure.Domain
{
    public class ChannelAssignment: EntityBase
    {
        public Guid ChannelId { get; set; }
        public Guid ReferenceId { get; set; }
        public ChannelAssignmentType EntityType { get; set; }
    }
}
