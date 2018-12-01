using Harvey.CRMLoyalty.Application.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.AppSettings.Commands.DeleteAppSettingCommandHandler
{
    public class DeleteAppSettingCommand : BaseRequest
    {
        public string AppSettingId { get; set; }
        public string UserName { get; set; }
    }
}
