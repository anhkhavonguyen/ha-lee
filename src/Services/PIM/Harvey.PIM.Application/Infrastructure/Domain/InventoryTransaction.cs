using Harvey.Domain;
using System;

namespace Harvey.PIM.Application.Infrastructure.Domain
{
    public class InventoryTransaction : EntityBase, IAuditable
    {
        public Guid TransactionTypeId { get; set; }
        public Guid GIWDocumentId { get; set; }
        public Guid FromLocationId { get; set; }
        public Guid ToLocationId { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
