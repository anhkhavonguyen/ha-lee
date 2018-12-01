using System.ComponentModel.DataAnnotations;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Commands.InitCustomerProfileCommandHandler
{
    public class InitCustomerProfileCommand
    {
        public string UserId { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneCountryCode { get; set; }
        public string CreatedBy { get; set; }
        public string OriginalUrl { get; set; }
        public string CurrentUserId { get; set; }
        public string OutletId { get; set; }
        public string OutletName { get; set; }
    }
}
