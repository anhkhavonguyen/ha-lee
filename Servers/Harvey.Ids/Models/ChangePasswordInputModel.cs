using System.ComponentModel.DataAnnotations;

namespace Harvey.Ids.Models
{
    public class ChangePasswordInputModel
    {
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
