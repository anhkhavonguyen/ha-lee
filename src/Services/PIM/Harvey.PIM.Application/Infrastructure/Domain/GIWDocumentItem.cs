using Harvey.Domain;
using System;

namespace Harvey.PIM.Application.Infrastructure.Domain
{
    public class GIWDocumentItem : EntityBase
    {
        public Guid GIWDocumentId { get; set; }
        public GIWDocument GIWDocument { get; set; }
        public Guid VariantId { get; set; }
        public Guid StockTypeId { get; set; }
        public StockType StockType { get; set; }
        public int Quantity { get; set; }
    }
}
