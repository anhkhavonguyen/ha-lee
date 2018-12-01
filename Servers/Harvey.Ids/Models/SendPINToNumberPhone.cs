using System.ComponentModel.DataAnnotations;

namespace Harvey.Ids.Models
{
    public class SendPINToNumberPhone
    {
        [Required]
        public string OutletName { get; set; }
        [Required]
        public string CountryCode { get; set; }
        [Required]
        public string NumberPhone { get; set; }
        public string AcronymBrandName { get; set; }
    }
}
