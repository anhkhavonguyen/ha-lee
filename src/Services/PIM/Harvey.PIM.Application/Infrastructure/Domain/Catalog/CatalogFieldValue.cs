using Harvey.Domain;
using Harvey.PIM.Application.FieldFramework;
using Harvey.PIM.MarketingAutomation;
using System;

namespace Harvey.PIM.Application.Infrastructure.Domain.Catalog
{
    public class CatalogFieldValue: FeedItemBase
    {
        public Guid FieldValueId { get; set; }
        public Guid EntityId { get; set; }
        public Guid FieldId { get; set; }
        public string FieldValue { get; set; }
        public FieldType FieldType { get; set; }
        public string FieldName { get; set; }
        public string Section { get; set; }
        public int OrderSection { get; set; }
        public bool IsVariantField { get; set; }
    }
}
