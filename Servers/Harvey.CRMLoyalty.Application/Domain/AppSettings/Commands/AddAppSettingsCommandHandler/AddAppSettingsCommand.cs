using Harvey.CRMLoyalty.Application.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.AppSettings.Commands.AddAppSettingsCommandHandler
{
    public class AddAppSettingsCommand : BaseRequest
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string GroupName { get; set; }
        public string AppSettingTypeName { get; set; }
       
    }
}
