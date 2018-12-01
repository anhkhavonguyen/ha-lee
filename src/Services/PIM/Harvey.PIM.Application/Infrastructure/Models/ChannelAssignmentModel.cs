using Harvey.PIM.Application.Infrastructure.Enums;
using System;

namespace Harvey.PIM.Application.Infrastructure.Models
{
    public class ChannelAssignmentModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ChannelAssignmentType Type { get; set; }
    }

    public class AddChannelAssignmentModel
    {
        public Guid ReferenceId { get; set; }
        public ChannelAssignmentType EntityType { get; set; }
    }
}
