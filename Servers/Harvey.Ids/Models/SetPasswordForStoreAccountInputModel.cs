using System.ComponentModel.DataAnnotations;

namespace Harvey.Ids.Models
{
    public class SetPasswordForStoreAccountInputModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
