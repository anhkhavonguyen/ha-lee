using Harvey.Domain;
using Harvey.PIM.Application.Infrastructure.Enums;
using System;

namespace Harvey.PIM.Application.Infrastructure.Domain
{
    public class AssortmentAssignment: EntityBase
    {
        public Guid AssortmentId { get; set; }
        public Guid ReferenceId { get; set; }
        public AssortmentAssignmentType EntityType { get; set; }
    }
}
