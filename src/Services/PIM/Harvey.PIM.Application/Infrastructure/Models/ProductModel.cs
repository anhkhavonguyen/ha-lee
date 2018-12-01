using Harvey.PIM.Application.Infrastructure.Enums;
using System;
using System.Collections.Generic;

namespace Harvey.PIM.Application.Infrastructure.Models
{
    public class ProductModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid FieldTemplateId { get; set; }
        //TODO need to return FieldTemplate object
        public string FieldTemplateName { get; set; }
        public List<Section> Sections { get; set; } = new List<Section>();
        public List<VariantModel> Variants { get; set; } = new List<VariantModel>();
        public Guid CategoryId { get; set; }

        public class Section : SectionModel
        {
            public List<dynamic> FieldValues = new List<dynamic>();
        }

        public class VariantModel : SectionModel
        {
            public Guid Id { get; set; }
            public List<dynamic> FieldValues = new List<dynamic>();
            public PriceModel Price { get; set; }
        }
    }

    public class AddProductModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
        public string Description { get; set; }
        public Guid FieldTemplateId { get; set; }
        public List<FieldValueModel> ProductFields { get; set; }
        public List<AddVariantModel> Variants { get; set; }


        public class AddVariantModel
        {
            public Guid Id { get; set; }
            public List<FieldValueModel> VariantFields { get; set; } = new List<FieldValueModel>();
            public PriceModel Prices { get; set; }
            public ItemActionType Action { get; set; } = ItemActionType.Get;
        }
    }

    public class UpdateProductModel : AddProductModel
    {

    }
}
