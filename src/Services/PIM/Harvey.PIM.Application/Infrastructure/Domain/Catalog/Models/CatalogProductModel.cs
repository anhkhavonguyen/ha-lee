using System;
using System.Collections.Generic;

namespace Harvey.PIM.Application.Infrastructure.Domain.Catalog.Models
{
    public class CatalogProductModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<CatalogVariantModel> Variants { get; set; } = new List<CatalogVariantModel>();
    }
}
