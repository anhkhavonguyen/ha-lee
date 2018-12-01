using System.ComponentModel.DataAnnotations;

namespace Harvey.CRMLoyalty.Api.Models
{
    public class InitCustomerProfileInputModel
    {
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string PhoneCountryCode { get; set; }
        public string CreatedBy { get; set; }
        [Required]
        public string OriginalUrl { get; set; }
        public string OutletId { get; set; }
        public string OutletName { get; set; }
    }
}
