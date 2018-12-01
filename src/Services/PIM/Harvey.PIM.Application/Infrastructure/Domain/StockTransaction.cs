using Harvey.Domain;
using System;

namespace Harvey.PIM.Application.Infrastructure.Domain
{
    public class StockTransaction : EntityBase, IAuditable
    {
        public Guid TransactionTypeId { get; set; }
        public Guid StockTypeId { get; set; }
        public Guid VariantId { get; set; }
        public Guid FromLocationId { get; set; }
        public Guid ToLocationId { get; set; }
        public int Quantity { get; set; }
        public int Balance { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}
