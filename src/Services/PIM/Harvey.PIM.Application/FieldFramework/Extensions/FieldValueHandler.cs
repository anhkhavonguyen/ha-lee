using Harvey.Domain;
using Harvey.PIM.Application.FieldFramework.Entities;

namespace Harvey.PIM.Application.FieldFramework.Extensions
{
    public static class FieldValueHandler
    {
        public static void SetData(FieldType fieldType, FieldValue fieldValue, string value)
        {
            switch (fieldType)
            {
                case FieldType.RichText:
                    fieldValue.RichTextValue = value;
                    break;
                case FieldType.Checkbox:
                    fieldValue.BooleanValue = bool.Parse(value);
                    break;
                case FieldType.Numeric:
                    fieldValue.NumericValue = decimal.Parse(value);
                    break;
                case FieldType.Tags:
                    fieldValue.TagsValue = value;
                    break;
                case FieldType.PredefinedList:
                    fieldValue.PredefinedListValue = value;
                    break;
                case FieldType.EntityReference:
                    fieldValue.EntityReferenceValue = value;
                    break;
                default:
                    fieldValue.TextValue = value;
                    break;
            }
        }
    }
}
