using Harvey.PIM.Application.Infrastructure.Enums;
using System;

namespace Harvey.PIM.Application.Infrastructure.Models
{
    public class AssortmentAssignmentModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public AssortmentAssignmentType Type { get; set; }
    }

    public class AddAssortmentAssignmentModel
    {
        public Guid ReferenceId { get; set; }
        public AssortmentAssignmentType EntityType { get; set; }
    }
}
