using System.Collections.Generic;
using Harvey.PIM.Application.Infrastructure.Domain.Catalog;

namespace Harvey.PIM.Application.Channels.Products
{
    public class CatalogProductFeed : CatalogProduct
    {
        public List<CatalogFieldValue> FieldValues { get; set; } = new List<CatalogFieldValue>();
    }
}
