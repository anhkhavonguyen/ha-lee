using Harvey.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.PIM.Application.Infrastructure.Domain
{
    public class Channel : EntityBase, IAuditable
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ServerInformation { get; set; }
        public bool IsProvision { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
