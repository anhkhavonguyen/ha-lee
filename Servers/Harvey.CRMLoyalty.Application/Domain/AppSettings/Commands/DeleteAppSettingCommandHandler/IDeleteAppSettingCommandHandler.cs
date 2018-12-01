using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.AppSettings.Commands.DeleteAppSettingCommandHandler
{
    public interface IDeleteAppSettingCommandHandler
    {
        Task ExcuteAsync(DeleteAppSettingCommand command);
    }
}
