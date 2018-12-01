using System.ComponentModel.DataAnnotations;

namespace Harvey.Ids.Models
{
    public class InitAccountMemberInputModel
    {
        [Required]
        public string PhoneCountryCode { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string OriginalUrl { get; set; }
    }
}
