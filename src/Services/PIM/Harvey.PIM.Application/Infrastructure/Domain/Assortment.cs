using Harvey.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.PIM.Application.Infrastructure.Domain
{
    public class Assortment : EntityBase, IAuditable
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
