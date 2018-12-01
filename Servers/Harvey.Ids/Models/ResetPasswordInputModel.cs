using System.ComponentModel.DataAnnotations;

namespace Harvey.Ids.Models
{
    public class ResetPasswordInputModel
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
