using System;
using System.Collections.Generic;

namespace Harvey.PIM.Application.Infrastructure.Domain.Catalog.Models
{
    public class CatalogVariantModel
    {
        public Guid Id { get; set; }
        public List<CatalogFieldValueModel> Fields { get; set; }
        public CatalogPriceModel Price { get; set; }
    }
}
