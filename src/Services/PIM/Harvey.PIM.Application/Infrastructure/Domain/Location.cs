using Harvey.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.PIM.Application.Infrastructure.Domain
{
    public class Location: EntityBase, IAuditable
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public LocationType Type { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public enum LocationType
    {
        Warehouse = 1,
        Store = 2
    }
}
