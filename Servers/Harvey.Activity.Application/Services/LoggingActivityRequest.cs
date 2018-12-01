using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.Activity.Application.Services
{
    public class LoggingActivityRequest
    {
        public string UserId { get; set; }
        public int ActionAreaPath { get; set; }
        public int ActionType { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
        public string CreatedByName { get; set; }
        public string Value { get; set; }
    }
}
