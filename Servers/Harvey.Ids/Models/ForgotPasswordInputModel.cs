using System.ComponentModel.DataAnnotations;

namespace Harvey.Ids.Models
{
    public class ForgotPasswordInputModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string OriginalUrl { get; set; }
        public string OutletName { get; set; }
        public string AcronymBrandName { get; set; }
        public string BrandName { get; set; }
    }
}
