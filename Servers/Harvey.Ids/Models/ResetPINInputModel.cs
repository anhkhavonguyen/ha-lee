using System.ComponentModel.DataAnnotations;

namespace Harvey.Ids.Models
{
    public class ResetPINInputModel
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string NewPIN { get; set; }
        [Required]
        public string ConfirmPIN { get; set; }
    }
}
