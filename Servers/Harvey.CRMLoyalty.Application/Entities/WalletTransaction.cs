using System;

namespace Harvey.CRMLoyalty.Application.Entities
{
    public class WalletTransaction: EntityBase
    {
        public string StaffId { get; set; }
        public string CustomerId { get; set; }
        public string OutletId { get; set; }
        public decimal BalanceDebit { get; set; }
        public decimal BalanceCredit { get; set; }
        public decimal BalanceTotal { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public string Comment { get; set; }
        public Boolean Voided { get; set; }
        public string VoidedById { get; set; }
        public string IPAddress { get; set; }
        public string BrowserName { get; set; }
        public string Device { get; set; }
        public virtual Staff Staff { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Outlet Outlet { get; set; }
        public virtual Staff VoidedBy { get; set; }
        public string WalletTransactionReferenceId { get; set; }
        public virtual WalletTransaction WalletTransactionReference { get; set; }
    }
}
