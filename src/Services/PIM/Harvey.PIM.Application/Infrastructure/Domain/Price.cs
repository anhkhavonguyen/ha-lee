using Harvey.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.PIM.Application.Infrastructure.Domain
{
    public class Price : EntityBase, IAuditable
    {
        public float ListPrice { get; set; }
        public float StaffPrice { get; set; }
        public float MemberPrice { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
