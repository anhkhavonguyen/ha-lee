using System.ComponentModel.DataAnnotations;

namespace Harvey.Ids.Models
{
    public class ForgotPINInputModel
    {
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string OriginalUrl { get; set; }
    }
}
