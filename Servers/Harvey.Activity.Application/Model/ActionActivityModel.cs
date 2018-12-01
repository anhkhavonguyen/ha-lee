using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.Activity.Application.Model
{
    public class ActionActivityModel
    {
        public string Id { get; set; }
        public string ActionArea { get; set; }
        public string ActionType { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string Value { get; set; }
    }
}
