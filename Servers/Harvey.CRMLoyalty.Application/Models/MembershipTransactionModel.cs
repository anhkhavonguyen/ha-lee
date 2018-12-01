using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Models
{
    public class MembershipTransactionModel : BaseModel
    {
        public string MembershipType { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public string Comment { get; set; }
        public string PhoneCustomer { get; set; }
        public string PhoneCountryCode { get; set; }
        public string CustomerName { get; set; }
        public string OutletName { get; set; }
        public string StaffName { get; set; }
        public string IPAddress { get; set; }
        public string BrowserName { get; set; }
        public string Device { get; set; }
        public string CustomerCode { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Boolean AllowVoid { get; set; }
        public string DoneBy { get; set; }
        public Boolean Voided { get; set; }
        public string VoidedBy { get; set; }
        public DateTime? VoidedDate { get; set; }
        public string CustomerId { get; set; }
        public int MembershipActionType { get; set; }
    }

    public enum MembershipTranactionEnum
    {
        Basic = 1,
        PremiumPlus = 2
    }
}
