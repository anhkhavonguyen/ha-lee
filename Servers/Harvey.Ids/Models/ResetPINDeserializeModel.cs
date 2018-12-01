using System;

namespace Harvey.Ids.Models
{
    public class ResetPINDeserializeModel
    {
        public string UserId { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
