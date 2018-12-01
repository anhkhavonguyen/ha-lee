using Harvey.Domain;
using System;

namespace Harvey.PIM.Application.FieldFramework.Entities
{
    public class Field_FieldTemplate : EntityBase
    {
        public bool IsVariantField { get; set; }
        public string Section { get; set; }
        public int OrderSection { get; set; }
        public Field Field { get; set; }
        public Guid FieldId { get; set; }
        public FieldTemplate FieldTemplate { get; set; }
        public Guid FieldTemplateId { get; set; }
    }
}
