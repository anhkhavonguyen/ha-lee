using System;
using System.Collections.Generic;
using System.Linq;
using Harvey.Domain;
using Harvey.PIM.Application.FieldFramework.Entities;
using Harvey.PIM.Application.FieldFramework.Services.Interface;
using Newtonsoft.Json;

namespace Harvey.PIM.Application.FieldFramework
{
    public class FieldValueFactory
    {
        public static FieldValue CreateFromFieldType(FieldType fieldType, string value, FieldValue field = null)
        {
            if (field == null)
            {
                field = new FieldValue();
            }
            switch (fieldType)
            {
                case FieldType.Checkbox:
                    field.BooleanValue = bool.Parse(value);
                    return field;
                case FieldType.EntityReference:
                    field.EntityReferenceValue = value;
                    return field;
                case FieldType.Numeric:
                    field.NumericValue = decimal.Parse(value);
                    return field;
                case FieldType.PredefinedList:
                    field.PredefinedListValue = value;
                    return field;
                case FieldType.RichText:
                    field.RichTextValue = value;
                    return field;
                case FieldType.Tags:
                    field.TagsValue = value;
                    return field;
                case FieldType.Text:
                    field.TextValue = value;
                    return field;
                default:
                    return null;
            }
        }

        public static dynamic GetFromFieldValue(FieldValue fieldValue, IEntityRefService entityRefService)
        {
            switch (fieldValue.Field.Type)
            {
                case FieldType.Checkbox:
                    return new CheckBoxFieldValueModel()
                    {
                        Id = fieldValue.Id,
                        FieldId = fieldValue.FieldId,
                        Name = fieldValue.Field.Name,
                        Value = fieldValue.BooleanValue,
                    };
                case FieldType.EntityReference:
                    return new EntityRefFieldValueModel()
                    {
                        Id = fieldValue.Id,
                        FieldId = fieldValue.FieldId,
                        Name = fieldValue.Field.Name,
                        Value = fieldValue.EntityReferenceValue,
                        Data = entityRefService.GetValuesAsync(JsonConvert.DeserializeObject<EntityRef>(fieldValue.Field.DefaultValue).Id).Result
                    };
                case FieldType.Numeric:
                    return new NumericFieldValueModel()
                    {
                        Id = fieldValue.Id,
                        FieldId = fieldValue.FieldId,
                        Name = fieldValue.Field.Name,
                        Value = fieldValue.NumericValue
                    };
                case FieldType.PredefinedList:
                    return new PredefinedListFieldValueModel()
                    {
                        Id = fieldValue.Id,
                        FieldId = fieldValue.FieldId,
                        Name = fieldValue.Field.Name,
                        Value = fieldValue.PredefinedListValue?.Split(',').ToList(),
                        Data = fieldValue.Field.DefaultValue?.Split(',').ToList()
                    };
                case FieldType.RichText:
                    return new RichTextFieldValueModel()
                    {
                        Id = fieldValue.Id,
                        FieldId = fieldValue.FieldId,
                        Name = fieldValue.Field.Name,
                        Value = fieldValue.RichTextValue
                    };
                case FieldType.Tags:
                    return new TagsFieldValueModel()
                    {
                        Id = fieldValue.Id,
                        FieldId = fieldValue.FieldId,
                        Name = fieldValue.Field.Name,
                        Value = fieldValue.TagsValue
                    };
                case FieldType.Text:
                    return new TextFieldValueModel()
                    {
                        Id = fieldValue.Id,
                        FieldId = fieldValue.FieldId,
                        Name = fieldValue.Field.Name,
                        Value = fieldValue.TextValue
                    };
                default:
                    return null;
            }
        }

        public static string GetFieldValueFromFieldType(FieldType fieldType, FieldValue field)
        {
            switch (fieldType)
            {
                case FieldType.Checkbox:
                    return field.BooleanValue.ToString();
                case FieldType.EntityReference:
                    return field.EntityReferenceValue?.ToString();
                case FieldType.Numeric:
                    return field.NumericValue.ToString();
                case FieldType.PredefinedList:
                    return field.PredefinedListValue?.ToString();
                case FieldType.RichText:
                    return field.RichTextValue?.ToString();
                case FieldType.Tags:
                    return field.TagsValue?.ToString();
                case FieldType.Text:
                    return field.TextValue?.ToString();
                default:
                    return null;
            }
        }
    }

    public abstract class FieldValueModel<TDataType>
    {
        public Guid Id { get; set; }
        public Guid FieldId { get; set; }
        public string Name { get; set; }
        public abstract FieldType Type { get; }
        public TDataType Value { get; set; }
        public dynamic Data { get; set; }
    }

    public class TextFieldValueModel : FieldValueModel<string>
    {
        public override FieldType Type => FieldType.Text;
    }

    public class CheckBoxFieldValueModel : FieldValueModel<bool>
    {
        public override FieldType Type => FieldType.Checkbox;
    }

    public class EntityRefFieldValueModel : FieldValueModel<string>
    {
        public override FieldType Type => FieldType.EntityReference;
    }

    public class NumericFieldValueModel : FieldValueModel<decimal>
    {
        public override FieldType Type => FieldType.Numeric;
    }

    public class PredefinedListFieldValueModel : FieldValueModel<List<string>>
    {
        public override FieldType Type => FieldType.PredefinedList;
    }

    public class RichTextFieldValueModel : FieldValueModel<string>
    {
        public override FieldType Type => FieldType.RichText;
    }

    public class TagsFieldValueModel : FieldValueModel<string>
    {
        public override FieldType Type => FieldType.Tags;
    }
}
