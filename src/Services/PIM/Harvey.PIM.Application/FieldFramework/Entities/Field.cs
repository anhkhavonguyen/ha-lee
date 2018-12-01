using Harvey.Domain;
using System;
using System.Collections.Generic;

namespace Harvey.PIM.Application.FieldFramework.Entities
{
    public class Field : EntityBase, IAuditable
    {
        public Field() : base()
        {

        }
        public Field(FieldType type)
        {
            Type = type;
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public FieldType Type { get; set; }
        public string DefaultValue { get; set; }
        public virtual ICollection<FieldValue> FieldValues { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
