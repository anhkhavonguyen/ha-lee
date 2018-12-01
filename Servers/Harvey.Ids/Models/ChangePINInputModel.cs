using System.ComponentModel.DataAnnotations;

namespace Harvey.Ids.Models
{
    public class ChangePINInputModel
    {
        [Required]
        public string OldPIN { get; set; }
        [Required]
        public string NewPIN { get; set; }
        [Required]
        [Compare("NewPIN")]
        public string ConfirmPIN { get; set; }
    }
}
