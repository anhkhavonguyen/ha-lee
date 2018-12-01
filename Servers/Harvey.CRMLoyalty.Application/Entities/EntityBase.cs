using System;

namespace Harvey.CRMLoyalty.Application.Entities
{
    public abstract class EntityBase
    {
        public string Id { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedDate { get; set; } = DateTime.UtcNow;

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public string CreatedByName { get; set; }

        public string UpdateByName { get; set; }
    }
}
