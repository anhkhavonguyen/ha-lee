using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.AppSettings.Commands.AddAppSettingsCommandHandler
{
    public interface IAddAppSettingsCommandHandler
    {
        Task<string> ExecuteAsync(AddAppSettingsCommand addAppSettingsCommand);
    }
}
