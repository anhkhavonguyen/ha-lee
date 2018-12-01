using System;

namespace Harvey.PIM.Application.Infrastructure.Domain.Catalog.Models
{
    public class CatalogPriceModel
    {
        public Guid Id { get; set; }
        public float ListPrice { get; set; }
        public float StaffPrice { get; set; }
        public float MemberPrice { get; set; }
    }
}
