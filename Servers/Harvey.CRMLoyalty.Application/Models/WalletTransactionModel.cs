using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Models
{
    public class WalletTransactionModel : BaseModel
    {
        public string StaffId { get; set; }
        public string OutletId { get; set; }
        public string CustomerId { get; set; }
        public decimal BalanceDebit { get; set; }
        public decimal BalanceCredit { get; set; }
        public decimal BalanceTotal { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public string PhoneCustomer { get; set; }
        public string PhoneCountryCode { get; set; }
        public string CustomerName { get; set; }
        public string OutletName { get; set; }
        public string StaffName { get; set; }
        public string Comment { get; set; }
        public Boolean Voided { get; set; }
        public string VoidedById { get; set; }
        public string VoidedBy { get; set; }
        public string IPAddress { get; set; }
        public string BrowserName { get; set; }
        public string Device { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedByName { get; set; }
        public string UpdateByName { get; set; }
        public string CustomerCode { get; set; }
        public string WalletTransactionReferenceId { get; set; }
    }
}
