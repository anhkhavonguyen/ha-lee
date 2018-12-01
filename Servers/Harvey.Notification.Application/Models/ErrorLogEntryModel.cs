using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.Notification.Application.Models
{
    public class ErrorLogEntryModel : BaseModel
    {
        public string Detail { get; set; }
        public string Caption { get; set; }
        public int Source { get; set; }
        public string ErrorLogSource { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
