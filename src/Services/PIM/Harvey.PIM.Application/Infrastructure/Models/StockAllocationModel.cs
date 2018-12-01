using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.PIM.Application.Infrastructure.Models
{
    public class StockAllocationModel
    {
        public string GIWDName { get; set; }
        public string GIWDDescription { get; set; }
        public Guid FromLocationId { get; set; }
        public Guid ToLocationId { get; set; }
        public List<AllocationProduct> AllocationProducts { get; set; }
    }

    public class AllocationProduct
    {
        public Guid VariantId { get; set; }
        public Guid StockTypeId { get; set; }
        public int Quantity { get; set; }
    }
}
