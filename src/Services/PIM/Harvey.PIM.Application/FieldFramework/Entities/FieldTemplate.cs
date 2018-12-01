using Harvey.Domain;
using Harvey.PIM.Application.Infrastructure.Enums;
using System.Collections.Generic;

namespace Harvey.PIM.Application.FieldFramework.Entities
{
    public class FieldTemplate : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public FieldTemplateType Type { get; set; } = FieldTemplateType.WithVariant;
        public virtual ICollection<Field_FieldTemplate> Field_FieldTemplates { get; set; }
    }
}
