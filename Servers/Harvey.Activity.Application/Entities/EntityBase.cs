using System;

namespace Harvey.Activity.Application.Entities
{
    public abstract class EntityBase<T>
    {
        public T Id { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public string CreatedByName { get; set; }
    }
}
