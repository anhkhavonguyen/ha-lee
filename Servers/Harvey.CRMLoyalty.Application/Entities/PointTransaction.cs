using System;
using System.Collections.Generic;

namespace Harvey.CRMLoyalty.Application.Entities
{
    public class PointTransaction : EntityBase
    {
        public string StaffId { get; set; }
        public string OutletId { get; set; }
        public string CustomerId { get; set; }
        public decimal BalanceDebit { get; set; }
        public decimal BalanceCredit { get; set; }
        public decimal BalanceTotal { get; set; }
        public string Comment { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public Boolean Voided { get; set; }
        public string VoidedById { get; set; }
        public int PointTransactionTypeId { get; set; }
        public string IPAddress { get; set; }
        public string BrowserName { get; set; }
        public string Device { get; set; }
        public virtual Staff Staff { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Outlet Outlet { get; set; }
        public virtual Staff VoidedBy { get; set; }
        public virtual PointTransactionType PointTransactionType { get; set; }
        public string PointTransactionReferenceId { get; set; }
        public virtual PointTransaction PointTransactionReference { get; set; }
        public virtual ICollection<PointTransaction> VoidPointTransactions { get; set; }
    }
}
