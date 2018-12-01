using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Entities
{
    public class PointTransactionType 
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public virtual ICollection<PointTransaction> PointTransactions { get; set; }
    }
}
