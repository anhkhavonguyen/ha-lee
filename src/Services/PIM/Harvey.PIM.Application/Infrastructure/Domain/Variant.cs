using System;
using Harvey.Domain;

namespace Harvey.PIM.Application.Infrastructure.Domain
{
    public class Variant : EntityBase, IAuditable
    {
        public Guid ProductId { get; set; }
        public Guid PriceId { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
