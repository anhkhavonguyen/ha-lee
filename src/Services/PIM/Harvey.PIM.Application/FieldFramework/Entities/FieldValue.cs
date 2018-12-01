using Harvey.Domain;
using System;

namespace Harvey.PIM.Application.FieldFramework.Entities
{
    public class FieldValue : EntityBase
    {
        public string TextValue { get; set; }
        public string RichTextValue { get; set; }
        public decimal NumericValue { get; set; }
        public string TagsValue { get; set; }
        public string PredefinedListValue { get; set; }
        public string EntityReferenceValue { get; set; }
        public bool BooleanValue { get; set; }
        public Guid EntityId { get; set; }
        public Guid FieldId { get; set; }
        public virtual Field Field { get; set; }

        public static FieldValue Default(Field field)
        {
            return new FieldValue()
            {
                FieldId = field.Id,
                Field = field
            };
        }
    }
}
