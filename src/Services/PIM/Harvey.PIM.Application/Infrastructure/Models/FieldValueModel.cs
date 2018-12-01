using Harvey.Domain;
using System;

namespace Harvey.PIM.Application.Infrastructure.Models
{
    public class FieldValueModel
    {
        public Guid Id { get; set; }
        public Guid FieldId { get; set; }
        public string FieldValue { get; set; }
    }

    public class DetailFieldValueModel
    {
        public Guid Id { get; set; }
        public Guid EntityId { get; }
        public Guid FieldId { get; }
        public string FieldValue { get; }
        public FieldType FieldType { get; }
        public string Section { get; }
        public bool IsVariantField { get; }
        public int OrderSection { get; }
        public string FieldName { get; }

        public DetailFieldValueModel(Guid productId, string section, int orderSection, bool isVariantField, FieldType fieldType, Guid fieldId, string fieldName, string fieldValue)
        {
            EntityId = productId;
            FieldValue = fieldValue;
            FieldType = fieldType;
            FieldId = fieldId;
            Section = section;
            IsVariantField = isVariantField;
            OrderSection = orderSection;
            FieldName = fieldName;
        }
    }
}
