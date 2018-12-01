using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Models
{
    public class AppSettingModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string GroupName { get; set; }
        public int AppSettingTypeId { get; set; }
        public string AppSettingType { get; set; }
    }
}
