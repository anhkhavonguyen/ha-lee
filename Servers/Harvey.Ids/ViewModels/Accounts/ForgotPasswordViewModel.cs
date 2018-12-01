using System.ComponentModel.DataAnnotations;

namespace Harvey.Ids.ViewModels.Accounts
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
