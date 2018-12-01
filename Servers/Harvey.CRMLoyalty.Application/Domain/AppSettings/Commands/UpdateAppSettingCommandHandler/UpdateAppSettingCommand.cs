using Harvey.CRMLoyalty.Application.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.AppSettings.Commands.UpdateAppSettingCommandHandler
{
    public class UpdateAppSettingCommand : BaseRequest
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public string Comment { get; set; }
    }
}
